param (
    [string][ValidatePattern("^[^<*>]+$")]$Name
)

Restart-Service $Name