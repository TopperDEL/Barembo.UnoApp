Write-Host “RUNNING POST-CLONE”
Invoke-WebRequest https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile "C:\hostedtoolcache\windows\NuGet\4.9.4\x64\Nuget.exe"