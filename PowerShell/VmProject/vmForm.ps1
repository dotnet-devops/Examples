#We're going to break this script down into parts.

#Part 1: Templates
#Part 2: Hosts
#Part 3: Datastore
#Part 4: VM Name
#Part 5: Adding the VM
#Part 5.5: Removing the VM (Optional)
#Part 6: Submitting the form.
#Part 7: Clear the form (with a button)


#=======================================
# Part 1: Templates
#=======================================
$tbTempFilter.add_TextChanged(
    {
        $tbTempFilter.Text = $tbTempFilter.Text.Replace("\","") #We have to remove \ from the textbox, otherwise the program freaks out.
        $lbTemplates.Items.Clear()
        if ($tbTempFilter.Text -eq "") {$templates | Sort-Object | % {$lbTemplates.Items.Add($_)}}
        else {
            [array]$matched = $templates | Sort-Object | Where {$_.Name -match $tbTempFilter.Text}
            $matched | Sort-Object | % {$lbTemplates.Items.Add($_)}
        }
    }
)

#Enable the Host ListBox button if there's text here.
$lbTemplates.add_SelectionChanged({$B_clear.IsEnabled = $lbHosts.IsEnabled = $tbHostFilter.IsEnabled = $chbSafeView.IsEnabled = $lbTemplates.SelectedItem -ne $null})

#=======================================
# Part 2: Hosts
#=======================================
$tbHostFilter.add_TextChanged(
    {
        $tbHostFilter.Text = $tbHostFilter.Text.Replace("\","") #We have to remove \ from the textbox, otherwise the program freaks out.
        $lbHosts.Items.Clear()

        #This section is bloated, but that's really for readability and redundancy checking.
        if ($tbHostFilter.Text -eq "") { #Is the filter blank? If so, load based on check selection.
            if ($chbSafeView.IsChecked) {$safeHosts | Sort-Object | % {$lbHosts.Items.Add($_)}}
            else {$hosts | Sort-Object | % {$lbHosts.Items.Add($_)}}
        }
        elseif (($tbHostFilter.Text -ne "") -and ($chbSafeView.IsChecked -eq $true)) {
            [array]$matched = $safeHosts | Sort-Object | Where {$_.Name -match $tbHostFilter.Text}
            $matched | Sort-Object | % {$lbHosts.Items.Add($_)}
        }
        elseif (($tbHostFilter.Text -ne "") -and ($chbSafeView.IsChecked -ne $true)) {
                [array]$matched = $hosts | Sort-Object | Where {$_.Name -match $tbHostFilter.Text}
                $matched | Sort-Object | % {$lbHosts.Items.Add($_)}
        }
    }
)

#Select an available item.
$lbHosts.add_SelectionChanged(
    {
        if ($lbHosts.SelectedItem) 
        {
            $cbDatastores.Items.Clear()
            [array]$dsIDs = $lbHosts.SelectedItem.DatastoreIdList
            foreach ($dsID in $dsIDs) {$cbDatastores.items.Add((Get-Datastore -Id $dsID -WarningAction SilentlyContinue | Sort-Object))}
            $cbDatastores.IsEnabled = $cbDatastores.HasItems
            $tbName.IsEnabled = $false
            $tbName.Text = "" #Get rid of the text to fix error checking with the "Add" button.
        }
    }
)

#Check the box. Remove unsafe hosts.
$chbSafeView.add_Unchecked(
    {
        if ($lbHosts.HasItems) {$lbHosts.Items.Clear()}
        $hosts | Sort-Object | % {$lbHosts.Items.Add($_)}
    }
)
#Uncheck the Box. Load all the hosts.
$chbSafeView.add_Checked(
    {
        if ($lbHosts.HasItems) {$lbHosts.Items.Clear()}
        $safeHosts | Sort-Object | % {$lbHosts.Items.Add($_)}
    }
)

#=======================================
# Part 3: Datastores
#=======================================
$cbDatastores.add_SelectionChanged(
    {
        $freeSpace = ""
        $tbName.IsEnabled = $cbSelectedIndex -ne -1
        $cbDatastores.IsEnabled = $cbDatastores.HasItems
        if ($cbDatastores.SelectedItem) {$freeSpace = $cbDatastores.SelectedItem.FreeSpaceGB.ToString("00")}
        $lFreeSpace.Content = "Free Space: $freeSpace GB"
    }
) #The easy part.

#=======================================
# Part 4: VM Name
#=======================================
$tbName.add_TextChanged({$B_addVM.IsEnabled = $tbName.Text.Length -gt 0})



#=======================================
# Part 5: Adding the VM
#=======================================
$B_addVM.add_Click({Add-TargetVM}) #Function located in functions.ps1

#Enable the remove button.
$lbSelected.add_SelectionChanged({$B_removeVM.IsEnabled = $lbSelected.SelectedItem -ne $null})

$B_removeVM.add_Click(
    {
        $lbSelected.Items.Remove($lbSelected.SelectedItem)
        $B_finalize.IsEnabled = $lbSelected.HasItems
    }
)

#=======================================
# Part 5: Creating the VMs
#=======================================

$B_finalize.add_Click(
    {
        Create-VM
        $lbSelected.Items.Clear()
    }
)
$B_log.add_Click({$tbErrorLog.Text = ""})

#=======================================
# Part 7: Clear the form
#=======================================

$B_clear.add_Click(
    {
        Initialize-VMData -initial $false
    }
)
