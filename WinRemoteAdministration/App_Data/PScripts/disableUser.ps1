#Disable User account in Active directory
#Input params: User - User login

param (
    [string][ValidatePattern("^[^<*>]+$")]$User
)

Disable-ADAccount -Identity $User