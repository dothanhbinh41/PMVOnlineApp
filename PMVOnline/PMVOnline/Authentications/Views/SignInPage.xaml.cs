using PMVOnline.Common.FontIcons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PMVOnline.Authentications.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                txtPassword.IsPassword = !txtPassword.IsPassword; 
                btn.Text = txtPassword.IsPassword ? MaterialDesign.EyeOffOutline : MaterialDesign.EyeOutline;
            }
        }
    }
}