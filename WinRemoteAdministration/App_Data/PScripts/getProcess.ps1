#Get running process according to id
#Input params: Id - Process id

param (
    [int]$Id
)

$Properties = 
    'Id',
    'Name',
    @{
      Name='CPU'; 
      Expression={
        ([Math]::Round($_.CPU, 2))
      }
    },
    @{
      Name = 'CPUPercent'
      Expression = {
        $TotalSec = (New-TimeSpan -Start $_.StartTime).TotalSeconds
        [Math]::Round(($_.CPU * 100 / $TotalSec), 2)
      }
    },
    @{
      Name='WS'; 
      Expression={
        ([Math]::Round($_.WS / 1MB, 2))
      }
    },
    @{
      Name='PeakWorkingSet'; 
      Expression={
        ([Math]::Round($_.WS / 1MB, 2))
      }
    },
    @{
      Name = 'StartTime';
      Expression = {($_.StartTime).ToString("dd.MM.yyyy HH:mm:ss")}
    },
    'Responding',
    'Description';

Get-Process -Id $Id | Select-Object $Properties | Sort-Object -Property CPUPercent -Descending