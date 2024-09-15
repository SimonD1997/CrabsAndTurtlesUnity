using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Dreamteck;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

namespace Scenes.Gameboard.Scripts
{
    public class GameController : MonoBehaviour
    {
        private int _diceNumber;
        
        private bool gameIsRunning = false;
        private int playerTurn = 0;
        private int nextMove;

        //if an actioncard change an variable the gamecontroller has to check the correct input bevor the next dice roll
        private int _newColourNumber;
        private int _corectionTimes = 1;
        private bool _actionCardState = false;
        
        
        public TextMeshProUGUI yourTurnField ;
        public MovementScript movementScript;
        private MovementScript lastMovementScript;
        public Abzeichen abzeichen;
        public VariablenTafel variablenTafel;

        public Inventory inventory;

        public List<MovementScript> playerMovements;
        
        public GameObject cardPrefab;
        public GameObject riddleCardPrefab;
        public Sprite[] cardPrefabStack;
        public Sprite[] riddlePrefabStack;
        
        public Camera m_camera;

        private Timer _timer;
        private int _correctAnswer;
        public bool inputFieldEnter = false;
        public TMP_InputField inputField;
        public TextMeshProUGUI inputFieldPlaceholder;
        public TextMeshProUGUI timerField; 
        public TMP_Text confirmButtonText;
        public Button confirmButton;
        public bool confirmButtonEnter = false;
        
        //private GameObject[] _cardStack;
        private List<GameObject> _cardList;
        private List<GameObject> _riddleCardList;
        private List<int> _abzeichenList;
    
    
        // Start is called before the first frame update
        void Start()
        {
           // Debug.Log(this._cardStack.Length.ToString());
            this.gameIsRunning = true;
            timerField.text = "";
            this.inputField.textComponent.enableWordWrapping = true;
            inputFieldPlaceholder = inputField.placeholder.gameObject.GetComponent<TextMeshProUGUI>();
            inputFieldPlaceholder.text = "";
            inputField.interactable = false;
            confirmButtonText.text = "";
            confirmButton.interactable = false;
            
            variablenTafel.SwitchGameobjectState(false);
            
            //Starts the Game:
            playerTurn = Random.Range(0, 2);
            
            movementScript = playerMovements[playerTurn];
            lastMovementScript = movementScript;
            
            abzeichen = movementScript.gameObject.GetComponent<Abzeichen>();
            // Warum auch immer gibt es Nullpointer exeptions TODO. Herausfinden warum... 
            abzeichen.ShowAbzeichen();
        }
        /// <summary>
        /// Sets the horizontal field of view for the camera.
        /// </summary>
        /// <param name="horizontalFOV">The desired horizontal field of view in degrees.</param>
        private void SetHorizontalFieldOfView(float horizontalFOV)
        {
            float verticalFOV = 2 * Mathf.Atan(Mathf.Tan(horizontalFOV * Mathf.Deg2Rad / 2) / m_camera.aspect) * Mathf.Rad2Deg;
            m_camera.fieldOfView = verticalFOV;
        }
        
