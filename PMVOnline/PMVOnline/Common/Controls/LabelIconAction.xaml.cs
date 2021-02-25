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
    public partial class LabelIconAction : Frame
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

        public static BindableProperty IconProperty = BindableProperty.Create(
                nameof(Icon),
                typeof(string),
                typeof(LabelIconAction),
                default(string),
                BindingMode.OneWay
                );

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        Color _color { get; set; }
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                bgIcon.BackgroundColor = value;
                lblAction.TextColor = value;
            }
        }

        public string IconAction
        {
            get => lblAction.Text;
            set
            {
                lblAction.Text = value;
                lblAction.IsVisible = !string.IsNullOrEmpty(value);
            }
        }

        public LabelIconAction()
        {
            InitializeComponent();
        }
    }
}