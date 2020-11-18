using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;

namespace Notissimus
{
    public partial class MainPage : ContentPage
    {
        static List<Offer> Offers = new List<Offer>();
        public static XmlDocument document = new XmlDocument();
        public MainPage()
        {
            Title = "offers";
            getXML();

            XmlNodeList offerNodes = document.GetElementsByTagName("offer");
            foreach (XmlNode offerNode in offerNodes)
            {
                Offer offer = new Offer();
                XmlAttributeCollection offerAttributes = offerNode.Attributes;
                offer.id = offerAttributes.GetNamedItem("id").Value.ToString();
                offer.type = offerAttributes.GetNamedItem("type").Value.ToString();
                offer.bid = offerAttributes.GetNamedItem("bid").Value.ToString();
                offer.available = offerAttributes.GetNamedItem("available").Value.ToString();
                XmlNodeList childNodes = offerNode.ChildNodes;
                List<KeyValuePair<string, string>> childrenList = new List<KeyValuePair<string, string>>();
                foreach (XmlNode node in childNodes)
                {
                    childrenList.Add(new KeyValuePair<string, string>(node.Name, node.InnerText));
                }
                offer.childNodes = childrenList;
                Offers.Add(offer); 
            }
            ListView listView = new ListView
            {
                HasUnevenRows = true,
                ItemsSource = Offers,

                ItemTemplate = new DataTemplate(() =>
                {
                    Label idLabel = new Label
                    {
                        Padding = new Thickness(10, 15),
                        FontSize = 23, TextColor = Color.FromRgb(233, 133, 33) };
                    idLabel.SetBinding(Label.TextProperty, "id");

                    Label typeLabel = new Label
                    {
                        Padding = new Thickness(15, 18),
                        FontSize = 16, TextColor = Color.FromRgb(250, 180, 50) };
                    typeLabel.SetBinding(Label.TextProperty, "type");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = { idLabel, typeLabel }
                        }
                    };
                })
            };
            listView.ItemTapped += OnItemTapped;

            StackLayout stack = new StackLayout();            
            stack.Children.Add(listView);
            Content = new ScrollView { Content = stack };
            InitializeComponent();
        }


        private static void getXML()
        {          
            Task.Run(async () =>
            {
                HttpClient client = new HttpClient();
                using (Stream stream = await client.GetStreamAsync("https://yastatic.net/market-export/_/partner/help/YML.xml"))
                {
                    using (XmlTextReader reader = new XmlTextReader(stream))
                    {
                        document.Load(reader);
                    }
                }
            }).Wait();
        }

        async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Offer selectedOffer = e.Item as Offer;
            string json = "", id = selectedOffer.id;
            foreach (Offer offer in Offers)
            {
                if (offer.id == id) { json = offer.getJSON(); break; }
            }
            await Navigation.PushAsync(new OfferPage(json));
        }
    }
}