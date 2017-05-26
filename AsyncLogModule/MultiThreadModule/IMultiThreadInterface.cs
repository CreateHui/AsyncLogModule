
namespace MultiThreadModule
{
    /// <summary>
    /// Multi thread module interface definition
    /// 多线程模块接口定义
    /// </summary>
    public interface IMultiThreadInterface
    {
        void Start();
        void FillCommand(ModuleCommand command);
    }
}
