using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PMVOnline.Common.Controls
{
    public class RatingBar : Grid
    {
        public const double UncheckOpacity = 0.2;
        public static BindableProperty TotalRateProperty = BindableProperty.Create(
           nameof(TotalRate),
           typeof(int),
           typeof(RatingBar),
           5,
           BindingMode.TwoWay,
           propertyChanged: (s, o, n) =>
           {
               if (s is RatingBar control)
               {
                   control.SetUpUI();
               }
           });
        public int TotalRate
        {
            get => (int)GetValue(TotalRateProperty);
            set => SetValue(TotalRateProperty, value);
        }


        public static BindableProperty RateProperty = BindableProperty.Create(
           nameof(Rate),
           typeof(int),
           typeof(RatingBar),
           0,
           BindingMode.TwoWay,
           propertyChanged: (s, o, n) =>
           {
               if (s is RatingBar control && n is int newValue && o is int oldValue)
               {
                   control.SetRate(oldValue, newValue);
               }
           });
        public int Rate
        {
            get => (int)GetValue(RateProperty);
            set => SetValue(RateProperty, value);
        }

        public RatingBar()
        {
            SetUpUI();
        }

        void SetUpUI()
        {
            this.Children.Clear();
            this.RowDefinitions.Clear();
            this.ColumnDefinitions.Clear();

            for (int i = 0; i < TotalRate; i++)
            {
                this.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            for (int i = 0; i < TotalRate; i++)
            {
                Image item = new Image
                {
                    Source = "ic_star",
                    Opacity = i < Rate ? 1 : UncheckOpacity,
                    WidthRequest = 24,
                    HeightRequest = 24,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                this.Children.Add(item, i, 0);
                TapGestureRecognizer tapGesture = new TapGestureRecognizer((v) =>
                {
                    Item_Clicked(v);
                });
                item.GestureRecognizers.Add(tapGesture);
            }
        }

        private void Item_Clicked(object sender)
        {
            if (sender is Image item)
            {
                var columnIndex = Grid.GetColumn(item);
                Rate = columnIndex + 1;
            }
        }

        void SetRate(int oldValue, int newValue)
        {
            for (int i = 0; i < TotalRate; i++)
            {
                if (this.Children[i] is Image item)
                {
                    item.Opacity = i < newValue ? 1 : UncheckOpacity;
                }
            }
        }
    }
}
