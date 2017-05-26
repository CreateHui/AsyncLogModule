using AsyncLogModule;
using System;

namespace AsyncLogModuleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogAgent logConsoleAgent = new LogConsoleAgent();
            logConsoleAgent.Initial(new Guid().ToString(), string.Empty);

            LogModule.Instance.Initialize("Async Log Module Console Sample");
            LogModule.Instance.RegisterAgent(logConsoleAgent);
            LogModule.Instance.Start();
            
            Console.WriteLine("Press Enter to show sample...");
            Console.ReadLine();

            LogModule.Instance.AppendLog(LogCategory.Bussiness, LogLevel.Information, "Progam:Main", "This is an Information");
            LogModule.Instance.AppendLog(LogCategory.Bussiness, LogLevel.Warning, "Progam:Main", "This is a Warning");
            LogModule.Instance.AppendLog(LogCategory.Bussiness, LogLevel.Error, "Progam:Main", "This is an Error", 9);
            LogModule.Instance.AppendLog(LogCategory.System, LogLevel.Failed, "Progam:Main", "This is a Failed");
            LogModule.Instance.AppendLog(LogCategory.System, LogLevel.Exception, "Progam:Main", "This is an Exception");
            LogModule.Instance.AppendLog(LogCategory.Tool, LogLevel.Critical, "Progam:Main", "This is a Critical");
            LogModule.Instance.AppendLog(LogCategory.Tool, LogLevel.Illegal, "Progam:Main", "This is an Illegal");

            Console.ReadLine();
        }
    }
}
