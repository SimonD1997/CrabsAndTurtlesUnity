using System;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;


namespace LRS {
    public class Sender {
        
        // state
        public bool complete { get; set; }
        public bool success { get; set; }
        public string response { get; set; }
        
        public Sender(String LRSUrl,
                      String LRSKey,
                      String LRSSecret) 
        {
            this.LRSUrl = LRSUrl;
            this.LRSKey = LRSKey;
            this.LRSSecret = LRSSecret;
        }

        private String LRSUrl {set;get;}
        private String LRSKey {set;get;}
        private String LRSSecret {set;get;}

        public async Task<RestResponse> SendStatement(String statement) {
            var client = new RestClient(LRSUrl) {
                Authenticator = new HttpBasicAuthenticator(LRSKey,LRSSecret)
            };
            var request = new RestRequest("/xapi/statements", Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddStringBody(statement,DataFormat.Json);
            request.AddHeader("X-Experience-API-Version", "1.0.1");
            Debug.Log("SEnd STatement Task");
            return await client.ExecutePostAsync(request);
        }
        public void ClearState()
        {
            complete = false;
            success = false;
            response = "";
        }

        public void PostStatement(String statement)
        {
            //reinit state
            ClearState();

            // https://learninglocker.dig-itgames.com/data/xAPI/statements?statementId=58098b7c-3353-4f9c-b812-1bddb08876fd
            //string queryURL = endpoint + "statements";

            //byte[] formBytes = Encoding.UTF8.GetBytes(statement.ToJSON(version));

            String auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(LRSKey + ":" + LRSSecret));

            var request = UnityWebRequest.Put(LRSUrl, statement);

            // Configure the request
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Authorization", auth);
            request.SetRequestHeader("X-Experience-API-Version", "1.0.0");
            request.SetRequestHeader("Content-Type", "application/json");

            var requestOperation = request.SendWebRequest();
            requestOperation.completed += (operation) =>
            {
                success = !(request.isNetworkError || request.isHttpError);

                if (success)
                {
                    JArray ids = JArray.Parse(request.downloadHandler.text);
                    response = ids[0].ToString();
                }
                else
                {
                    response = request.error;
                }

                complete = true;
            };
        }
    }
}