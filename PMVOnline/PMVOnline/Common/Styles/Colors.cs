using Xamarin.Forms;

namespace PMVOnline.Common.Styles
{
    public partial class Colors: ResourceDictionary
    {
        public Colors()
        {
            Add("Primary", Color.FromHex("#152F70"));
            Add("EndPrimary", Color.FromHex("#121E3C"));
            Add("Background", Color.FromHex("#F8F8F8"));
            Add("Text", Color.Black);
            Add("Description", Color.FromHex("#706F6F"));
            Add("Normal", Color.FromHex("#0e8a54"));
            Add("High", Color.FromHex("#a69d28"));
            Add("Highest", Color.FromHex("#a30707"));
            Add("Foreground", Color.White);
        }
    } 
}