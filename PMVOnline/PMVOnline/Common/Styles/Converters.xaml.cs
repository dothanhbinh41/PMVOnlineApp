﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PMVOnline.Common.Styles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Converters : ResourceDictionary
    {
        public Converters()
        {
            InitializeComponent();
        }
    }
}