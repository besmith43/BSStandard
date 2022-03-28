$pwd = (Get-Location).Path
$pwd = $pwd.Replace("\", "/")

docker run -v $pwd/output/self-contained/linux-x64:/root -v $pwd/test:/scripts mcr.microsoft.com/powershell pwsh -F /scripts/test.ps1