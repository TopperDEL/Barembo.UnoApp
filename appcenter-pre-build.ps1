Write-Host �RUNNING PRE-BUILD�
Invoke-WebRequest https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile Nuget.exe