#Stop service according its name
#Input params: Name - Service name

param (
    [string][ValidatePattern("^[^<*>]+$")]$Name
)

Stop-Service $Name