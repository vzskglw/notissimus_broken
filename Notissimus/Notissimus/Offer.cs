using System;
using System.Collections.Generic;
using System.Text;

namespace Notissimus
{
    class Offer
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
        public string getJsonString()
        {
            json = "offer = {\"attributes\" : {\"id\" : \"" + 
                id  + "\" , \"type\" : \"" + type + "\" , \"bid\" : \"" + bid + "\" }";
            foreach (KeyValuePair<string, string> node in childNodes)
            {
                json += " , \"" + node.Key + "\" : \"" + node.Value + "\"";
            }
            json += "}";
            return json;
        }
        public string getJSON()
        {
            json = "offer = \n\t{";
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
