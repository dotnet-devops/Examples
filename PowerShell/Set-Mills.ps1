$importExcel = Get-Module -Name ImportExcel
$excelPath = 'C:\Options\EmailUserReport.xlsx'

$book1 = "Book1"
$book2 = "Book2"

if ($null -eq $importExcel)
{
    Find-Module -Name ImportExcel | Install-Module -Scope CurrentUser # If you have local admin, use -Scope AllUsers
}

Import-Module -Name ImportExcel

# Import the Excel Document. The document should be in the same folder as this script.
$sapInfo = Import-Excel -Path $excelPath -WorksheetName $sapBook  #$PSScriptRoot # If not in the same folder, please provide the exact path.
$o365Info = Import-Excel -Path $excelPath -WorksheetName $book2

foreach ($s in $sapInfo)
{
    [string]$sEmail = $s."Employee - Email (Key)"
    if ([string]::IsNullOrWhiteSpace($sEmail))
    {
        continue;
    }
    
    foreach ($o in $o365Info)
    {
        [string]$oEmail = $o."User Principal Name"
        if ([string]::IsNullOrWhiteSpace($oEmail))
        {
            continue;
        }
        
        if ($sEmail.ToLower() -ne $oEmail.ToLower())
        {
            continue;
        }

        try 
        {
            $o | Add-Member -NotePropertyName "Location" -NotePropertyValue $s."Employee - Personnel Area (Text)" -ErrorAction Stop
        }
        catch
        {
            if ($s.Position -eq "99999999" -or $s.Position -eq 99999999)
            {
                continue;
            }

            $o.Location = $s."Employee - Personnel Area (Text)"
        }
        

        break;

    }
}
$date = [DateTime]::Now.ToShortDateString().Replace("/", "-")
Export-Excel -WorksheetName "Report" -Path ('C:\Options\' + "EmailUserReport-$date.xlsx") -InputObject $o365Info -FreezeTopRow -AutoSize -AutoFilter

