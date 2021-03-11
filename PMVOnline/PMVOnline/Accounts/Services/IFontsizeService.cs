using System;
using Xamarin.Forms;

namespace PMVOnline.Accounts.Services
{
    public enum Fontsize
    {
        //Small  = 0,
        Normal  = 1,
        Large  = 2
    }

    public interface IFontsizeService
    {
        void ChangeFontsize(Fontsize font = Fontsize.Normal);
        Fontsize CurrentSize { get; }
        void Init();
    }

    public class FontsizeService : IFontsizeService
    {
        public Fontsize CurrentSize => (Fontsize)Xamarin.Essentials.Preferences.Get("_fontsize_", (int)Fontsize.Normal);

        public void ChangeFontsize(Fontsize font = Fontsize.Normal)
        {
            switch (font)
            {
                //case Fontsize.Small:
                //    Application.Current.Resources["FontSizeSmall"] = 11;
                //    Application.Current.Resources["FontSizeNormal"] = 13;
                //    Application.Current.Resources["FontSizeTitle"] = 15;
                //    break;
                case Fontsize.Large:
                    Application.Current.Resources["FontSizeSmall"] = 16;
                    Application.Current.Resources["FontSizeNormal"] = 19;
                    Application.Current.Resources["FontSizeTitle"] = 21;
                    break;
                case Fontsize.Normal:
                default:
                    Application.Current.Resources["FontSizeSmall"] = 12;
                    Application.Current.Resources["FontSizeNormal"] = 15;
                    Application.Current.Resources["FontSizeTitle"] = 17;
                    break;
            }
            SaveSetting(font);
        }

        public void Init()
        {
            ChangeFontsize(CurrentSize);
        }

        void SaveSetting(Fontsize font = Fontsize.Normal)
        {
            Xamarin.Essentials.Preferences.Set("_fontsize_", (int)font);
        }
    }
}
