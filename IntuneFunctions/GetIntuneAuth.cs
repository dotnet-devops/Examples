using FunctionLibrary.Standard;
using FunctionLibrary.Standard.Enums;
using FunctionLibrary.Standard.Graph;
using FunctionLibrary.Standard.Models.Graph;
using FunctionLibrary.Standard.Models.Intune;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntuneFunctions
{
    public class GetIntuneAuth
    {
        #region Environment Variables
        private readonly string _clientId;
        private readonly string _clientSecret;

        private readonly string _usersIntuneWindows;

        private readonly string _svcUser;
        private readonly string _svcPwd;
        #endregion

        #region Injects
        private readonly GraphHelper _graph;
        private readonly AuthenticationManager _auth;
        private ILogger _log;
        #endregion

        private string _token = string.Empty;

        private Dictionary<IntuneDeploymentZone, IntuneZoneModel> intuneZones;

        public GetIntuneAuth(GraphHelper graph, AuthenticationManager auth)
        {
            _graph = graph;
            _auth = auth;

            _clientId = Environment.GetEnvironmentVariable("clientId");
            _clientSecret = Environment.GetEnvironmentVariable("clientSecret");

            _svcUser = Environment.GetEnvironmentVariable("svcUser");
            _svcPwd = Environment.GetEnvironmentVariable("svcPwd");

            _usersIntuneWindows = Environment.GetEnvironmentVariable("usersIntuneWindows");

            intuneZones = new Dictionary<IntuneDeploymentZone, IntuneZoneModel>();
            foreach (IntuneDeploymentZone z in Enum.GetValues(typeof(IntuneDeploymentZone)))
            {
                switch (z)
                {
                    case IntuneDeploymentZone.DayZero:
                        intuneZones.Add(z, new IntuneZoneModel(Environment.GetEnvironmentVariable("devicesDayZero"), Environment.GetEnvironmentVariable("usersDayZero"), z));
                        break;
                    case IntuneDeploymentZone.DayFive:
                        intuneZones.Add(z, new IntuneZoneModel(Environment.GetEnvironmentVariable("devicesDayFive"), Environment.GetEnvironmentVariable("usersDayFive"), z));
                        break;
                    case IntuneDeploymentZone.DayTen:
                        intuneZones.Add(z, new IntuneZoneModel(Environment.GetEnvironmentVariable("devicesDayTen"), Environment.GetEnvironmentVariable("usersDayTen"), z));
                        break;
                }
            }
        }

        [FunctionName("GetIntuneAuth")]
        public async Task Run(
            //[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [TimerTrigger("0 0 * * * *")] TimerInfo myTimer,
            ILogger log)
        {
            log.LogInformation("Triggered at (UTC)" + DateTime.UtcNow);
            _log = log;

            // Get Graph Token and Initialize Helper.
            string token = _token == string.Empty ? await _auth.EnsureAccessTokenAsync(new Uri("https://graph.microsoft.com"), _svcUser, _svcPwd, _clientId, _clientSecret) : _token;
            _token = token;
            _graph.ConfigureHttpClient(token);

            var intuneWindowsMembers = await _graph.GetGroupMembers(_usersIntuneWindows);

            // Get the information.
            foreach (IntuneDeploymentZone z in Enum.GetValues(typeof(IntuneDeploymentZone)))
                await GetIntuneZoneInformation(z);

            // Remove old user references in DayTen
            _log.LogInformation("Going through Day 10 Remove User.");
            if (await RemoveOldIntuneMemberships(intuneWindowsMembers, IntuneDeploymentZone.DayTen))
            {
                // If users are removed, refresh each list..
                intuneWindowsMembers = await _graph.GetGroupMembers(_usersIntuneWindows);
                await GetIntuneZoneInformation(IntuneDeploymentZone.DayTen);
            }

            // Add any new users and update the local DayTen object to reflect the new users.
            _log.LogInformation("Going through Day 10 Add User.");
            if (await AddNewUsersToDayTen(intuneWindowsMembers))
                await GetIntuneZoneInformation(IntuneDeploymentZone.DayTen);

            // Check for duplicate memberships.
            _log.LogInformation("Checking for dupes in day 0.");
            await RemoveDuplicateUsers(IntuneDeploymentZone.DayZero);
            _log.LogInformation("Going for dupes in day 5.");
            await RemoveDuplicateUsers(IntuneDeploymentZone.DayFive);

            // Add new devices and remove old ones.
            foreach (IntuneDeploymentZone z in Enum.GetValues(typeof(IntuneDeploymentZone)))
            {
                _log.LogInformation("Running device membership check.");
                await RunDeviceMembershipCheck(intuneZones[z]);
                _log.LogInformation("Running remove device check.");
                await RemoveOldDevices(intuneZones[z]);
            }

            log.LogInformation("Completed loop at (UTC)" + DateTime.UtcNow);

        }

        private async Task<bool> AddNewUsersToDayTen(GroupMembershipModel[] allUsers)
        {
            bool refresh = false;
            foreach (var u in allUsers.Where(user => !intuneZones[IntuneDeploymentZone.DayZero].Users.Select(zero => zero.UserPrincipalName).Contains(user.UserPrincipalName) &&
                                                     !intuneZones[IntuneDeploymentZone.DayFive].Users.Select(zero => zero.UserPrincipalName).Contains(user.UserPrincipalName) &&
                                                     !intuneZones[IntuneDeploymentZone.DayTen].Users.Select(zero => zero.UserPrincipalName).Contains(user.UserPrincipalName)))
            {
                refresh = true;
                _log.LogInformation($"New user added to DayTen: { u.UserPrincipalName }");
                await _graph.AddUserToGroup(intuneZones[IntuneDeploymentZone.DayTen].UserGroupId, u.Id);
            }
            return refresh;
        }

        private async Task CheckDeviceGroupMembership(DeviceModel[] devices, IntuneZoneModel zone)
        {
            foreach (var d in devices.Where(device => device.OperatingSystem == "Windows"))
            {
                // Check if the group already has the device.
                if (zone.Devices.Select(m => m.DisplayName).Contains(d.DeviceName))
                    continue;

                await _graph.AddDeviceToGroup(zone.DeviceGroupId, d.DeviceName);
                _log.LogInformation($"Added { d.DeviceName } to Devices Zone { zone.Zone }");
            }
        }

        private async Task GetIntuneZoneInformation(IntuneDeploymentZone zone)
        {
            intuneZones[zone].Users = await _graph.GetGroupMembers(intuneZones[zone].UserGroupId);
            intuneZones[zone].Devices = await _graph.GetGroupMembers(intuneZones[zone].DeviceGroupId);
        }

        private async Task RemoveDuplicateUsers(IntuneDeploymentZone zone)
        {
            if (await DuplicateUserCheck(zone))
            {
                foreach (IntuneDeploymentZone z in Enum.GetValues(typeof(IntuneDeploymentZone)))
                    await GetIntuneZoneInformation(z);
            }
        }

        private async Task<bool> DuplicateUserCheck(IntuneDeploymentZone reference)
        {
            // Method only references DayZero and DayFive. DayTen is always to be compared against.
            IntuneDeploymentZone target = reference == IntuneDeploymentZone.DayZero ? IntuneDeploymentZone.DayFive : IntuneDeploymentZone.DayZero;
            bool refresh = false;
            foreach (var user in intuneZones[reference].Users)
            {
                if (intuneZones[IntuneDeploymentZone.DayTen].Users.Select(t => t.UserPrincipalName).Contains(user.UserPrincipalName))
                {
                    refresh = true;
                    await RemoveUserFromIntuneGroup(IntuneDeploymentZone.DayTen, user);
                }

                if (target != IntuneDeploymentZone.DayFive)
                    continue;
            }
            return refresh;
        }

        private async Task RemoveUserFromIntuneGroup(IntuneDeploymentZone reference, GroupMembershipModel user)
        {
            try
            {
                _log.LogInformation($"Duplicate user found! { user.UserPrincipalName }");
                await _graph.RemoveMemberFromGroup(intuneZones[reference].UserGroupId, user.Id);
            }
            catch (Exception e)
            {
                _log.LogInformation("Failed to remove duplicate user.");
                _log.LogInformation(e.Message);
            }
        }

        private async Task<bool> RemoveOldIntuneMemberships(GroupMembershipModel[] allUsers, IntuneDeploymentZone zone)
        {
            bool refresh = false;
            foreach (var z in intuneZones[zone].Users.Where(user => !allUsers.Select(a => a.UserPrincipalName).Contains(user.UserPrincipalName)))
            {
                refresh = true;
                await _graph.RemoveMemberFromGroup(intuneZones[zone].UserGroupId, z.Id);
                _log.LogInformation($"Removing user from Zone { zone }: { z.UserPrincipalName }");
            }

            return refresh;
        }

        private async Task<bool> RemoveOldDevices(IntuneZoneModel zone)
        {
            bool refresh = false;
            foreach (var i in zone.Devices.Where(id => !zone.UserDevices.Select(u => u.DeviceName).Contains(id.DisplayName)))
            {
                refresh = true;
                _log.LogInformation($"Removing device: { i.DisplayName }");
                await _graph.RemoveMemberFromGroup(zone.DeviceGroupId, i.Id);
            }
            return refresh;
        }

        public async Task RunDeviceMembershipCheck(IntuneZoneModel zone)
        {
            foreach (var z in zone.Users)
            {
                var userDevices = await _graph.GetUserDevices(z.Id);
                if (userDevices == null)
                    return;

                zone.UserDevices.AddRange(userDevices);
                await CheckDeviceGroupMembership(userDevices, zone);
            }
        }
    }
}