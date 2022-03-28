
# call init command with text file import
/bin/bash -c '/root/MoneyTracker init < /scripts/init.txt'

/root/MoneyTracker add -w McDonalds -p 10.75
/root/MoneyTracker add -w Walmart -p 135.00 -c groceries
/root/MoneyTracker add --where Amazon -p 25 --category toys
/root/MoneyTracker add -w McDonalds -p 5.00 -c food -d 25 -m 2 -y 2020 -f cash

if ((/root/MoneyTracker check -c food) -ne "10.75")
{
	Write-Error "Total amount spent on food is wrong"
}

if ((/root/MoneyTracker check -c groceries) -ne "135")
{
	Write-Error "Total amount spent on groceries is wrong"
}

if ((/root/MoneyTracker check -c toys) -ne "25") # console.writeline in c# will ignore the .00 at the end of a whole number even though the variable is a double
{
	Write-Error "Total amount spent on toys is wrong"
}

if ((/root/MoneyTracker check -m 2 -y 2020 -c food) -ne "5")
{
	Write-Error "Total amount spent on food for the second month of 2020 is wrong"
}

/root/MoneyTracker backup

if (!(Test-Path /MoneyTrackerBackup.zip -PathType Leaf))
{
	Write-Error "Backup failed"
}