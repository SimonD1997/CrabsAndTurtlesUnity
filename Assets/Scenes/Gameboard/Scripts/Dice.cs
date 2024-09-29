using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Scenes.Gameboard.Scripts
{
    public class Diece : MonoBehaviour
    {
        
        private Rigidbody rb;

        private int _diceNumber;

        private GameController _gameController;
        public Button _button;


        void Awake()
        {
        }

        // Start is called before the first frame update
        void Start()
        {
            _gameController = FindFirstObjectByType<GameController>();
            
            rb = this.gameObject.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_gameController.GetGameState() == 0|| _gameController.debugMode)
            {
                _button.interactable = true;
            }else
            {
                _button.interactable = false;
            }

        }

        

        public void RollTheDice()
        {
            Debug.Log("Roll The Dice");
            
            _diceNumber = 0;

            rb.mass = 1;
            rb.AddForce(Vector3.back * (Random.Range(100, 150) *25* rb.mass));
            rb.AddForce(Vector3.right * (Random.Range(10, 15)));
            rb.AddTorque(Random.Range(0, 99), Random.Range(0, 99), Random.Range(0, 99), ForceMode.Impulse);

            StartCoroutine(WaitforDiceDown());
        }

        IEnumerator WaitforDiceDown()
        {
            yield return new WaitForSeconds(0.6f);
            rb.AddForce(Vector3.back * (Random.Range(100, 150) * rb.mass));
            rb.AddTorque(Random.Range(0, 99), Random.Range(0, 99), Random.Range(0, 99), ForceMode.Impulse);

            yield return new WaitForSeconds(1f);

            Debug.Log(" Dice is Down");
            rb.velocity = Vector3.zero;
            //rb.MovePosition(new Vector3(0,0,0));
            rb.mass = 5000;
            rb.AddForce(Vector3.forward * 2000, ForceMode.Impulse);

            StartCoroutine(GetDiceCount());
        }

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
                 // Error case
                 
                 // If the dice is not in the correct position, then push the dice down
                 if (_diceNumber == 0)
                 {
                     rb.mass = 1;
                     yield return new WaitForSeconds(0.2f);
                     rb.mass = 5000;
                     rb.AddForce(Vector3.forward * 2000, ForceMode.Impulse);
                     rb.AddTorque(Vector3.right*5, ForceMode.Impulse);
                 }
                yield return _diceNumber;
                
            }

            Debug.Log(_diceNumber);
            this._gameController.SetDiceNumber(_diceNumber);
        }
    }
}
            
  