using PMVOnline.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Guides.Services
{
    public interface IGuideService
    {
        Task<string> GetGuideAsync();
    }

    public class GuideService : ApiProvider<AppApi>, IGuideService
    { 
        public async Task<string> GetGuideAsync()
        {
            var result = await Api.GetGuide();
            return result.Content?.Content;
        }
    }
}
