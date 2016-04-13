param (
    [string]$Name,
    [string]$Login,
    [string]$Password
)

New-ADUser -Name $Name -SamAccountName $Login -AccountPassword (ConvertTo-SecureString -AsPlainText $Password -Force) -Enabled $true -ChangePasswordAtLogon $true