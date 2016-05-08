$Properties = @(
    'SamAccountName',
    'Name',
    'Enabled'
)

Get-ADUser -Filter * -Properties $Properties | Sort-Object SamAccountName | Select-Object -property $Properties