using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PMVOnline.Tasks.ViewModels
{
    public class CommentViewModel : ViewModelBase
    {
        public string Comment { get; set; } 
        public ObservableCollection<FileModel> Files { get; set; }

        readonly INavigationService navigationService;

        public CommentViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters); 
            Files = new ObservableCollection<FileModel>();
        }
         
        ICommand _AddFileCommand;
        public ICommand AddFileCommand => _AddFileCommand = _AddFileCommand ?? new AsyncCommand(ExecuteAddFileCommand);
        async Task ExecuteAddFileCommand()
        {
            var files = await FilePicker.PickMultipleAsync(PickOptions.Default);
            if (files?.Any() == true)
            {
                foreach (var item in files)
                {
                    Files.Add(new FileModel { ContentType = item.ContentType, FileName = item.FileName, FullPath = item.FullPath });
                }
            }
        }

        ICommand _RemoveFileCommand;
        public ICommand RemoveFileCommand => _RemoveFileCommand = _RemoveFileCommand ?? new Command<FileModel>(ExecuteRemoveFileCommand);
        void ExecuteRemoveFileCommand(FileModel file)
        {
            Files?.Remove(file);
        }
    }
}
