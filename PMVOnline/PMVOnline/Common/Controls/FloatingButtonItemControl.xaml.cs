using Sharpnado.Shades;
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
    public partial class FloatingButtonItemControl : ContentView
    {
        public static BindableProperty ColorProperty = BindableProperty.Create(
                 nameof(Color),
                 typeof(Color),
                 typeof(FloatingButtonItemControl),
                 default(Color),
                 BindingMode.TwoWay
                 );

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly BindableProperty IconProperty = BindableProperty.Create(
            nameof(Icon),
            typeof(ImageSource),
            typeof(FloatingButtonItemControl),
            default(ImageSource),
            BindingMode.OneWay,
            propertyChanged: HandleIconChanged
            );

        private static void HandleIconChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FloatingButtonItemControl bt)
            {
                bt.SetIcon();
            }
        }

        void SetIcon()
        {
            ItemFloatingBtn.Source = Icon;
        }

        public ImageSource Icon
        {
            set => SetValue(IconProperty, value);
            get => (ImageSource)GetValue(IconProperty);
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
          nameof(Title),
          typeof(string),
          typeof(FloatingButtonItemControl),
          default(string),
          BindingMode.TwoWay
          );


        public string Title
        {
            set => SetValue(TitleProperty, value);
            get => (string)GetValue(TitleProperty);
        }

        public static BindableProperty CommandProperty = BindableProperty.Create(
              nameof(Command),
              typeof(ICommand),
              typeof(FloatingButtonItemControl),
               null,
              BindingMode.OneWay
              );
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }


        public static BindableProperty CommandParameterProperty = BindableProperty.Create(
           nameof(CommandParameter),
           typeof(object),
           typeof(FloatingButtonItemControl),
           default(object),
           BindingMode.OneWay
           );
        public object CommandParameter
        {
            get => (object)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }


        public FloatingButtonItemControl()
        {
            InitializeComponent(); 
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            if (Parent is FloatingButton btn)
            {
                btn.Col();
            }
            if (Command?.CanExecute(CommandParameter) == true)
            {
                Command.Execute(CommandParameter);
            }
        }
    }
}