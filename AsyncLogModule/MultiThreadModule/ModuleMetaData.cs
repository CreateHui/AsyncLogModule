using System;

namespace MultiThreadModule
{
    /// <summary>
    /// Command class used to transfer data with other modules
    /// 用于模块间数据交换的命令类
    /// </summary>
    public class ModuleCommand
    {
        /// <summary>
        /// Unique ID
        /// 唯一ID标识
        /// </summary>
        public string CommandID = string.Empty;

        /// <summary>
        /// User defined Operation Type
        /// 自定义操作类型标识
        /// </summary>
        public int CommandOperationType = 0;

        /// <summary>
        /// User defined data
        /// 自定义数据
        /// </summary>
        public Object CommandData = null;
    }
}
