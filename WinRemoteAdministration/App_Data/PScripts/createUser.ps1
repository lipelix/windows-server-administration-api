param (
    [string][ValidatePattern("^[a-zA-Z0-9_ ]+$")]$Name,
    [string][ValidatePattern("^[a-zA-Z0-9_]+$")]$Login,
    [string][ValidatePattern("^[a-zA-Z0-9]+$")]$Password
)

New-ADUser -Name $Name -SamAccountName $Login -AccountPassword (ConvertTo-SecureString -AsPlainText $Password -Force) -Enabled $true -ChangePasswordAtLogon $true