param([int32] $interval, [string] $root)
$taskName = "Timelines"
$action = New-ScheduledTaskAction -execute "$root\NICE.Timelines.ConsoleApp.exe"
$trigger = New-ScheduledTaskTrigger -Once -At (Get-Date) -RepetitionInterval (New-TimeSpan -Minutes $interval)
$settings = New-ScheduledTaskSettingsSet -StartWhenAvailable
Unregister-ScheduledTask -TaskName $taskName -Confirm:$false -ErrorAction continue
Register-ScheduledTask -TaskName $taskName -User "NT AUTHORITY\SYSTEM" -Trigger $trigger -Action $action -Setting $settings
Set-ScheduledTask $taskName -Trigger $trigger