using TMPro;
using UnityEngine;

namespace Scenes.Gameboard.Scripts
{
    public class DiceText : MonoBehaviour
    {
        private TMP_Text diceText;
        public GameController gameController;
    
        // Start is called before the first frame update
        void Start()
        {
            diceText = this.gameObject.GetComponentInChildren<TMP_Text>();
            //Dice();
            Debug.Log("DiceText");
        
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

    
    
        public void Dice()
        {
            int diceRoll = Random.Range(1, 6);
            diceText.text = diceRoll.ToString(); 
            Debug.Log(diceRoll);
            this.gameController.SetDiceNumber(diceRoll);
        }
    }
}
