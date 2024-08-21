using UnityEngine;
using TMPro;

namespace Scenes.Gameboard.Scripts
{
    public class Timer : ScriptableObject
    {
        public TextMeshProUGUI text;
        public float timeRemaining = 30;
        public bool timerIsRunning = false;
        // Start is called before the first frame update
        void Start()
        {
            
            timerIsRunning = true;
        }

        // Update is called once per frame
        void Update()
        {
            
            if (timerIsRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }
                else
                {
                    timeRemaining = 0;
                    timerIsRunning = false;
                }
            }
        }
    }
}
