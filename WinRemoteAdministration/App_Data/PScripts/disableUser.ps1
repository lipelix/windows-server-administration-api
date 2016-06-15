param (
    [string][ValidatePattern("^[^<*>]+$")]$User
)

Disable-ADAccount -Identity $User