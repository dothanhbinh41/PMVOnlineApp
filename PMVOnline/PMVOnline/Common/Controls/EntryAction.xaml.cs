using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PMVOnline.Common.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryAction : Grid
    {
        public static BindableProperty CommandProperty = BindableProperty.Create(
                   nameof(Command),
                   typeof(ICommand),
                   typeof(LabelIconAction),
                   default(ICommand),
                   BindingMode.TwoWay
                   );

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static BindableProperty TextProperty = BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(LabelIconAction),
                default(string),
                BindingMode.OneWay
                );

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static BindableProperty PlaceholderProperty = BindableProperty.Create(
                nameof(Placeholder),
                typeof(string),
                typeof(LabelIconAction),
                default(string),
                BindingMode.OneWay
                );

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public EntryAction()
        {
            InitializeComponent();
            entry.BindingContext = this;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (Command?.CanExecute(null) == true)
            {
                Command.Execute(null);
            }
        }
    }
}