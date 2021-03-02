using Acr.UserDialogs;
using Prism;
using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Common.Bases
{
    public class ModelBase : BindableBase
    {
    }

    public class ViewModelBase : ModelBase, INavigationAware, IPageLifecycleAware, IInitialize, IDestructible, IInitializeAsync
    {
        public bool IsBusy { get; set; }

        public virtual void Destroy()
        {
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public void OnIsBusyChanged()
        {
            if (IsBusy)
            {
                UserDialogs.Instance.ShowLoading(maskType: MaskType.Black);
            }
            else
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public virtual Task InitializeAsync(INavigationParameters parameters)
        {
            return Task.CompletedTask;
        }
    }

    public class TabViewModelBase : ViewModelBase, IActiveAware
    {
        public event EventHandler IsActiveChanged;

        bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, RaiseIsActiveChanged); }
        }

        public virtual void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }
    }


    public class DialogViewModelBase : BindableBase, IDialogAware
    {
        public virtual event Action<IDialogParameters> RequestClose;

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
