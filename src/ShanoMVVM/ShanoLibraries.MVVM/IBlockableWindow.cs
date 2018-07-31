using System.Windows;

namespace ShanoLibraries.MVVM
{
    public interface IBlockableWindow
    {
        bool IsBlocked { get; }
        void Block(FrameworkElement popup);
        void Unblock();
    }
}
