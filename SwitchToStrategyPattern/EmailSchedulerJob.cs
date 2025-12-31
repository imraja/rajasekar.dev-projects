namespace SwitchToStrategyPattern;

public class EmailSchedulerJob : ISchedulerJob
{
    public void Run()
    {
        Console.WriteLine("Running Email Scheduler Job...");
        Console.WriteLine("Sending scheduled emails to users.");
    }
}
