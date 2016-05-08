$Computers = @()

Get-ADComputer -Filter * |

ForEach-Object {
    $Properties = @{
        'Name' = $_.Name;
        'DnsHostName' = $_.DnsHostName;
        'Status' = 'unknown';
    }

    $Computer = New-Object -TypeName PSObject -Prop $Properties

    if ($_.DnsHostName -ne $null) {           
        $rtn = Test-Connection -CN $_.dnshostname -Count 1 -BufferSize 16 -Quiet
        if ($rtn -match "True") {
           $Computer.Status = 'online'
        } else {
           $Computer.Status = 'offline' 
        }
    }
    
    $Computers += $Computer 
}

$Computers