using System;
using System.ComponentModel;
using System.Resources;
using Prism.Ioc;
using Prism.Modularity;

namespace PMVOnline.Localization
{
    public class PMVOnlineLocalizationModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        { 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILocalizedResourceProvider, LocalizedResourceProvider>(); 
        } 
    }
}
