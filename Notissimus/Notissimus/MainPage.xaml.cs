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
        public static List<Offer> Offers = new List<Offer>();
        public static List<string> IDs = new List<string>();
        public static XmlDocument document = new XmlDocument();
        public MainPage()
        {
            InitializeComponent();
            Title = "offers";
            getOfferList();
            XmlNodeList offers = document.GetElementsByTagName("offer");
            foreach (XmlNode offer in offers)
            {
                Offer o = new Offer();
                XmlAttributeCollection ac = offer.Attributes;
                o.id = ac.GetNamedItem("id").Value.ToString();
                o.type = ac.GetNamedItem("type").Value.ToString();
                o.bid = ac.GetNamedItem("bid").Value.ToString();
                o.available = ac.GetNamedItem("available").Value.ToString();
                XmlNodeList childNodes = offer.ChildNodes;
                List<KeyValuePair<string, string>> childrenList = new List<KeyValuePair<string, string>>();
                // Console.WriteLine("OFFER " + o.id);
                foreach (XmlNode node in childNodes)
                {
                    childrenList.Add(new KeyValuePair<string, string>(node.Name, node.InnerText));
                    Debug.WriteLine("\n\n\n\n\n\n");
                    Debug.WriteLine("<" + node.Name + "> " + node.InnerText + " </" + node.Name + ">");
                }
                o.childNodes = childrenList;
                Offers.Add(o);
            }
            foreach (Offer of in Offers)
            {
                 IDs.Add(of.id);
            }

           ListView listView = (ListView)FindByName("listView");
           listView.ItemsSource = IDs;
           listView.ItemTapped += OnItemTapped;
        }

        private static void getOfferList()
        {          
            Task.Run(async () =>
            {
                HttpClient cl = new HttpClient();
                using (Stream stream = await cl.GetStreamAsync("https://yastatic.net/market-export/_/partner/help/YML.xml"))
                {
                    using (XmlTextReader reader = new XmlTextReader(stream))
                    {
                        document.Load(reader);
                    }
                }
            }).Wait();
        }

        private static async Task<XmlDocument> getREsponseAsync(string url)
        {
            HttpClient cl = new HttpClient();
            XmlDocument document = new XmlDocument();
            using (Stream stream = await cl.GetStreamAsync(url))
            {
                using (XmlTextReader reader = new XmlTextReader(stream))
                {
                    document.Load(reader);
                }
            }
            return document;
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


        public class Offer
        {
            public string id, type, bid, cbid, available;
            public string json;
            public List<KeyValuePair<string, string>> childNodes;
            public Offer()
            { }
            Offer(string id, string type, string bid, string cbid, string available)
            {
                this.id = id;
                this.type = type;
                this.bid = bid;
                this.cbid = cbid;
                this.available = available;
            }
            public string getJSON()
            {
                json += "offer = \n\t{";
                json += "\t\"attributes\" : \n";
                json += "\t\t\t\t{\"id\" : \"" + id + "\",\n";
                json += "\t\t\t\t\"type\" : \"" + type + "\",\n";
                json += "\t\t\t\t\"bid\" : \"" + bid + "\" }";
                foreach (KeyValuePair<string, string> node in childNodes)
                {
                    json += ",\n";
                    json += "\t\t\"" + node.Key + "\" : \"" + node.Value + "\"";
                }

                json += "\n\t}";
                return json;
            }
        }
    }
}