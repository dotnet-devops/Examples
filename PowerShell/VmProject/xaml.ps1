$inputXML = Get-Content ".\gui.xaml"

$inputXML = $inputXML -replace 'mc:Ignorable="d"','' -replace "x:N",'N' -replace '^<Win.*', '<Window'
[void][System.Reflection.Assembly]::LoadWithPartialName('presentationframework')
[xml]$XAML = $inputXML
#Read XAML
Add-Type -AssemblyName System.Windows.Forms #Injected here to load the forms binary.
$reader=(New-Object System.Xml.XmlNodeReader $xaml)
try{$Form=[Windows.Markup.XamlReader]::Load( $reader )}
catch {
    Write-Warning "Unable to parse XML, with error: $($Error[0])`n Ensure that there are NO SelectionChanged or TextChanged properties in your textboxes (PowerShell cannot process them)"
    throw
}
#===========================================================================
# Load XAML Objects In PowerShell
#===========================================================================
  
$xaml.SelectNodes("//*[@Name]") | %{
    try {Set-Variable -Name "$($_.Name)" -Value $Form.FindName($_.Name) -ErrorAction Stop}
    catch{throw}
    }
#===========================================================================
# Import the rest of the script.
#===========================================================================
.".\WpfFunctions.ps1"
.".\vmForm.ps1"

#===========================================================================
# Manipulate the GUI from PowerShell
#===========================================================================

#Buttons
$B_login.IsEnabled = $false
$B_addVM.IsEnabled = $false
$B_removeVM.IsEnabled = $false
$B_clear.IsEnabled = $false
$B_finalize.IsEnabled = $false

#ListBoxes
$lbTemplates.IsEnabled = $false
$lbHosts.IsEnabled = $false

#CheckBoxes
$chbSafeView.IsEnabled = $false
$chbSafeView.IsChecked = $true

#TextBoxes
$tbName.IsEnabled = $false
$tbTempFilter.IsEnabled = $false
$tbHostFilter.IsEnabled = $false

#ComboBox
$cbDatastores.IsEnabled = $false

#Labels
$lNameError.Visibility = "Hidden"

#Filters
$hostFilter = "mfg", "ics" #Filter out substrings for hosts to avoid messing with production.


#Tabs
$tTemplate.Visibility = "Hidden"
$tLogin.Visibility = "Hidden"

$Form.ResizeMode = "NoResize"