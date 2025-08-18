$os = Get-WmiObject -Class Win32_OperatingSystem
$totalRAM = $os.TotalVisibleMemorySize
$freeRAM = $os.FreePhysicalMemory
$usedRAMPercent = (($totalRAM - $freeRAM) / $totalRAM) * 100
[math]::Round($usedRAMPercent, 2)
