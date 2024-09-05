using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scenes.Gameboard.Scripts
{
    public class GameController : MonoBehaviour
    {
        private int _diceNumber;
        
        
        //Todo:
        //Rundenbasierte Spielmechanik einführen 
        
        private bool gameIsRunning = false;
        private int playerTurn = 0;
        private int nextMove;

        //if an actioncard change an variable the gamecontroller has to check the correct input bevor the next dice roll
        private int _newColourNumber;
        private int _corectionTimes = 1;
        
        
        public TextMeshProUGUI yourTurnField ;
        public MovementScript movementScript;
        public VariablenTafel variablenTafel;
        
        public GameObject cardPrefab;
        public GameObject riddleCardPrefab;
        public Sprite[] cardPrefabStack;
        public Sprite[] riddlePrefabStack;

        private Timer _timer;
        private int _correctAnswer;
        public bool inputFieldEnter = false;
        public TMP_InputField inputField;
        public TextMeshProUGUI timerField;
        
        //private GameObject[] _cardStack;
        private List<GameObject> _cardList;
        private List<GameObject> _riddleCardList;
    
    
        // Start is called before the first frame update
        void Start()
        {
           // Debug.Log(this._cardStack.Length.ToString());
            this.gameIsRunning = true;
            timerField.text = "0";
            this.inputField.textComponent.enableWordWrapping = true;
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


                if (inputFieldEnter == true && nextMove > 0 )
                {
                    if (Convert.ToInt32(inputField.text) != nextMove)
                    {
                        if (nextMove == 1)
                        {
                            inputField.text = "Dies war die falsche Angabe oder die der anderen Figur! Du darfst "+ nextMove + " Schritt vor gehen!";
                        }
                        else
                        {
                            inputField.text = "Dies war die falsche Angabe oder die der anderen Figur! Du darfst "+ nextMove + " Schritte vor gehen!";
                        }
                        
                    }
                    this.movementScript.Movement(nextMove);

                    nextMove = 0;
                    //inputField.text = "";
                    inputFieldEnter = false;
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

        public int GetPlayerTurn()
        {
            return this.playerTurn;
        }
        
        public void EndEditInputField()
        {
            inputFieldEnter = true;
        }

        public void SetNextMove(int setNextMove)
        {
            nextMove = setNextMove;
        }
        
        public int GetDiceNumber()
        {
            Debug.Log("DiceNumber:"+ this._diceNumber);
            return this._diceNumber;
        }
        
        public void SetDiceNumber(int diceNumber)
        {
            Debug.Log(_corectionTimes);
            
            ///checks correct variablentafel bevor let the player roll the next dice
            if (_newColourNumber == variablenTafel.GetVar(movementScript.GetPositionColour()) || _corectionTimes == 0)
            {
                timerField.text = "0";
                _corectionTimes = 1;
                variablenTafel.SetVar(movementScript.GetPositionColour(),_newColourNumber);
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
                    AddCardStack(_cardList,cardPrefab,cardPrefabStack,0);
                }

                if (tempCard2.GetComponent<CardFlipClick>().GetTurnState() == false)
                {
                    tempCard2.GetComponent<CardFlipClick>().ClickStateActivate();
                    // TODO Wait until animation is complete 
                
                    this._riddleCardList.RemoveAt(0);
                    Destroy(tempCard2);
                
                    MoveCardsUp(_riddleCardList);
                    AddCardStack(_riddleCardList,riddleCardPrefab,riddlePrefabStack,-100);
                }
                
                
                
            }else
            {
                timerField.text = "Falscher Spielzug: Überprüfe die Variablentafel! Du hast eine korrektur Möglichkeit!";
                _corectionTimes -= 1;
            }
            
            
            
            
        }

        private void CardStack()
        {
            
            for (int i = 0; i < 5; i++)
            {
                GameObject tempCard = Instantiate(riddleCardPrefab, new Vector3(-400, -100, i*5), riddleCardPrefab.transform.rotation);
                //tempCard.transform.rotation.Set(89,0,0,0 );
                tempCard.GetComponentInChildren<SpriteRenderer>().sprite = riddlePrefabStack[Random.Range(0,riddlePrefabStack.Length)];
                //this._cardStack[i] = tempCard;
                //Karte dort runternehmen und dann den Stapel von unten auffüllen
                _riddleCardList.Add(tempCard);
            }
            for (int i = 0; i < 5; i++)
            {
                GameObject tempCard = Instantiate(cardPrefab, new Vector3(-400, 0, i*5), riddleCardPrefab.transform.rotation);
                tempCard.GetComponentInChildren<SpriteRenderer>().sprite = cardPrefabStack[Random.Range(0,cardPrefabStack.Length)];
                _cardList.Add(tempCard);
            }
            
            //GameObject tempCard2 = this._cardList[4];
            //tempCard2.GetComponent<CardFlipClick>().enabled = true;
            _riddleCardList[0].gameObject.GetComponent<CardFlipClick>().enabled = true;
            //_riddleCardList[0].gameObject.GetComponent<Animator>().enabled = true;
            _cardList[0].gameObject.GetComponent<CardFlipClick>().enabled = true;
            //_cardList[0].gameObject.GetComponent<Animator>().enabled = true;
        }

        private void MoveCardsUp(List<GameObject> cardList)
        {
            foreach (var o in cardList)
            {
                Vector3 tempVector = o.transform.position;
                Debug.Log(tempVector);
                tempVector.z = o.transform.position.z - 5;
                o.transform.position = tempVector;
                Debug.Log(tempVector);
            }
            cardList[0].gameObject.GetComponent<CardFlipClick>().enabled = true;
            
        }

        private void AddCardStack(List<GameObject> cardList, GameObject prefab, Sprite[] spriteStack,int yPosition)
        {
            GameObject tempCard = Instantiate(prefab, new Vector3(-400, yPosition, 20), riddleCardPrefab.transform.rotation);
            tempCard.GetComponentInChildren<SpriteRenderer>().sprite = spriteStack[Random.Range(0,spriteStack.Length)];
            cardList.Add(tempCard);
           
        }

        public void StartRiddle(int correctAnswer, Timer timer)
        {
            inputField.text = "";
            inputFieldEnter = false;
            _timer =  timer;
            _correctAnswer = correctAnswer;
            _timer.timeRemaining = 30;
            //_timer.text = this.timerField;
            _timer.StartTimer();
            
            
            StartCoroutine(Waiter());
            
        }

        public void ActionCard(int newColourNumber)
        {
            inputFieldEnter = false;
            _newColourNumber = newColourNumber;

        }

        void CheckAnswer()
        {
            if (inputField.text.Equals(_correctAnswer.ToString()))
            {
                Debug.Log("correct Answer");
                inputField.text = "Richtige Antwort";
                variablenTafel.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("wrong Answer");
                inputField.text = "Falsche Antwort";
                
            }
        }
        IEnumerator Waiter()
        {
            
            Debug.Log("correct Answer:" + _correctAnswer);
            
            while (_timer.timerIsRunning && _timer.timeRemaining != 0)
            {
                if (inputFieldEnter)
                {
                    _timer.StopTimer();
                    CheckAnswer();
                    yield break;
                } 
                yield return null;
            }
            
            CheckAnswer();
            
        }
    }
}
