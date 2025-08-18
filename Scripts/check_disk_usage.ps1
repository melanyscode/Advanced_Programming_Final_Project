$disk = Get-WmiObject -Class Win32_LogicalDisk -Filter "DeviceID='C:'"
$usedPercent = (($disk.Size - $disk.FreeSpace) / $disk.Size) * 100
[math]::Round($usedPercent, 2)
