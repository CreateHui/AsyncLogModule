using MultiThreadModule;
using System;
using System.Collections.Generic;

namespace AsyncLogModule
{
    /// <summary>
    /// Single instance Log module
    /// 单例日志模块
    /// </summary>
    public class LogModule : MultiThreadBase
    {
        private static LogModule m_Instance = null;
        private static readonly object m_Padlock = new object();

        public static LogModule Instance
        {
            get
            {
                if (m_Instance == null)
                    lock (m_Padlock)
                        if (m_Instance == null)
                            m_Instance = new LogModule();

                return m_Instance;
            }
        }

        private LogRecord m_SampleRecord = null;

        private Dictionary<string, ILogAgent> m_LogAgentList = null;

        private LogModule()
        { }

        private LogModuleOperationType mOperationType = LogModuleOperationType.None;

        /// <summary>
        /// Handle the log data from other modules
        /// 处理来自其它模块的日志数据
        /// </summary>
        /// <param name="command">command data - 命令数据</param>
        protected override void HandleCommand(ModuleCommand command)
        {
            mOperationType = (LogModuleOperationType)command.CommandOperationType;
            switch (mOperationType)
            {
                case LogModuleOperationType.SaveRecord:
                    lock (m_Padlock)
                        foreach(ILogAgent logAgent in m_LogAgentList.Values)
                            logAgent.RecordLogData((LogRecord)command.CommandData);                    
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Initial log module
        /// 初始化日志模块
        /// </summary>
        /// <param name="source">the source that produce the log - 产生日志的应用程序标识</param>
        public void Initialize(string source)
        {
            base.Initialize();

            m_SampleRecord = new LogRecord();
            m_SampleRecord.LogSource = source;

            m_LogAgentList = new Dictionary<string, AsyncLogModule.ILogAgent>();
        }

        /// <summary>
        /// Register log agent
        /// 注册日志处理对象
        /// </summary>
        /// <param name="logAgent">agent to handle the log data - 日志存储或传输的代理</param>
        public bool RegisterAgent(ILogAgent logAgent)
        {
            if (m_LogAgentList.ContainsKey(logAgent.AgentID))
                return false;

            lock (m_Padlock)
                m_LogAgentList.Add(logAgent.AgentID, logAgent);

            return true;
        }

        /// <summary>
        /// Register log agent
        /// 注销日志处理对象
        /// </summary>
        /// <param name="agentID">log agent ID - 日志代理唯一编号</param>
        public void UnregisterAgent(string agentID)
        {
            lock (m_Padlock)
                if (m_LogAgentList.ContainsKey(agentID))
                    m_LogAgentList.Remove(agentID);
        }

        /// <summary>
        /// Append log to the module
        /// 添加日志
        /// </summary>
        /// <param name="logCategory">日志大类 - 业务、系统、工具</param>
        /// <param name="logLevel">日志等级 - 警告、信息、异常、错误等</param>
        /// <param name="subModules">产生日志的子模块 - AAA:BBB:CCC</param>
        /// <param name="logContent">日志内容</param>
        /// <param name="logCustomType">用户自定义日志类型</param>
        public void AppendLog(LogCategory logCategory, LogLevel logLevel, string subModules, string logContent, int logCustomType = 0)
        {
            ModuleCommand tCommand = new ModuleCommand();
            tCommand.CommandOperationType = (int)LogModuleOperationType.SaveRecord;

            LogRecord tempRecord = m_SampleRecord.SelfClone();
            tempRecord.LogCategory = (int)logCategory;
            tempRecord.LogLevel = (int)logLevel;
#if _Release
            tempRecord.LogType = (int)RecordType.Release;
#else
            tempRecord.LogType = (int)RecordType.Debug;
#endif

            tempRecord.LogCustomType = logCustomType;
            tempRecord.LogSubModules = subModules;
            tempRecord.LogContent = logContent;

            tCommand.CommandData = tempRecord;

            FillCommand(tCommand);
        }
    }

    public enum RecordType
    {
        /// <summary>
        /// Debug type
        /// 调试类型
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Release type
        /// 发布类型
        /// </summary>
        Release = 2
    }

    /// <summary>
    /// Log record data
    /// 日志数据
    /// </summary>
    public class LogRecord
    {
        public int LogCategory { get; set; }

        public int LogLevel { get; set; }

        public int LogType { get; set; }

        public int LogCustomType { get; set; }

        public string LogSource { get; set; }

        public string LogSubModules { get; set; }

        public string LogContent { get; set; }

        public long LogTimeStamp { get; set; }

        public LogRecord SelfClone()
        {
            LogRecord result = new LogRecord();
            result.LogSource = LogSource;

            result.LogTimeStamp = DateTime.UtcNow.Ticks;

            return result;
        }
    }

    /// <summary>
    /// Log module operation type
    /// 日志模块日志类型
    /// </summary>
    public enum LogModuleOperationType
    {
        None = 0,
        SaveRecord,
    }

    public enum LogCategory
    {
        /// <summary>
        /// 业务分类
        /// </summary>
        Bussiness = 1,

        /// <summary>
        /// 系统分类
        /// </summary>
        System = 2,

        /// <summary>
        /// 工具分类
        /// </summary>
        Tool = 4
    }

    public enum LogLevel
    {
        /// <summary>
        /// 信息：用于日志一般关注的事件
        /// </summary>
        Information = 1,

        /// <summary>
        /// 警告：用于日志潜在风险事件
        /// </summary>
        Warning = 2,

        /// <summary>
        /// 错误：用于日志常规错误，如密码不正确
        /// </summary>
        Error = 4,

        /// <summary>
        /// 失败：用于日志正常操作失败，如远程连接请求失败
        /// </summary>
        Failed = 8,

        /// <summary>
        /// 异常：用于日志未期望发生的事件，如数据格式转换失败、空引用
        /// </summary>
        Exception = 16,

        /// <summary>
        /// 紧急：用于日志已发生严重事件
        /// </summary>
        Critical = 32,

        /// <summary>
        /// 非法：用于日志可能的非法操作事件
        /// </summary>
        Illegal = 64
    }
}
