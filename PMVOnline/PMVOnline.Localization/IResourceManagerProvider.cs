using System;
using System.Resources;

namespace PMVOnline.Localization
{
    public interface IResourceManagerProvider
    { 
        ResourceManager ResourceManager { get; } 
    }

    //public class ResourceManagerProvider : IResourceManagerProvider
    //{
    //    public ResourceManager ResourceManager { get => throw new NotImplementedException(); }
    //}
}
