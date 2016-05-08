param (
    [string][ValidatePattern("^[^<*>]+$")]$Name
)

Stop-Service $Name