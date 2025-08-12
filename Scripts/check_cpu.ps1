# cpu_check.ps1
$cpuLoad = Get-WmiObject -Class Win32_Processor | Measure-Object -Property LoadPercentage -Average
$usage = $cpuLoad.Average

Write-Output ($usage)
