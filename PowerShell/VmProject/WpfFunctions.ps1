#Alaphabetical... I think...
Function Add-TargetVM 
{
    #Created local variables so the values wouldn't get erased by Initialize-VMData.
    $script:vmName = $tbName.Text
    $vmTemp = $lbTemplates.SelectedItem
    $vmHost = $lbHosts.SelectedItem
    $vmDatastore = $cbDatastores.SelectedItem  

    $lbItem = New-Object System.Windows.Controls.ListBoxItem #Create custom listbox Item.

    #Create custom object in order to add the specific items later on. Also to manipulate the ToString() method.
    $obj = New-Object PSObject 
    $obj | Add-Member -MemberType ScriptMethod -Name ToString -Value {$vmName} -PassThru -Force #Required to set the ToString() method to the textbox name.
    $obj | Add-Member -NotePropertyName Name -NotePropertyValue $vmName #Adding Text out of order for object readability.
    $obj | Add-Member -NotePropertyName Template -NotePropertyValue $vmTemp
    $obj | Add-Member -NotePropertyName Host -NotePropertyValue $vmHost
    $obj | Add-Member -NotePropertyName Datastore -NotePropertyValue $vmDatastore
    $lbItem.Content = $obj
    #Check if the name is used in vCenter and in the listBox.
    if (($vmName -notin $vms.Name) -and ($vmName -notin $lbSelected.Items.Content.Name)) {
        $lbSelected.Items.Add($lbItem)
        $lNameError.Visibility = "Hidden" #Hide the error message.
        }
    else {$lNameError.Visibility = "Visible"} #Produce the error message.

    $lbSelected.IsEnabled = $B_finalize.IsEnabled = $lbSelected.HasItems #Enable finalizing the form if there's something added to the selected ListBox.

    if ($lNameError.Visibility -eq "Hidden") {Initialize-VMData -initial $false}

}

Function Initialize-VMData ([bool]$initial) 
{
    switch ($true) {
        ($tbHostFilter.Text.Length -gt 0) {$tbHostFilter.Text = ""} #Host Filter TextBox
        $lbHosts.HasItems {$lbHosts.Items.Clear()} #Host ListBox
        ($chbSafeView.Checked -eq $false) {$chbSafeView.IsChecked = $true} #Safe View CheckBox.
        $cbDatastores.HasItems {$cbDatastores.Items.Clear()} #Datastore ComboBox
        ($tbName.Text.Length -gt 0) {$tbName.Text = ""} #VM Name TextBox
    }

    $lFreeSpace.Content = "Free Space"
    $tbName.IsEnabled = $cbDatastores.IsEnabled = $chbSafeView.IsEnabled = $lbHosts.IsEnabled = $tbHostFilter.IsEnabled = $false #Disable everything.
           
    #Get VMWare Items
    if ($initial) {
        $script:vms = Get-VM
        $script:templates = Get-Template
        $script:hosts = Get-VMHost
    }
    
    #Create a custom ArrayList for the objects below.
    if (!$safeHosts) {$script:safeHosts = New-Object System.Collections.ArrayList}
    else {$safeHosts.Clear()} #Clear out $safehosts if it was already created.
    #Add the items to their respective controls.
    if ($lbTemplates.HasItems -eq $false) {$templates | Sort-Object | % {$lbTemplates.Items.Add($_) | Out-Null}}
    foreach ($h in $hosts) {
        [array]$filterCheck = $null
        foreach ($filter in $hostFilter) {if ($h.Name -match $filter) {$filterCheck += $true}}
        if ($filterCheck -notcontains $true) {$safeHosts.Add($h) | Out-Null}
    }
    if ($safeHosts.count -gt 0) {$safeHosts | Sort-Object | % {$lbHosts.Items.Add($_) | Out-Null}}
    $tbTempFilter.IsEnabled = $lbTemplates.IsEnabled = $lbTemplates.HasItems #Enable the Template and Template Filter
    $lbHosts.IsEnabled = $lbTemplates.SelectedItem -ne $null
}

Function Create-VM 
{
    foreach ($item in $lbSelected.Items.Content) {
        $vmName = $item.Name
        $vmHost = $item.Host
        $vmDatastore = $item.Datastore
        $vmTemplate = $item.Template

        try {New-VM -Name $vmName -VMHost $vmHost -Datastore $vmDatastore -Template $vmTemplate -ErrorAction Stop -DiskStorageFormat Thin -RunAsync}
        catch {$tbErrorLog.Text += $_.Exception.Message}

        $tbErrorLog.text += Show-Completed
    }
}

#Key Presses
$Form.add_KeyDown({
    param(
        [Parameter(Mandatory)][Object]$sender,
        [Parameter(Mandatory)][Windows.Input.KeyEventArgs]$e
    )
    switch ($e.Key) 
    {
        ("F2" ) {
            if ($B_addVM.IsEnabled) {Add-TargetVM}
        }
        ("F4" ) {
            if ($lbSelected.SelectedItem) {$lbSelected.Items.remove($lbSelected.SelectedItem)} #Remove the selected item, if something is selected.
        }
        ("F5" ) {Initialize-VMData -initial $false}
        ("F8" ) {
            Create-VM
            $lbSelected.Items.Clear()
        }
    }
})

Function Show-Completed 
{
    $time = Get-Date
    $msg =
"
#================================
# Done! $time
#================================
"
    return $msg
}