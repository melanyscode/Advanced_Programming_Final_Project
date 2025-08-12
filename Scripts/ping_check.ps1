$ping = Test-Connection -ComputerName "8.8.8.8" -Count 4 -Quiet:$false | 
        Measure-Object -Property ResponseTime -Average
[math]::Round($ping.Average, 2)
