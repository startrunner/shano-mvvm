using System;

namespace ShanoLibraries.MVVM
{
    public interface IDialogManager
    {
        /// <summary>
        /// Sets a <see cref="FrameworkElement"/>'s <see cref="FrameworkElement.DataContext"/> property to <paramref name="viewModel"/>
        /// and shows it on screen
        /// </summary>
        /// <param name="viewModel">The <see cref="FrameworkElement.DataContext"/> of the view</param>
        /// <param name="behavior">
        /// <see cref="WindowShowBehavior.Dialog"/>: Blocks all other windows (or the window associated with <paramref name="ownerViewModel"/>, if <paramref name="ownerViewModel"/>'s value is not null) until closed
        /// <see cref="WindowShowBehavior.Window"/>: Does not block other windows
        /// </param>
        /// <param name="ownerViewModel">The <see cref="ViewModelBase"/> whose associated window will be blocked by the dialog</param>
        /// <param name="onClosed">An <see cref="Action"/> to be invoked when closing the window</param>
        /// <returns>
        /// <see langword="true"/> or <see langword="false"/>, depending on whether or not a dialog is successful
        /// <see langword="null"/>, when <paramref name="behavior"/> is <see cref="WindowShowBehavior.Window"/>
        /// </returns>
        bool? Show(
            ViewModelBase viewModel,
            WindowShowBehavior behavior = WindowShowBehavior.Dialog,
            ViewModelBase ownerViewModel = null,
            Action onClosed = null
        );

        void Block(ViewModel viewModel, ViewModelBase ownerViewModel);
    }
}
