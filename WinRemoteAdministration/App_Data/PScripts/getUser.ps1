param (
    [string][ValidatePattern("^[a-zA-Z0-9._ ]+$")]$User
)

$Properties = 
    'GivenName',
    'SurName',
    'Name',
    'DisplayName',
    'SamAccountName',
    'Enabled',
    @{
      Name = 'Created'
      Expression = {
        $_.Created.ToString("dd.MM.yyyy HH:mm:ss")
      }
    },
    @{
      Name = 'LastLogon'
      Expression = {
        [DateTime]::FromFileTime($_.LastLogon).ToString("dd.MM.yyyy HH:mm:ss")
      }
    },
    @{
      Name = 'AccountExpirationDate'
      Expression = {
       $_.AccountExpirationDate.ToString("dd.MM.yyyy HH:mm:ss")
      }
    },
    @{
      Name = 'PasswordLastSet'
      Expression = {
        $_.PasswordLastSet.ToString("dd.MM.yyyy HH:mm:ss")
      }
    },
    'EmailAddress'

Get-ADUser $User -Properties * | Select $Properties