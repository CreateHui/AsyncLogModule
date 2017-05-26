using System;
using System.Threading.Tasks;

namespace MultiThreadModule
{
    /// <summary>
    /// Multi module base class, used to define some common function
    /// 多线程模块基类，用于定义一些公共的操作
    /// </summary>
    public abstract class MultiThreadBase : IMultiThreadInterface
    {
        protected ModuleCommandQueue m_CommandQueue;
        private Task m_WorkingTask;

        /// <summary>
        /// Initialization of members
        /// 初始化成员
        /// </summary>
        /// <returns></returns>
        protected virtual bool Initialize()
        {
            m_CommandQueue = new ModuleCommandQueue();
            m_WorkingTask = new Task(new Action(WorkingFunction));

            return true;
        }

        /// <summary>
        /// Accept command data, add to command queue
        /// 接收命令数据，加入到命令队列中
        /// </summary>
        /// <param name="command">command data - 命令数据</param>
        public void FillCommand(ModuleCommand command)
        {
            m_CommandQueue.AppendCommand(command);
        }
        
        /// <summary>
        /// Start the module
        /// 启动模块
        /// </summary>
        public virtual void Start()
        {
            if (m_WorkingTask.Status != TaskStatus.Running)
                m_WorkingTask.Start();
        }

        /// <summary>
        /// Working function
        /// 工作线程方法
        /// </summary>
        private void WorkingFunction()
        {
            PreThreadWorking();
            HandleCommand();
        }

        /// <summary>
        /// Preparation before the module start, override it when anything need to be done before start the module
        /// 模块启动前的预操作，如果有任何操作需要在模块启动前完成，请在子类中重载此函数
        /// </summary>
        protected virtual void PreThreadWorking()
        {

        }

        /// <summary>
        /// Working function that handle the command, sub-class inherited from this class should override this function to finish user defined behavior
        /// 处理命令数据的方法，继承自此类的子类要重载此方法来实现自定义的命令数据处理行为
        /// </summary>
        /// <param name="command"></param>
        protected virtual void HandleCommand(ModuleCommand command)
        {

        }

        /// <summary>
        /// Command fetch function
        /// 命令获取函数
        /// </summary>
        private void HandleCommand()
        {
            ModuleCommand tCommand;
            while (true)
            {
                tCommand = m_CommandQueue.PopCommand();
                if (tCommand == null)
                    continue;

                HandleCommand(tCommand);
                tCommand = null;
            }
        }
    }
}