        private void Awake()
        {
            SetHorizontalFieldOfView(90);
            
            _cardList = new List<GameObject>();
            _riddleCardList = new List<GameObject>();
            _abzeichenList = new List<int>();
            
            
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
                    this.yourTurnField.text = "CRAB IST DRAN!";
                }else if (playerTurn == 1)
                {
                    
                    this.yourTurnField.gameObject.SetActive(true);
                    this.yourTurnField.text = "TURTLE IST DRAN!";
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
         /// <summary>
        /// sets the round for new player, checks max player and starts again at player 0
        /// </summary>
        void NextPlayer()
        {
            // sonst wird beim zweiten player/versuch die abzeichen nicht angefügt...
            //_abzeichenList.Clear();

            this.lastMovementScript = this.movementScript;
            
            int maxPlayer = 2;
            if (playerTurn < maxPlayer - 1)
            {
                playerTurn += 1;
            }
            else
            {
                playerTurn = 0;
            }
            this.movementScript = playerMovements[playerTurn];
            this.abzeichen = this.movementScript.gameObject.GetComponent<Abzeichen>();
            this.abzeichen.ShowAbzeichen();
        }
        /// <summary>
        /// Gets the current player's turn.
        /// </summary>
        /// <returns>The current player's turn.</returns>
        public int GetPlayerTurn()
        {
            return this.playerTurn;
        }
        
        public void EndEditInputField()
        {
            inputFieldEnter = true;
        }

        public void ConfirmButtonClick()
        {
            confirmButtonEnter = true;
        }

        public void SetNextMove(int setNextMove)
        {
            nextMove = setNextMove;
        }
        /// <summary>
        /// Gets the dice number.
        /// </summary>
        /// <returns>The dice number.</returns>
        public int GetDiceNumber()
        {
            return this._diceNumber;
        }
        
        // <summary>
        /// Sets the dice number. And starts the movement of the player.
        /// Also flips cards if they are turned and removes them from the stack.
        /// </summary>
        /// <param name="diceNumber">The dice number to set.</param>
        public void SetDiceNumber(int diceNumber)
        {
            
            inputField.text = "";
            
            if ( _actionCardState == false)
            {
                
                timerField.text = "";
                _corectionTimes = 1;
                
                this._diceNumber = diceNumber;
                
                //Camera from player enabled
                lastMovementScript = movementScript;
                lastMovementScript.SetCameraActiv(true);
                this.movementScript.Movement(diceNumber);
                
                this.abzeichen.ShowAbzeichen();
            
                //Karten vom Stapel nehmen, wenn die Karte umgedreht ist.

                GameObject tempCard = this._cardList[0].gameObject;
                GameObject tempCard2 = this._riddleCardList[0].gameObject;
                if (tempCard.GetComponent<CardFlipClick>().GetTurnState() == false)
                {
                    
                    StartCoroutine(WaiterAnimator(tempCard));
                    
                    this._cardList.RemoveAt(0);
                    
                
                    //Karten alle um eins nach oben verschieben und neue Karte unten anfügen
                
                    MoveCardsUp(_cardList);
                    AddCardStack(_cardList,cardPrefab,cardPrefabStack,4);
                }

                if (tempCard2.GetComponent<CardFlipClick>().GetTurnState() == false)
                {
                    
                    
                    StartCoroutine(WaiterAnimator(tempCard2));
                    
                    this._riddleCardList.RemoveAt(0);
                    
                
                    MoveCardsUp(_riddleCardList);
                    AddCardStack(_riddleCardList,riddleCardPrefab,riddlePrefabStack,-15);
                }
                
            }
            
        }
        /// <summary>
        /// Coroutine to animate the card flip and destroy the card after a delay.
        /// </summary>
        /// <param name="tempCard">The card to animate and destroy.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        IEnumerator WaiterAnimator(GameObject tempCard)
        {
            yield return new WaitForSeconds(0.5f);
            tempCard.GetComponent<CardFlipClick>().ClickStateActivate();
            yield return new WaitForSeconds(5);
            Destroy(tempCard);
            
        }
        /// <summary>
        /// Coroutine to handle the end of a move.
        /// </summary>
        /// <returns>An IEnumerator for the coroutine.</returns>
        IEnumerator WaiterToEndOfMove()
        {
            
            
            bool secondPlayer = confirmButtonEnter == true;
            confirmButtonEnter = false;
            confirmButtonText.text = "";
            inputField.interactable = false;
            inputFieldPlaceholder.text = "";
            timerField.text = "";
            
            yield return new WaitForSeconds(3);

            inputField.text = "";
            timerField.text = "";
            
            if (secondPlayer == false)
            {
                confirmButton.interactable = true;
                confirmButtonText.text = "Spielzug beenden";
                
                while (true)
                {
                    if (confirmButtonEnter)
                    {
                        confirmButtonText.text = "";
                        confirmButton.interactable = false;
                        
                        NextPlayer();
                    
                        timerField.text = "";
                        yield break;
                    }
                
                    yield return null;
                }
                
            }
            
            
        }
        /// <summary>
        /// Initializes the card stack.
        /// </summary>
        private void CardStack()
        {
            
            for (int i = 0; i < 5; i++)
            {
                GameObject tempCard = Instantiate(riddleCardPrefab, new Vector3(-40, -15, i), riddleCardPrefab.transform.rotation);
                //tempCard.transform.rotation.Set(89,0,0,0 );
                tempCard.GetComponentInChildren<SpriteRenderer>().sprite = riddlePrefabStack[Random.Range(0,riddlePrefabStack.Length)];
                //this._cardStack[i] = tempCard;
                //Karte dort runternehmen und dann den Stapel von unten auffüllen
                _riddleCardList.Add(tempCard);
            }
            for (int i = 0; i < 5; i++)
            {
                GameObject tempCard = Instantiate(cardPrefab, new Vector3(-40, 4, i), riddleCardPrefab.transform.rotation);
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
        /// <summary>
        /// Moves the cards up in the stack.
        /// </summary>
        /// <param name="cardList">The list of cards to move up.</param>
        private void MoveCardsUp(List<GameObject> cardList)
        {
            foreach (var o in cardList)
            {
                Vector3 tempVector = o.transform.position;
                Debug.Log(tempVector);
                tempVector.z = o.transform.position.z - 1;
                o.transform.position = tempVector;
                Debug.Log(tempVector);
            }
            cardList[0].gameObject.GetComponent<CardFlipClick>().enabled = true;
            
        }
        /// <summary>
        /// Adds a card to the stack.
        /// </summary>
        /// <param name="cardList">The list of cards to add to.</param>
        /// <param name="prefab">The card prefab to instantiate.</param>
        /// <param name="spriteStack">The array of sprites to choose from.</param>
        /// <param name="yPosition">The y position to place the card at.</param>
        private void AddCardStack(List<GameObject> cardList, GameObject prefab, Sprite[] spriteStack,int yPosition)
        {
            GameObject tempCard = Instantiate(prefab, new Vector3(-40, yPosition, 4), riddleCardPrefab.transform.rotation);
            tempCard.GetComponentInChildren<SpriteRenderer>().sprite = spriteStack[Random.Range(0,spriteStack.Length)];
            cardList.Add(tempCard);
           
        }

        public void StartRiddle(int correctAnswer, Timer timer, List<int> abzeichenList)
        {
            this.lastMovementScript = this.movementScript;
            
            inputFieldPlaceholder.text = "Eingabefeld";
            inputField.interactable = true;
            inputField.text = "";
            inputFieldEnter = false;
            
            variablenTafel.SwitchInputFields(false);
            
            confirmButton.interactable = true;
            confirmButtonText.text = "Bestätigen";
            
            _timer =  timer;
            _correctAnswer = correctAnswer;
            _timer.SetTimerText("Timer: ");
            _timer.timeRemaining = 30;
            //_timer.text = this.timerField;
            _timer.StartTimer();
            
            _abzeichenList = abzeichenList;
            
            StartCoroutine(Waiter());
            
        }

        public void ActionCard(int newColourNumber,List<int> abzeichenList)
        {
            inputFieldEnter = false;
            //inputFieldPlaceholder.text = ""; set in class ActionCard
            _newColourNumber = newColourNumber;
            _actionCardState = true;
            _abzeichenList = abzeichenList;
            
            variablenTafel.SwitchInputFields(true);
            
            confirmButton.interactable = true;
            confirmButtonText.text = "Eingabe bestätigen";
            
            StartCoroutine(Waiter3());

        }

        void CheckAnswer(bool firstTry)
        {
            
            if (inputField.text.Equals(_correctAnswer.ToString()))
            {
                Debug.Log("correct Answer");
                inputField.text = "Richtige Antwort";
                
                variablenTafel.SwitchGameobjectState(false);
                this.lastMovementScript.SetCameraActiv(false);
                variablenTafel.SwitchInputFields(false);
                
                abzeichen.AddAbzeichen(_abzeichenList);
                abzeichen.ShowAbzeichen();
                abzeichen.ShowPopUp();
                
                //flip Card after correct Answer
                GameObject tempCard = this._riddleCardList[0].gameObject;
                if (tempCard.GetComponent<CardFlipClick>().GetTurnState() == false)
                {
                    StartCoroutine(WaiterAnimator(tempCard));
                    this._riddleCardList.RemoveAt(0);
                    MoveCardsUp(_riddleCardList);
                    AddCardStack(_riddleCardList,riddleCardPrefab,riddlePrefabStack,-15);
                }
                
                StartCoroutine(WaiterToEndOfMove());
                

            }
            else 
            {
                Debug.Log("wrong Answer");
                inputField.text = "Falsche Antwort";
                if (firstTry)
                {
                    NextPlayer();
                    StartCoroutine(Waiter2());  
                }
                else
                {
                    
                    variablenTafel.SwitchInputFields(false);
                    variablenTafel.SwitchGameobjectState(false);
                    this.lastMovementScript.SetCameraActiv(false);
                    
                    //flip Card after last Try
                    GameObject tempCard = this._riddleCardList[0].gameObject;
                    if (tempCard.GetComponent<CardFlipClick>().GetTurnState() == false)
                    {
                        StartCoroutine(WaiterAnimator(tempCard));
                        this._riddleCardList.RemoveAt(0);
                        MoveCardsUp(_riddleCardList);
                        AddCardStack(_riddleCardList,riddleCardPrefab,riddlePrefabStack,-15);
                    }
                    
                    StartCoroutine(WaiterToEndOfMove());
                }
            }

            if (firstTry)
            {
                //NextPlayer();
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

                    confirmButton.interactable = false;
                    confirmButtonEnter = false;
                    CheckAnswer(true);
                    yield break;
                } 
                yield return null;
            }
            confirmButton.interactable = false;
            confirmButtonEnter = false;
            CheckAnswer(true);
        }
        /// <summary>
        /// Waiter and Timer for second try from second player after wrong answer
        /// </summary>
        /// <returns></returns>
        IEnumerator Waiter2()
        {
            _timer.SetTimerText("Zeit zum weitergeben: ");
            _timer.timeRemaining = 5;
            inputField.text = "Falsche Antwort! Das andere Team bekommt die Chance!";
            _timer.StartTimer();
            yield return new WaitForSeconds(5);
            
            inputField.text = "";
            confirmButton.interactable = true;
            inputFieldEnter = false;
            _timer.SetTimerText("Timer: ");
            _timer.timeRemaining = 30;
            _timer.StartTimer();
            
            Debug.Log("correct Answer:" + _correctAnswer);
            
            while (_timer.timerIsRunning && _timer.timeRemaining != 0)
            {
                if (inputFieldEnter)
                {
                    _timer.StopTimer();
                    
                    confirmButton.interactable = false;
                    confirmButtonEnter = true;
                    CheckAnswer(false);
                    //important for update if player 2 gets the right answer
                    abzeichen.ShowAbzeichen();
                    abzeichen.ShowPopUp();
                    yield break;
                } 
                yield return null;
            }
            
            confirmButton.interactable = false;
            confirmButtonEnter = true;
            CheckAnswer(false);
            //important for update if player 2 gets the right answer
            abzeichen.ShowAbzeichen();
            abzeichen.ShowPopUp();
            
        }

        IEnumerator Waiter3()
        {
            confirmButtonEnter = false;
            while (true)
            {
                
                ///checks correct variablentafel bevor let the player roll the next dice
                if ((_newColourNumber == variablenTafel.GetVar(movementScript.GetPositionColour()) ||
                     _corectionTimes == 0) && confirmButtonEnter)
                {
                    variablenTafel.SwitchGameobjectState(false);

                    if (_newColourNumber == variablenTafel.GetVar(movementScript.GetPositionColour()))
                    {
                        inputField.text = "Richtige Antwort!"; 
                    }
                    
                    if (_corectionTimes == 1)
                    {
                        // AbzeichenTest 
                        abzeichen.AddAbzeichen(_abzeichenList);
                        abzeichen.ShowAbzeichen();
                        abzeichen.ShowPopUp();
                        
                    }
                    this.lastMovementScript.SetCameraActiv(false);
                    
                    confirmButton.interactable = false;
                    confirmButtonEnter = false;
                    StartCoroutine(WaiterToEndOfMove());
                    
                    _actionCardState = false;


                    timerField.text = "";
                    _corectionTimes = 1;

                    //Karten vom Stapel nehmen, wenn die Karte umgedreht ist.
                    GameObject tempCard = this._cardList[0].gameObject;
                    StartCoroutine(WaiterAnimator(tempCard));
                    this._cardList.RemoveAt(0);
                    //Karten alle um eins nach oben verschieben und neue Karte unten anfügen
                    MoveCardsUp(_cardList);
                    AddCardStack(_cardList, cardPrefab, cardPrefabStack, 4);

                    

                    //sets colour in Variablentafel (-999 stands für player movement and no variable change)
                    if (_newColourNumber != -999)
                    {
                        variablenTafel.SetVar(movementScript.GetPositionColour(), _newColourNumber);

                    }

                    
                    yield break;

                }else if (confirmButtonEnter)
                {
                    timerField.text =
                        "Falscher Spielzug: Überprüfe die Variablentafel! Du hast eine korrektur Möglichkeit!";
                    _corectionTimes -= 1;
                    confirmButtonEnter = false;
                }else if (_newColourNumber == -999 && nextMove == 0)
                {
                    //abzeichen.AddAbzeichen(_abzeichenList);
                    //abzeichen.ShowAbzeichen();
                    
                    confirmButton.interactable = false;
                    confirmButtonEnter = false;
                    StartCoroutine(WaiterToEndOfMove());
                    
                    this.lastMovementScript.SetCameraActiv(false);
                    
                    
                    _actionCardState = false;
                    inputField.text = "";
                    inputFieldPlaceholder.text = "";
                    
                    //Karten vom Stapel nehmen, wenn die Karte umgedreht ist.
                    GameObject tempCard = this._cardList[0].gameObject;
                    StartCoroutine(WaiterAnimator(tempCard));
                    this._cardList.RemoveAt(0);
                    //Karten alle um eins nach oben verschieben und neue Karte unten anfügen
                    MoveCardsUp(_cardList);
                    AddCardStack(_cardList, cardPrefab, cardPrefabStack, 4);
                    
                    
                    
                    yield break;
                }

                yield return null;
            }
        }
    }
}
