using System.Threading;
namespace ShanoLibraries.MVVM
{
    /// <summary>
    /// Implements the <see cref="UniqueId"/> property to be used for association with an active <see cref="System.Windows.Window"/>
    /// All viewModels to be managed by <see cref="DialogManager"/> must implement this class.
    /// </summary>
    public abstract class ViewModelBase
    { 
        static long IdCounter = long.MinValue;
        public long UniqueId { get; } = Interlocked.Increment(ref IdCounter);
    }
}
