#Find-Module ExchangeOnlineManagement | Install-Module # Run as admin. Comment out after installation.
#!IMPORTANT! If you see the console write out a RecipientType with a name containing "Group", you are not getting the total number.
Import-Module ExchangeOnlineManagement

# Config
$o365AdminAccount = ""
[array]$groupsToCount = "Group1", "Group2"

#################### DO NOT EDIT ###################
[int]$total = 0                                    #
[int]$groupTotal = 0                               #
[int]$numberOfGroups = 0                           #
[int]$currentGroupIndex = 0                        #
####################################################

# Function required to gather nested objects.
Function Get-RecipientCount($group)
{
    $localTotal = 0

    # Get the members of the DynamicDistributionGroup. This should be UserMailBox.
    $members = Get-Recipient -RecipientPreviewFilter (Get-DynamicDistributionGroup $group.Id).RecipientFilter

    foreach ($m in $members)
    {
        if ($m.RecipientType -eq "UserMailBox")
        {
            $localTotal += 1;
        }
        else
        {
            $recipientType = $m.RecipientType
            Write-Warning "RecipientType not recognized and will not be counted! RecipientType: $recipientType"
        }
    }

    return $localTotal
}

try 
{

    # Check if the O365AdminAccount has been set. End the script if not.
    if ($o365AdminAccount -eq "") { throw "You must edit this script to include your O365 Admin account"}
    # Connect to ExchangeOnline
    $connection = Connect-ExchangeOnline -UserPrincipalName $o365AdminAccount -ShowProgress $true
    foreach ($g in $groupsToCount)
    {
        # Reset some indexes.
        $currentGroupIndex = 0
        $groupTotal = 0

        # Get the group objects for the target groups.
        $distroGroup = Get-DistributionGroup $g

        # Get the groups nested in each group.
    
        $distroGroupMembers = Get-DistributionGroupMember -Identity $distroGroup.Guid.Guid

        # Run through the French groups.
        $numberOfGroups = $distroGroupMembers.Count
        foreach ($d in $distroGroupMembers)
        {
            $currentGroupIndex += 1
            Write-Host "On group $currentGroupIndex of $numberOfGroups"
            $groupTotal += Get-RecipientCount -group $d
        }
        $total += $groupTotal
        Clear-Host
        Write-Host "$g has $groupTotal members."

    }
    Write-Host "There are $total members."
}
catch
{
    Write-Warning $PSItem
    Write-Warning "The script failed. Please try again after making the appropriate revisions."
}