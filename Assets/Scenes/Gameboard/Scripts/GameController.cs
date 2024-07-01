using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scenes.Gameboard.Scripts
{
    public class GameController : MonoBehaviour
    {
    
        //Todo:
        //Rundenbasierte Spielmechanik einführen 

        private bool gameIsRunning = false;
        private int playerTurn = 0;
        public TextMeshProUGUI yourTurnField ;
        public MovementScript movementScript;
        private int _diceNumber;
        public GameObject cardPrefab;
        public Sprite[] cardPrefabStack;
        private GameObject[] _cardStack;
        private List<GameObject> _cardList;
    
    
        // Start is called before the first frame update
        void Start()
        {
           // Debug.Log(this._cardStack.Length.ToString());
            this.gameIsRunning = true;
        }

        private void Awake()
        {
            CardStack();
        }

        // Update is called once per frame
        void Update()
        {
            if (gameIsRunning == true)
            {
                if (playerTurn == 0)
                {
                    this.yourTurnField.gameObject.SetActive(true);
                    this.yourTurnField.text = "DU BIST DRAN!";
                }
                else
                {
                    TurnOfOtherPlayers();
                }
            }
            else
            {
                //todo:
                //run endscreen with scoreboard
            }
        }


        void TurnOfOtherPlayers()
        {
            //todo:
            // warten falls andere Spieler analog anwesend sind oder
            // einen virtuellen Zug machen falls gegen den Computer gekämpft wird
        
        }

        public void SetDiceNumber(int diceNumber)
        {
            this._diceNumber = diceNumber;
            this.movementScript.Movement(diceNumber);
        }

        public void CardStack()
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject tempCard = Instantiate(cardPrefab, new Vector3(-400, 0, i*5), Quaternion.identity);
                tempCard.GetComponentInChildren<SpriteRenderer>().sprite = cardPrefabStack[Random.Range(0,cardPrefabStack.Length)];
                //this._cardStack[i] = tempCard;
                _cardList.Add(tempCard);
                
            }
            
             
        }

    }
}
