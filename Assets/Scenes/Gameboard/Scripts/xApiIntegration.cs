using System;
using LRS.Domain;
using UnityEngine;

namespace Scenes.Gameboard.Scripts
{
    public class XApiIntegration : MonoBehaviour
    {
        public XApiIntegration() {

        }
        // LRS Credentials
        public string lrsUrl;
        public string lrsKey;
        public string lrsSecret;
        


        // Start is called before the first frame update
        void Start() {
            
            // set Account and homepage for a more anonymous and contained identity:
            //PlayerPrefs.SetString("LRSAccountId","123456789");
            //PlayerPrefs.SetString("LRSHomepage","https://www.yetanalytics.com");

            // Or if you prefer, Email can be set the following way:
            PlayerPrefs.SetString("LRSEmail","user@example.com");

            PlayerPrefs.SetString("LRSUsernameDisplay","John Doe");
            
            // Game Identity Data
            // This sets the Platform of the statement under context.platform
            // by default, the object.id (aka the Activity) will also use this as it's identifier:
            PlayerPrefs.SetString("LRSGameId", "http://video.games/button-clicker");
            PlayerPrefs.SetString("LRSGameDisplay", "Button Clicker");

            // In addition to LRSGameId, Set the following if you'd like to override the ActivityId via playerPrefs.
            // Note that both LRSActivityId and LRSActivityDefinition need to be set in order for this to work:
            PlayerPrefs.SetString("LRSActivityId", "http://video.games/button-clicker/level/1");
            PlayerPrefs.SetString("LRSActivityDefinition", "Level 1 of button-clicker");
            
            // Session Identity Data
            PlayerPrefs.SetString("LRSSessionIdentifier",Guid.NewGuid().ToString());
            
            
            // Location Data
            // Add this to the PlayerPrefs if you wish to get user location data
            //PlayerPrefs.SetString("LRSEnableUserLocation", "true");

            PlayerPrefs.Save();
            
            
        }

        
    }
}
