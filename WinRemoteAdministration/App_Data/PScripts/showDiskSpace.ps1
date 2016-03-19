param (
    [string]$Disk
)

Try {
    $DiskParam = "DeviceID='" + $Disk +":'"
    Get-WmiObject Win32_LogicalDisk -ComputerName $env:COMPUTERNAME -Filter $DiskParam
} Catch {
    return -1
}