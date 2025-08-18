$os = Get-WmiObject -Class Win32_OperatingSystem
$lastBoot = [System.Management.ManagementDateTimeConverter]::ToDateTime($os.LastBootUpTime)

[int]((New-TimeSpan -Start $lastBoot -End (Get-Date)).TotalMinutes)
