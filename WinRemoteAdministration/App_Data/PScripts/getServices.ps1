$Properties = @(
    'Name',
    'StartType',
    'Status'
)

Get-Service | Sort-Object Name | Select-Object -property $Properties