namespace SwitchToStrategyPattern;

public class ReportSchedulerJob : ISchedulerJob
{
    public void Run()
    {
        Console.WriteLine("Running Report Scheduler Job...");
        Console.WriteLine("Generating and sending reports.");
    }
}
