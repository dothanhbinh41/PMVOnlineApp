using System; 
using PMVOnline.Api;
using PMVOnline.Api.Authorization;
using Prism.Ioc;
using Prism.Modularity;

namespace PMVOnline.Api
{
    public class PMVOnlineApiModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        { 
            containerRegistry.Register<IAuthHeaderManager, AuthHeaderManager>();
        }
    }
}
