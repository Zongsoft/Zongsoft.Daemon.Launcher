@echo off

SET install="C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe"
SET current=%cd%

%install% /u "%current%\Zongsoft.Daemon.Launcher.exe"
