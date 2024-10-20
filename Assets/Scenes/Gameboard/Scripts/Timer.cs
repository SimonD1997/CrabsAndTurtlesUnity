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

        public void InitTimer(TextMeshProUGUI text)
        {
            this.text = text; 
            audioSource = text.gameObject.GetComponent<AudioSource>();
        }

       public void StartTimer(bool sound)
        {
            if (sound) PlaySound();
            
            timerIsRunning = true;
            
        }

        public void StopTimer()
        {
            timerIsRunning = false;
            timeRemaining = 0;
            StopSound();
        }

        public void SetTimerText(string timerText)
        {
            _timeText = timerText;
        }

        private void PlaySound()
        {
            audioSource.volume = 1;
            audioSource.Play();
        }

        private void StopSound()
        {
            audioSource.Stop();
        }

        private void FadeSound(float volume)
        {
            audioSource.volume += volume;
        }
    }
}
