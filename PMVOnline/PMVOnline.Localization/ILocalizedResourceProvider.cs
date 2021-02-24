using System;
using System.Globalization;
using System.Resources; 
using Prism;
using Xamarin.Forms;

namespace PMVOnline.Localization
{
    public interface ILocalizedResourceProvider
    {
        string GetText(string resourceKey, params object[] objects);
    }


    public class LocalizedResourceProvider : ILocalizedResourceProvider
    {
        private readonly IResourceManagerProvider resourceManager;

        public LocalizedResourceProvider(IResourceManagerProvider resourceManager)
        {
            this.resourceManager = resourceManager;
        }
        public string GetText(string resourceKey, params object[] objects)
        {
            return LocalizedResourceHelper.GetText(resourceManager.ResourceManager,resourceKey, objects);
        }
    }

    public class LocalizedResourceHelper
    {
        public static string GetText(ResourceManager ResourceManager, string resourceKey, params object[] objects)
        {
            if (string.IsNullOrWhiteSpace(resourceKey))
            {
                return string.Empty;
            } 
             
            var translation = ResourceManager.GetString(resourceKey, CultureInfo.InstalledUICulture);

            if (translation == null)
            {
                translation = resourceKey;
            }

            if (objects == null || objects.Length == 0)
            {
                return translation;
            }

            try
            {
                return string.Format(translation, objects);
            }
            catch //(Exception e)//Logged-ex
            {
                return translation;
            }
        }

        public static string GetText(string text,params object[] objects)
        {
            var container = (Application.Current as PrismApplicationBase).Container;
            var resource = (IResourceManagerProvider)container.Resolve(typeof(IResourceManagerProvider));
            return GetText(resource.ResourceManager, text, objects);
        }
    }
}
