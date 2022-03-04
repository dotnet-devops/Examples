param (
    [string] $AppId,
    [string] $CertPath,
    [string] $CertPwd,
    [string] $Mailbox,
    [string] $Org,
    [string] $User,
)

class GraphMailbox
{
    [string]$Id
    [string]$DisplayName
    [bool]$Elevated
    [string]$Email
}

if ([string]::IsNullOrEmpty($Mailbox) -or [string]::IsNullOrEmpty($CertPwd))
{
    return;
}

try
{
    $shutup = Import-Module ExchangeOnlineManagement -ErrorAction Stop   | Out-Null
}
catch
{
    Install-Module -Name ExchangeOnlineManagement -Scope CurrentUser -Confirm:$false -AllowClobber -Force | Out-Null
}

Connect-ExchangeOnline -CertificateFilePath $CertPath `
                       -CertificatePassword (ConvertTo-SecureString -String $CertPwd -AsPlainText -Force) `
                       -AppID $AppId `
                       -Organization $Org `
                       -ShowBanner:$false
# Please stop the auto-tabs, vscode...
[bool] $success = $true;
[System.Collections.ArrayList] $users = [System.Collections.ArrayList]::new();
try {
    $permissions = Get-EXORecipientPermission -Identity $Mailbox
    foreach ($p in $permissions)
    {
        if ($p.Trustee -eq 'NT AUTHORITY\SELF')
        {
            continue;
        }

        if ([string]::IsNullOrEmpty($p.Trustee))
        {
            continue;
        }

        $email = $p.Trustee;
        
        try 
        {
            $profile = Get-EXOMailbox $email -ErrorAction Stop;
            [GraphMailbox]$user = New-Object GraphMailbox;
            $user.DisplayName = $profile.DisplayName
            $user.Elevated = $p.AccessRights.Contains("SendAs");
            $user.Email = $p.Trustee
            $user.Id = $profile.ExternalDirectoryObjectId
            $users.Add($user) | Out-Null;
        }
        catch 
        {
            continue;
        }
        
    }
}
catch { $success = false; }

Disconnect-ExchangeOnline -Confirm:$false -InformationAction Ignore -ErrorAction SilentlyContinue

if (!$success)
{
    throw "One or more actions have failed.";
}

Write-Output ($users | ConvertTo-Json);