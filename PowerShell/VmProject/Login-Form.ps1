if ($passCheck -ne $true) {
    Write-Warning "Prerequisites have not yet been met. Please run 'RUN_ME_FIRST.ps1' located in this folder."
    Read-Host "Press Enter to Exit..."
    Exit
}
Import-Module VMware.VimAutomation.Core -ErrorAction Stop
.".\xaml.ps1"

#DEBUGGING: Already connected?
if ($global:DefaultVIServer.IsConnected) {Initialize-VMData -initial $true}

$tbUsername.add_TextChanged(
    { #Enable the login button.
        $B_login.IsEnabled = ($tbUsername.Text.Length -ne 0) -and ($pbPassword.Password.Length -ne 0)
    }
)

$pbPassword.add_PasswordChanged(
    { #Enable the login button.
        $B_login.IsEnabled = ($tbUsername.Text.Length -ne 0) -and ($pbPassword.Password.Length -ne 0)
    }
)

$B_login.add_Click(
    {
        $lError.Visibility = "Hidden" #Hide the error message, if applicable.
        $username = $tbUsername.Text
        $password = $pbPassword.Password | ConvertTo-SecureString -AsPlainText -Force
        $creds = New-Object pscredential($username,$password)
        try {
            #Login
            if (!$global:DefaultVIServer.IsConnected -or ($global:DefaultVIServer.IsConnected -eq $false)) {$script:connect = Connect-VIServer $Server -Credential $creds -ErrorAction Stop}
            else {Initialize-VMData -initial $true}
            #Change XAML after logging in.
            $tTemplate.IsSelected = $true
            $tbUsername.IsEnabled = $false
            $pbPassword.IsEnabled = $false
            Initialize-VMData -initial $true
            }
        catch {
            $lError.content = $_.Exception.Message
            $lError.Visibility = "Visible"
            }
    }
)

$Form.ShowDialog() | Out-Null

if ($global:DefaultVIServer) {Disconnect-VIServer $connect -Force -Confirm:$false} #Disconnects the created powershell connection.