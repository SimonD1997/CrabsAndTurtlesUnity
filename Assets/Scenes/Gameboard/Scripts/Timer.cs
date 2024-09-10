using System;
using UnityEngine;
using TMPro;

namespace Scenes.Gameboard.Scripts
{
    public class Timer : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public float timeRemaining = 30;
        private string _timeText;
        public bool timerIsRunning = false;
        
        
        
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
                //Debug.Log("Timer:" + timeRemaining);
                
            }
            this.text.text = _timeText + Convert.ToInt32(timeRemaining).ToString();
            
           
        }

       public void StartTimer()
        {
            timerIsRunning = true;
        }

        public void StopTimer()
        {
            timerIsRunning = false;
            timeRemaining = 0;
        }

        public void SetTimerText(string timerText)
        {
            _timeText = timerText;
        }
    }
}
