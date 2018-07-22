using System;

namespace AlexanderIvanov.ShanoMVVM
{
    public interface IDialogManager
    {
        bool? Show(
            object viewModel,
            WindowShowBehavior behavior = WindowShowBehavior.Dialog,
            ViewModel owner = null,
            Action onClosed = null
        );
    }
}
