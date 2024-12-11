using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Scenes.Gameboard.Scripts
{
    
    /// <summary>
    /// Not used in the final version of the game
    /// </summary>
    public class DiceText : MonoBehaviour
    {
        private TMP_Text diceText;
        private GameController _gameController;
        public Button button;
    
        // Start is called before the first frame update
        void Start()
        {
            _gameController = FindObjectOfType<GameController>();
            diceText = this.gameObject.GetComponentInChildren<TMP_Text>();
            //Dice();
            Debug.Log("DiceText");
        
        
        }

        // Update is called once per frame
        void Update()
        {
            if (_gameController.GetGameState() == 0|| _gameController.debugMode)
            {
                button.interactable = true;
            }else
            {
                button.interactable = false;
            }
        }

    
    
        public void Dice()
        {
            
            
            int diceRoll = Random.Range(1, 7);
            diceText.text = diceRoll.ToString(); 
            this._gameController.SetDiceNumber(diceRoll);
        }
    }
}
