import-module /modules/BSmith.PWSH.dll

$result = Test-Add 2 3

if ($result -eq 5)
{
    Write-Information "it works!"
}
else
{
    Write-Error "you failed"
}