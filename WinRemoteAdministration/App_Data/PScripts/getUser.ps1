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

Try {
    Get-ADUser $User -Properties $Properties
} Catch {
    return -1;
}
