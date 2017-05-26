using System.Collections.Generic;
using System.Threading;

namespace MultiThreadModule
{
    /// <summary>
    /// Multi thread safe command queue
    /// 线程安全的命令队列
    /// </summary>
    public class ModuleCommandQueue
    {
        private Queue<ModuleCommand> m_CommandQueue;
        private ManualResetEvent m_Event = new ManualResetEvent(false);

        public ModuleCommandQueue()
        {
            m_CommandQueue = new Queue<ModuleCommand>();
        }

        /// <summary>
        /// Add command data to queue
        /// 添加命令到队列里
        /// </summary>
        /// <param name="command">command data - 命令数据</param>
        public void AppendCommand(ModuleCommand command)
        {
            lock (m_CommandQueue)
            {
                m_CommandQueue.Enqueue(command);
                m_Event.Set();
            }
        }

        /// <summary>
        /// Pop one command and return it if there is command in the queue, or block the thread until there is command add into the queue.
        /// 将一个命令数据弹出队列并返回，否则，阻塞线程，直到有命令数据加入到命令队列。
        /// </summary>
        /// <returns>command data - 命令数据</returns>
        public ModuleCommand PopCommand()
        {
            if (m_CommandQueue.Count == 0)
            {
                m_Event.Reset();
                m_Event.WaitOne();
            }

            lock (m_CommandQueue)
            {
                return m_CommandQueue.Dequeue();
            }
        }
    }
}
