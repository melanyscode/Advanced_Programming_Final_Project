# memory_check.ps1
$os = Get-CimInstance Win32_OperatingSystem
$total = $os.TotalVisibleMemorySize
$free = $os.FreePhysicalMemory
$used = $total - $free
$usedPercent = ($used / $total) * 100
Write-Output $usedPercent