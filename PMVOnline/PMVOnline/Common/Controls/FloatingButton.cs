using Sharpnado.Shades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PMVOnline.Common.Controls
{
    public class FloatingButton : Grid
    {
        ContentView FloatingBtn { set; get; }
        int DEFAULT_PRIMARY_BUTTON = DeviceInfo.Platform == DevicePlatform.Android ? 48 : 48;
        const int DEFAULT_SPACING = 16;
        double newHeight = 0;
        List<double> heights = new List<double>();
        List<int> margins = new List<int>();

        public static BindableProperty ItemsProperty = BindableProperty.Create(
         nameof(Items),
         typeof(IEnumerable),
         typeof(FloatingButton),
         default(IEnumerable),
         BindingMode.TwoWay,
         propertyChanged: (o, ovl, nvl) =>
         {
             if (o is FloatingButton control)
             {
                 control.SetData();
             }
         });



        public IEnumerable Items
        {
            get => (IEnumerable)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }


        public static BindableProperty IsExpandedProperty = BindableProperty.Create(
         nameof(IsExpanded),
         typeof(bool),
         typeof(FloatingButton),
         default(bool),
         BindingMode.TwoWay
         );



        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }


        public FloatingButton()
        {
            RowDefinitions.Clear();
            ColumnDefinitions.Clear();
            Children.Clear();
            RowSpacing = 0;
            ColumnSpacing = 0;

            var sd = new Shadows
            {
                CornerRadius = DEFAULT_PRIMARY_BUTTON / 2,
                Shades = new List<Shade>
                {
                    new Shade
                    {
                        Color = (Color)Application.Current.Resources["EndPrimary"],
                        Opacity = 0.7,
                        BlurRadius = 6,
                        Offset = new Point(0,5)
                    }
                },
                HeightRequest = DEFAULT_PRIMARY_BUTTON,
                WidthRequest = DEFAULT_PRIMARY_BUTTON,
                Margin = DEFAULT_SPACING / 2,
                VerticalOptions = LayoutOptions.End,
            };
            var frame = new Frame()
            {
                CornerRadius = DEFAULT_PRIMARY_BUTTON / 2,
                Padding = 0,
                BackgroundColor = (Color)Application.Current.Resources["EndPrimary"]
            };
            sd.Content = frame;
            FloatingBtn = sd;

            Label Icons = new Label()
            {
                FontSize = DEFAULT_PRIMARY_BUTTON / 2,
                FontFamily = (string)Application.Current.Resources["FontMaterialdesign"],
                Text = FontIcons.MaterialDesign.Plus,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.White
            };

            frame.Content = Icons;
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += FloatinButtonTapped;
            FloatingBtn.GestureRecognizers.Add(tap);
            Children.Add(sd, 1, 0);

        }


        public void Col()
        {
            FloatinButtonTapped(null, null);
        }

        void FloatinButtonTapped(object sender, EventArgs e)
        {
            if (Items == null) return;
            double expandHeight;
            IsExpanded = !IsExpanded;
            var rotate = IsExpanded ? 45 : 0;

            for (int i = 0; i < Children.Count; i++)
            {
                if (i < Children.Count - 1)
                {
                    Children[i].IsVisible = IsExpanded;
                    expandHeight = IsExpanded ? -72d * (i * 0.75d + 1d) : 0;
                    Children[i].TranslateTo(0, expandHeight, 150, Easing.CubicInOut);
                }
                else
                {
                    FloatingBtn.Content.RotateTo(rotate, 150);
                }
            }
        }

        void SetData()
        {
            if (Items == null)
                return;
            Children.Clear();
            RowDefinitions.Clear();
            ColumnDefinitions.Clear();
            heights.Clear();
            margins.Clear();

            var buttons = new List<Object>();

            foreach (var item in Items)
            {
                buttons.Add(item);
            }
            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            ColumnDefinitions.Add(new ColumnDefinition { Width = DEFAULT_PRIMARY_BUTTON + DEFAULT_SPACING });

            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i] is View flbtn)
                {
                    flbtn.HorizontalOptions = LayoutOptions.End;
                    flbtn.VerticalOptions = LayoutOptions.End;
                    Children.Add(flbtn, 0, 0);
                    Grid.SetColumnSpan(flbtn, 2);
                    var btnSize = flbtn.Measure(double.PositiveInfinity, double.PositiveInfinity);
                    heights.Add(btnSize.Request.Height);
                    if (i == 0) margins.Add(DEFAULT_PRIMARY_BUTTON + DEFAULT_SPACING);
                    else margins.Add(margins[i - 1] + DEFAULT_SPACING + (int)heights[i - 1]);
                    flbtn.IsVisible = false;
                }
            }
            newHeight = margins.LastOrDefault() + heights.LastOrDefault();
            HeightRequest = newHeight;
            Children.Add(FloatingBtn, 1, 0);
        }
    }
}
