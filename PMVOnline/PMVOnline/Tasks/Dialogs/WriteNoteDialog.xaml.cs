using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PMVOnline.Tasks.Dialogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WriteNoteDialog : ContentView
    {
        public WriteNoteDialog()
        {
            InitializeComponent();
        }
    }
}