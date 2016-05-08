$Properties = 
    'Id',
    'Name',
    'CPU',
    @{
      Name = 'CPUPercent'
      Expression = {
        $TotalSec = (New-TimeSpan -Start $_.StartTime).TotalSeconds
        [Math]::Round(($_.CPU * 100 / $TotalSec), 2)
      }
    },
    'Description',
    @{
      Name='WS'; 
      Expression={
        ([Math]::Round($_.WS / 1MB, 2))
      }
    }


Get-Process | Select-Object -Property $Properties | Sort-Object -Property CPUPercent -Descending