param (
    [string][ValidatePattern("^[^<*>]+$")]$Name
)

Start-Service $Name