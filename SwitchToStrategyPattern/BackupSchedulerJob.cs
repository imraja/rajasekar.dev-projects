namespace SwitchToStrategyPattern;

public class BackupSchedulerJob : ISchedulerJob
{
    public void Run()
    {
        Console.WriteLine("Running Backup Scheduler Job...");
        Console.WriteLine("Performing database backup.");
    }
}
