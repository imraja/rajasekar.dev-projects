using SwitchToStrategyPattern;


Console.WriteLine("Scheduler Job Runner");
Console.WriteLine("Available jobs: email, backup, report");
Console.Write("Enter job type: ");

string? jobType = Console.ReadLine()?.ToLower();

ISchedulerJob? job = jobType switch
{
    "email" => new EmailSchedulerJob(),
    "backup" => new BackupSchedulerJob(),
    "report" => new ReportSchedulerJob(),
    _ => null
};

if (job != null)
{
    job.Run();
}
else
{
    Console.WriteLine($"Unknown job type: {jobType}");
    Console.WriteLine("Valid options are: email, backup, report");
}
