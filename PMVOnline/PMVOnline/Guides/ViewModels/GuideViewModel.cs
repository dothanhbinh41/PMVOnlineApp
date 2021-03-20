using PMVOnline.Common.Bases;
using PMVOnline.Guides.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Guides.ViewModels
{
    public class GuideViewModel : TabViewModelBase
    {
        public string Content { get; set; }
        readonly IGuideService guideService;

        public GuideViewModel(IGuideService guideService)
        {
            this.guideService = guideService;
        }

        public override void RaiseIsActiveChanged()
        {
            base.RaiseIsActiveChanged();
            if (string.IsNullOrEmpty(Content))
            {
                LoadData();
            }
        }

        async Task LoadData()
        {
            IsBusy = true;
            var text = await guideService.GetGuideAsync();
            Content = text;
            IsBusy = false;
        }
    }
}
