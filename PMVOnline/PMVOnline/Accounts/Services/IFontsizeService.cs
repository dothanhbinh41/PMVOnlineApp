using System;
using Xamarin.Forms;

namespace PMVOnline.Accounts.Services
{
    public enum Fontsize
    {
        Small, Normal, Large
    }

    public interface IFontsizeService
    {
        void ChangeFontsize(Fontsize font = Fontsize.Normal);
        Fontsize CurrentSize { get; }
    }

    public class FontsizeService : IFontsizeService
    {
        public Fontsize CurrentSize => (Fontsize)Xamarin.Essentials.Preferences.Get("_fontsize_", (int)Fontsize.Normal);

        public void ChangeFontsize(Fontsize font = Fontsize.Normal)
        {
            switch (font)
            {
                case Fontsize.Small:
                    Application.Current.Resources["FontSizeSmall"] = 8;
                    Application.Current.Resources["FontSizeNormal"] = 11;
                    Application.Current.Resources["FontSizeTitle"] = 14;
                    break;
                case Fontsize.Large:
                    Application.Current.Resources["FontSizeSmall"] = 14;
                    Application.Current.Resources["FontSizeNormal"] = 18;
                    Application.Current.Resources["FontSizeTitle"] = 22;
                    break;
                case Fontsize.Normal:
                default:
                    Application.Current.Resources["FontSizeSmall"] = 11;
                    Application.Current.Resources["FontSizeNormal"] = 14;
                    Application.Current.Resources["FontSizeTitle"] = 17;
                    break;
            }
            SaveSetting(font);
        }

        void SaveSetting(Fontsize font = Fontsize.Normal)
        {
            Xamarin.Essentials.Preferences.Set("_fontsize_", (int)font);
        }
    }
}
