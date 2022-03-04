[bool]$global:pass = $false
try {
    Import-Module VMware.VimAutomation.Core -ErrorAction Stop
    $pass = $true
}
catch {
    Install-Module -scope CurrentUser -SkipPublisherCheck -Name VMware.PowerCLI -Force -AllowClobber
    try{
        Import-Module VMware.VimAutomation.Core -ErrorAction Stop
        $pass = $true
    }
    catch {$pass = $false}
    Set-PowerCLIConfiguration -Scope User -ParticipateInCeip $false
}

$policy = Get-ExecutionPolicy
if ($policy -ne "Unrestricted") {
    try {Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy Unrestricted -Force -ErrorAction Stop}
    catch {[bool]$pass = $false}
}
else {$pass = $true}