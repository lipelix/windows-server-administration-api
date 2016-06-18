#Start service according to its name
#Input params: Name - Service name

param (
    [string][ValidatePattern("^[^<*>]+$")]$Name
)

Start-Service $Name