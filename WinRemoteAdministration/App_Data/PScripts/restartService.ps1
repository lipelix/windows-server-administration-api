#Restart service according to its name
#Input params: Name - Name of service

param (
    [string][ValidatePattern("^[^<*>]+$")]$Name
)

Restart-Service $Name