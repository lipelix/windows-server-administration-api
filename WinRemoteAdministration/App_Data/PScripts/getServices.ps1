#Get all services available in system

$Properties = @(
    'Name',
    'StartType',
    'Status'
)

Get-Service | Sort-Object Name | Select-Object -property $Properties