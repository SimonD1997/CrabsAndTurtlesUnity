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

        public AudioSource audioSource;
        public AudioClip soundClip;

        


        // Update is called once per frame
        /// <summary>
        ///  Update Timer and Sound for Timer
        /// Sound is fading down when Timer is under 29 and fading up when Timer is under 5
        /// </summary>
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
                    StopTimer();
                } 
                //Debug.Log("Timer:" + timeRemaining);
                this.text.text = _timeText + Convert.ToInt32(timeRemaining).ToString();
            }
            

            if (timeRemaining < 5)
            {
                // Fade Sound UP
                FadeSound(0.01f);
            }
            
            if (timeRemaining < 29 && timeRemaining > 5)
            {
                // Fade Sound Down
                FadeSound(-0.005f);
                
            }
           
        }

        /// <summary>
        ///  Init Timer with TextMeshProUGUI
        /// </summary>
        /// <param name="text"> TextMeshProUGUI</param>
        public void InitTimer(TextMeshProUGUI text)
        {
            this.text = text; 
            audioSource = text.gameObject.GetComponent<AudioSource>();
        }

        
        /// <summary>
        ///  Start Timer with Sound or without Sound
        /// </summary>
        /// <param name="sound"> true = with Sound, false = without Sound</param>
       public void StartTimer(bool sound)
        {
            if (sound) PlaySound();
            
            timerIsRunning = true;
            
        }

        /// <summary>
        ///  Stop Timer
        /// </summary>
        public void StopTimer()
        {
            timerIsRunning = false;
            timeRemaining = 0;
            StopSound();
        }

        /// <summary>
        ///  Set Timer Text
        /// </summary>
        /// <param name="timerText"> Text for Timer</param>
        public void SetTimerText(string timerText)
        {
            _timeText = timerText;
        }

        /// <summary>
        ///  Play Sound for Timer
        /// </summary>
        private void PlaySound()
        {
            audioSource.volume = 1;
            audioSource.Play();
        }

        /// <summary>
        ///  Stop Sound for Timer
        /// </summary>
        private void StopSound()
        {
            audioSource.Stop();
        }

        /// <summary>
        ///  Fade Sound for Timer with given value
        /// </summary>
        /// <param name="volume"> Value for Fade</param>
        private void FadeSound(float volume)
        {
            audioSource.volume += volume;
        }
    }
}
