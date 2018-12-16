#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using Newtonsoft.Json;

#endregion

namespace McDotNet
{
    public class SettingsModel
    {
        public List<UserCredentials> Logins { get; set; }
        public bool IsOfflineMode { get; set; }
        public List<UserProfile> Profiles { get; set; }
        public UserProfile PreferredProfile { get; set; }
    }
}