using Newtonsoft.Json;
using PMVOnline.Accounts.Models;
using PMVOnline.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace PMVOnline.Common.Services
{
    public interface IApplicationSettings
    {
        UserModel User { get; set; }
        bool UseNotification { get; set; }
    }


    public class ApplicationSettings : IApplicationSettings
    {
        public UserModel User { get => Preferences.Get(nameof(User), string.Empty).DeserializeObject<UserModel>(); set => Preferences.Set(nameof(User), value.SerializeObject()); }
        public bool UseNotification { get => Preferences.Get(nameof(UseNotification), true); set => Preferences.Set(nameof(UseNotification), value); }
    } 
}
