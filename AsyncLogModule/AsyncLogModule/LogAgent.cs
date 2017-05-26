using System;

namespace AsyncLogModule
{
    /// <summary>
    /// Log handler interface
    /// 日志处理类接口
    /// </summary>
    public interface ILogAgent
    {
        string AgentID { get; }

        void RecordLogData(LogRecord logRecord);

        void Initial(string agentID, string targetPath);
    }

    /// <summary>
    /// Console handler agent
    /// 命令行日志代理
    /// </summary>
    public class LogConsoleAgent : ILogAgent
    {
        private string m_AgentID = string.Empty;

        public string AgentID
        {
            get
            {
                return m_AgentID;
            }
        }

        /// <summary>
        /// Set console color by the log level
        /// 根据日志等级设置命令行颜色方案
        /// </summary>
        /// <param name="logLevel">log level - 日志等级</param>
        private void SetConsoleColor(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Information:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Failed:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogLevel.Exception:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                case LogLevel.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                case LogLevel.Illegal:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Reset console color
        /// 重置命令行颜色方案
        /// </summary>
        private void ResetConsoleColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Show log data to the console with color by the log level
        /// 将以颜色区别等级的日志信息显示到命令行中
        /// </summary>
        /// <param name="logRecord">log data - 日志数据</param>
        public void RecordLogData(LogRecord logRecord)
        {
            Console.WriteLine("[" + ((RecordType)logRecord.LogType) + "]");
            SetConsoleColor((LogLevel)logRecord.LogLevel);

            Console.WriteLine("Source : " + logRecord.LogSource);
            Console.WriteLine("SubModules : " + logRecord.LogSubModules);
            Console.WriteLine("Category : " + ((LogCategory)logRecord.LogCategory).ToString());
            Console.WriteLine("Custom Type : " + logRecord.LogCustomType);
            Console.WriteLine("Level : " + ((LogLevel)logRecord.LogLevel).ToString());
            Console.WriteLine("Time Stamp : " + new DateTime(logRecord.LogTimeStamp).ToString());
            Console.WriteLine(logRecord.LogContent);
            Console.WriteLine();

            ResetConsoleColor();
        }

        /// <summary>
        /// Initial members
        /// 初始化成员
        /// </summary>
        /// <param name="agentID">log agent ID - 日志代理唯一编号</param>
        /// <param name="targetPath">set to be string.empty - 置为空字符串</param>
        public void Initial(string agentID, string targetPath)
        {
            m_AgentID = agentID;
        }
    }

    /// <summary>
    /// File handler agent
    /// 文件日志代理
    /// </summary>
    public class LogFileAgent : ILogAgent
    {
        private string m_AgentID = string.Empty;

        public string AgentID
        {
            get
            {
                return m_AgentID;
            }
        }

        private string m_TargetLogFolder = System.Environment.CurrentDirectory + "\\DefaultLogFolder";

        /// <summary>
        /// Save log data to the files
        /// 将日志数据保存到文件中
        /// </summary>
        /// <param name="logRecord">log data - 日志数据<</param>
        public void RecordLogData(LogRecord logRecord)
        {

        }

        /// <summary>
        /// Initial members
        /// 初始化成员
        /// </summary>
        /// <param name="agentID">log agent ID - 日志代理唯一编号</param>
        /// <param name="targetPath">folder to save the log files - 保存日志文件的文件夹路径</param>
        public void Initial(string agentID, string targetPath)
        {
            m_AgentID = agentID;
            m_TargetLogFolder = targetPath;
        }
    }
}
