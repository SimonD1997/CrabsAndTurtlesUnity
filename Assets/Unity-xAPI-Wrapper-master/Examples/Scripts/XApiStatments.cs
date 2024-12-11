using System.Collections.Generic;
using UnityEngine;
using XAPI;

namespace Unity_xAPI_Wrapper_master.Examples.Scripts
{
    public class XApiStatments : MonoBehaviour
    {
        private static Actor _actor;
        private static Actor _actor2;
        private Activity _activity;
        
        // Start is called before the first frame update
        private void Start()
        {
           _actor = Actor.FromMailbox("mailto:example@game.com",false , "Gruppe 1");
            _actor2 = Actor.FromMailbox("mailto:example2@game.com",false ,"Gruppe 2");
            _activity = new Activity("http://game.doubleday.de", "Crabs and Turtles");
            
            var configObject = XAPIMessenger.ConfigObject;
            configObject.config.Endpoint = PlayerPrefs.GetString("XapiServer", "https://lrs.adlnet.gov/xapi/");
            configObject.config.Username = PlayerPrefs.GetString("XapiKey", "xapi-tools");
            configObject.config.Password = PlayerPrefs.GetString("XapiSecret", "xapi-tools");


            //SendSimpleStatement();
        }

        public void SendSimpleStatement(Verb verb)
        {
            var _actor = Actor.FromMailbox("mailto:example@game.com",false , "Gruppe 1");

            var statement = new Statement(_actor, verb, _activity);

            XAPIWrapper.SendStatement(statement, res => {
                Debug.Log("Sent simple statement!  LRS stored with ID: " + res.StatementID); 
            });
            
        }
        
        /// <summary>
        /// Send a simple xAPI statement when the button is clicked
        /// </summary>
        public void ButtonClicked()
        {
            SendSimpleStatement(Verbs.Interacted);
        }
        
        /// <summary>
        ///  Send a simple xAPI statement when the Application is closed
        /// </summary>
        void OnApplicationQuit() {
            SendSimpleStatement(Verbs.Terminated);
        }



        #region Example methods
        //Example methods
        
        public void SendSimpleStatement()
        {
            var actor = Actor.FromAccount("https://auth.example.com", "some-long-user-id", name: "some-user-name");
            var verb = Verbs.Interacted;
            var activity = new Activity("https://lms.example.com", "Unity Button Example");

            var statement = new Statement(actor, verb, activity);

            XAPIWrapper.SendStatement(statement, res => {
                Debug.Log("Sent simple statement!  LRS stored with ID: " + res.StatementID); 
            });
        }
        
        public void SendMultipleStatements()
        {
            var actor = XAPI.Actor.FromAccount(new ActorAccount("https://auth.example.com", "some-long-user-id"), name: "some-user-name");
            var verb = XAPI.Verbs.Interacted;
            var activity = new XAPI.Activity("https://lms.example.com", "Unity Button Example");

            var statement = new Statement(actor, verb, activity);
            var statements = new Statement[] { statement, statement };

            XAPIWrapper.SendStatements(statements, res => {
                Debug.Log("Sent multiple statements!  LRS stored with IDs: " + res.StatementIDs[0] + " : " + res.StatementIDs[1]); 
            });
        }
        
        public void SendComplexStatement()
        {
            var actor = XAPI.Actor.FromAccount(new ActorAccount("https://auth.example.com", "some-long-user-id"), name: "some-user-name");
            var verb = XAPI.Verbs.Interacted;
            var activity = new XAPI.Activity("https://lms.example.com", "Unity Button Example");
            var result = new XAPI.Result(true, true);

            result.Score = new XAPI.Score(163, 0, 200);
            result.Duration = new System.TimeSpan(5, 20, 12);

            var statement = new Statement(actor, verb, activity);

            statement.Result = result;
            statement.Context = new Context();

            statement.Context.Instructor = Actor.FromMailbox("mailto:someone@example.com");

            statement.Context.Extensions = new Dictionary<string, string>();
            statement.Context.Extensions.Add("https://schema.org/FakeExtension", "fake-value");

            XAPIWrapper.SendStatement(statement, res => {
                Debug.Log("Sent complex statement!  LRS stored with ID: " + res.StatementID); 
            });
        }
        #endregion
    }
}
