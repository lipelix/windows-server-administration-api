#Get service according to its name
#Input params: Name - Service name

param (
    [string][ValidatePattern("^[^<*>]+$")]$Name
)

$Properties = @(
    'Name',
    'DisplayName',
    'StartType',
    'Status'
)

Get-Service $Name | Select-Object -property $Properties