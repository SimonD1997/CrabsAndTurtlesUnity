using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Scenes.Gameboard.Scripts
{
    public class Dice : MonoBehaviour
    {
        
        private Rigidbody rb;

        private int _diceNumber;
        private bool _diceInMotion;
        private int _diceRollCount;

        private GameController _gameController;
        public Button _button;
        

        // Start is called before the first frame update
        void Start()
        {
            _gameController = FindFirstObjectByType<GameController>();
            
            rb = this.gameObject.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            
            // Check if the dice is already in motion and if the game state is 0, then enable the button to roll the dice
            if ((_gameController.GetGameState() == 0|| _gameController.debugMode )&& _diceInMotion == false)
            {
                _button.interactable = true;
            }else
            {
                _button.interactable = false;
            }

        }

        /// <summary>
        /// Roll the dice and set the necessary parameters for the dice to roll and error handling
        /// </summary>
        public void RollTheDice()
        {
            Debug.Log("Roll The Dice");
            
            _diceNumber = 0;
            _diceRollCount = 0;
            _diceInMotion = true;

            rb.mass = 1;
            rb.AddForce(Vector3.back * (Random.Range(100, 150) *25* rb.mass));
            rb.AddForce(Vector3.right * (Random.Range(10, 15)));
            rb.AddTorque(Random.Range(0, 99), Random.Range(0, 99), Random.Range(0, 99), ForceMode.Impulse);

            StartCoroutine(WaitforDiceDown());
        }

        /// <summary>
        ///  Wait for an amount of time and then push the dice down to stop it from moving
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitforDiceDown()
        {
            yield return new WaitForSeconds(0.6f);
            rb.AddForce(Vector3.back * (Random.Range(100, 150) * rb.mass));
            rb.AddTorque(Random.Range(0, 99), Random.Range(0, 99), Random.Range(0, 99), ForceMode.Impulse);

            yield return new WaitForSeconds(1f);
            
            rb.velocity = Vector3.zero;
            rb.mass = 5000;
            rb.AddForce(Vector3.forward * 2000, ForceMode.Impulse);

            StartCoroutine(GetDiceCount());
        }

        /// <summary>
        ///  Wait for the dice to stop moving and then identify the face value of the dice
        /// </summary>
        /// <returns></returns>
        private IEnumerator GetDiceCount()
        {
            yield return new WaitForSeconds(1f);
            
            // Logic to identify the face value of the dice

            Vector3 up = Vector3.back;

            while (_diceNumber == 0)
            {
                if (Vector3.Dot(up, transform.forward) > 0.9f) _diceNumber = 1;
                if (Vector3.Dot(up, -transform.forward) > 0.9f) _diceNumber = 6;
                if (Vector3.Dot(up, transform.up) > 0.9f) _diceNumber = 2;
                if (Vector3.Dot(up, -transform.up) > 0.9f) _diceNumber = 5;
                if (Vector3.Dot(up, -transform.right) > 0.9f) _diceNumber = 3;
                if (Vector3.Dot(up, transform.right) > 0.9f) _diceNumber = 4;
                 
                 // If the dice is not in the correct position, then push the dice down
                 if (_diceNumber == 0)
                 {
                     _diceRollCount++;
                     rb.mass = 1;
                     yield return new WaitForSeconds(0.5f);
                     rb.mass = 5000;
                     rb.AddForce(Vector3.forward * 2000, ForceMode.Impulse);
                     rb.AddTorque(Vector3.right*10, ForceMode.Impulse);
                 }

                 //if the dice is not in the correct position after 3 attempts, then roll the dice again
                 if (_diceRollCount > 3)
                 {
                     RollTheDice();
                     StopCoroutine(this.GetDiceCount());
                 }
                 
                yield return _diceNumber;
                
            }

            Debug.Log(_diceNumber);
            this._gameController.SetDiceNumber(_diceNumber);
            _diceInMotion = false;
            
        }
    }
}
            
  