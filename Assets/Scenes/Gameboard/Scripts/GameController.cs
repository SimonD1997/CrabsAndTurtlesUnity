using System;
using System.Collections.Generic;
using System.Security.Policy;
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
        public GameObject riddleCardPrefab;
        public Sprite[] cardPrefabStack;
        public Sprite[] riddlePrefabStack;
        
        //private GameObject[] _cardStack;
        private List<GameObject> _cardList;
        private List<GameObject> _riddleCardList;
    
    
        // Start is called before the first frame update
        void Start()
        {
           // Debug.Log(this._cardStack.Length.ToString());
            this.gameIsRunning = true;
        }

        private void Awake()
        {
            _cardList = new List<GameObject>();
            _riddleCardList = new List<GameObject>();
            //_cardStack = new GameObject[5];
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
            
            //Karten vom Stapel nehmen, wenn die Karte umgedreht ist.

            GameObject tempCard = this._cardList[0].gameObject;
            GameObject tempCard2 = this._riddleCardList[0].gameObject;
            if (tempCard.GetComponent<CardFlipClick>().GetTurnState() == false)
            {
                tempCard.GetComponent<CardFlipClick>().ClickStateActivate();
                
                // TODO Wait until animation is complete 
                this._cardList.RemoveAt(0);
                Destroy(tempCard);
                
                //Karten alle um eins nach oben verschieben und neue Karte unten anfügen
                
                MoveCardsUp(_cardList);
                AddCardStack(_cardList,cardPrefab,cardPrefabStack);
            }

            if (tempCard2.GetComponent<CardFlipClick>().GetTurnState() == false)
            {
                tempCard2.GetComponent<CardFlipClick>().ClickStateActivate();
                // TODO Wait until animation is complete 
                
                this._riddleCardList.RemoveAt(0);
                Destroy(tempCard2);
                
                MoveCardsUp(_riddleCardList);
                AddCardStack(_riddleCardList,riddleCardPrefab,riddlePrefabStack);
            }
            
            
        }

        private void CardStack()
        {
            
            for (int i = 0; i < 5; i++)
            {
                GameObject tempCard = Instantiate(riddleCardPrefab, new Vector3(-400, -100, i*5), Quaternion.identity);
                tempCard.GetComponentInChildren<SpriteRenderer>().sprite = riddlePrefabStack[Random.Range(0,riddlePrefabStack.Length)];
                //this._cardStack[i] = tempCard;
                //Karte dort runternehmen und dann den Stapel von unten auffüllen
                _riddleCardList.Add(tempCard);
            }
            for (int i = 0; i < 5; i++)
            {
                GameObject tempCard = Instantiate(cardPrefab, new Vector3(-400, 0, i*5), Quaternion.identity);
                tempCard.GetComponentInChildren<SpriteRenderer>().sprite = cardPrefabStack[Random.Range(0,cardPrefabStack.Length)];
                _cardList.Add(tempCard);
            }
            
            //GameObject tempCard2 = this._cardList[4];
            //tempCard2.GetComponent<CardFlipClick>().enabled = true;
            _riddleCardList[0].gameObject.GetComponent<CardFlipClick>().enabled = true;
            _cardList[0].gameObject.GetComponent<CardFlipClick>().enabled = true;
        }

        private void MoveCardsUp(List<GameObject> cardList)
        {
            foreach (var o in cardList)
            {
                Vector3 tempVector = o.transform.position;
                tempVector.z = o.transform.position.z - 5;
                o.transform.position = tempVector;
            }
            cardList[0].gameObject.GetComponent<CardFlipClick>().enabled = true;
        }

        private void AddCardStack(List<GameObject> cardList, GameObject prefab, Sprite[] spriteStack)
        {
            GameObject tempCard = Instantiate(prefab, new Vector3(-400, 0, 20), Quaternion.identity);
            tempCard.GetComponentInChildren<SpriteRenderer>().sprite = spriteStack[Random.Range(0,spriteStack.Length)];
            cardList.Add(tempCard);
        }

    }
}
