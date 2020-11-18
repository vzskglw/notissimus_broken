using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Notissimus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OfferPage : ContentPage
    {
        public OfferPage(string json)
        {
            InitializeComponent();
            Title = "selected offer JSON";
            offerLabel.Text = json;
            offerLabel.TextColor = Color.FromRgb(215, 145, 0);
        }
    }
}