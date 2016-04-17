Get-PSDrive -PSProvider Filesystem |
    Where-Object {$_.Used -ne 0 -and $_.Free -ne $null} |
    Select-Object -property @{Name="Name";Expression={$_.name}},
        @{Name="Free";Expression={$_.free / 1GB}},
        @{Name="Used";Expression={$_.used / 1GB}},
        @{Name="Total";Expression={($_.used + $_.free) / 1GB}}