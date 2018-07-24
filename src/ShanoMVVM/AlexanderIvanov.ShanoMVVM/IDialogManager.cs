using System;

namespace AlexanderIvanov.ShanoMVVM
{
    public interface IDialogManager
    {
        bool? Show(
            object viewModel,
            WindowShowBehavior behavior = WindowShowBehavior.Dialog,
            IViewModel ownerViewModel = null,
            Action onClosed = null
        );
    }
}
