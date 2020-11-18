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
            InitializeComponent();
            Title = "offers";
            getXML();

            List<string> idList = new List<string>();
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
                idList.Add(offer.id);
            }
           ListView listView = (ListView)FindByName("listView");
           listView.ItemsSource = idList;
           listView.ItemTapped += OnItemTapped;
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
            string json = "", id = e.Item.ToString();
            foreach (Offer offer in Offers)
            {
                if (offer.id == id) { json = offer.getJSON(); break; }
            }
            await Navigation.PushAsync(new OfferPage(json));
        }
    }
}