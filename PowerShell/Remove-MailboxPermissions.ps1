param (
    [string] $AppId,
    [string] $CertPath,
    [string] $CertPwd,
    [string] $Mailbox,
    [string] $Org,
    [switch] $SendAs,
    [string] $User,
)

if ([string]::IsNullOrEmpty($Mailbox) -or [string]::IsNullOrEmpty($User) -or [string]::IsNullOrEmpty($CertPwd))
{
    return;
}

try
{
    Import-Module ExchangeOnlineManagement -ErrorAction Stop
}
catch
{
    Install-Module -Name ExchangeOnlineManagement -Scope CurrentUser -Confirm:$false -AllowClobber -Force
}

Connect-ExchangeOnline -CertificateFilePath $Certpath `
                       -CertificatePassword (ConvertTo-SecureString -String $CertPwd -AsPlainText -Force) `
                       -AppID $AppId `
                       -Organization $Org
# Please stop the auto-tabs, vscode...
[bool] $success = $true;
try {
    Remove-MailboxPermission -Identity $Mailbox -User $User -AccessRights FullAccess -Confirm:$false -ErrorAction Stop
}
catch { $success = false; }

if ($SendAs)
{
    try {
        Remove-RecipientPermission -Identity $Mailbox -Trustee $User -AccessRights SendAs -Confirm:$false -ErrorAction Stop
    }
    catch { $success = $false }
}

Disconnect-ExchangeOnline -Confirm:$false


if (!$success)
{
    throw "One or more actions have failed.";
}