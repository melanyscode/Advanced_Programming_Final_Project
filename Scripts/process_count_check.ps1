# process_count_check.ps1
$processCount = (Get-Process).Count
Write-Output $processCount
