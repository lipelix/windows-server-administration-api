#Get information about free, used and total capacity of system drives unit.

$Properties = 
    @{Name="Name";Expression={$_.Name}},
    @{Name="Free";Expression={$_.Free / 1GB}},
    @{Name="Used";Expression={$_.Used / 1GB}},
    @{Name="Total";Expression={($_.Used + $_.Free) / 1GB}}

Get-PSDrive -PSProvider FileSystem | Where-Object {$_.Free -ne $null} | Select-Object -property $Properties