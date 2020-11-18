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
            StackLayout stack = new StackLayout();
            Label offerLabel = new Label
            {
                Text = json,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 15,
                Padding = 10
            };
            offerLabel.TextColor = Color.FromRgb(215, 145, 0);
            stack.Children.Add(offerLabel);
            Content = new ScrollView { Content = stack };          
        }

        //private async void back(object sender, EventArgs e)
        //{
        //    await Navigation.PopAsync();
        //    Navigation.RemovePage(this);
        //}
    }
}