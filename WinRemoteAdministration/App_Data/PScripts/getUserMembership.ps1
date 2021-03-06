﻿#Get user group membership from Active Directory according to user login
#Input params: User - User login

param (
    [string][ValidatePattern("^[a-zA-Z0-9._ ]+$")]$Name
)

$User = Get-ADUser -Filter{SamAccountName -eq $Name} -Properties MemberOf
foreach ($line in $User.MemberOf) {
    (Get-ADGroup -Filter{DistinguishedName -eq $line} -Properties Name).Name
}