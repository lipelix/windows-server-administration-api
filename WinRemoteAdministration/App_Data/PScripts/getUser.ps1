param (
    [string]$User
)

$Properties = @(
    'DisplayName',
    'SamAccountName'
    'Enabled',
    'Created',
    'AccountExpirationDate',
    'LastLogonDate',
    'PasswordLastSet',
    'EmailAddress'
)

Get-ADUser $User -Properties $Properties
