using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Dreamteck;
using DTT.Rankings.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UIElements.Image;
using Random = UnityEngine.Random;



namespace Scenes.Gameboard.Scripts
{
    public class GameController : MonoBehaviour
    {
        public bool debugMode = false;
        
        private int _diceNumber;
        
        private bool gameIsRunning = false;
        private int playerTurn = 0;
        private int nextMove;
        private int _gameState = 0; // 0 = dice mode, 1 = draw mode, 2 = card mode

        //if an actioncard change an variable the gamecontroller has to check the correct input bevor the next dice roll
        private int _newColourNumber;
        private int _corectionTimes = 1;
        private bool _actionCardState = false;
        private bool _riddleCardState = false;
        
        
        public TextMeshProUGUI yourTurnField ;
        public RawImage playerImage;
        public RenderTexture[] playerImages;
        public MovementScript movementScript;
        private MovementScript lastMovementScript;
        public Abzeichen abzeichen;
        private Abzeichen shownBadges;
        public VariablenTafel variablenTafel;

        public GameObject inventory;
        private Inventory _inventoryScript;
        public GameObject mainMenu;

        public List<MovementScript> playerMovements;
        
        public GameObject cardPrefab;
        public GameObject riddleCardPrefab;
        public Sprite[] cardPrefabStack;
        public Sprite[] riddlePrefabStack;
        
        public Camera m_camera;

        private Timer _timer;
        
        public AudioClip endOfTimerSound;
        private AudioSource _audioSource;
        
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
    
        //Endgame
        public GameObject scoreboardUI;
        public TextMeshProUGUI scoreboardText;
        private int startingPlayer;
        private bool endOfGame;
        public LeaderboardUI _leaderboardUI;
        


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
            
            variablenTafel.SwitchInputFields(false);
            variablenTafel.SwitchGameobjectState(false);
            
            //Starts the Game:
            playerTurn = Random.Range(0, 2);
            startingPlayer = playerTurn;
            
            movementScript = playerMovements[playerTurn];
            lastMovementScript = playerMovements[(playerTurn+1)%2];
            Debug.Log(movementScript.gameObject.name + "  &  " + lastMovementScript.gameObject.name);
            
            abzeichen = movementScript.gameObject.GetComponent<Abzeichen>();
            shownBadges = abzeichen;
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

            _inventoryScript = inventory.GetComponent<Inventory>();

            _audioSource = this.gameObject.GetComponent<AudioSource>();
            
            //_cardStack = new GameObject[5];
            CardStack();
            
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape)){
                if (mainMenu.activeSelf)
                {
                    mainMenu.SetActive(false);
                }
                else
                {
                    mainMenu.SetActive(true);
                }
            }
            
            if (gameIsRunning == true)
            {
                
                
                if (playerTurn == 0)
                {
                    
                    this.yourTurnField.gameObject.SetActive(true);
                    this.yourTurnField.text = "CRAB IST DRAN!";
                    this.playerImage.texture = playerImages[0];
                }else if (playerTurn == 1)
                {
                    
                    this.yourTurnField.gameObject.SetActive(true);
                    this.yourTurnField.text = "TURTLE IST DRAN!";
                    this.playerImage.texture = playerImages[1];
                }
                else
                {
                    TurnOfOtherPlayers();
                }


                if (inputFieldEnter == true && nextMove > 0 && _actionCardState == true)
                {
                    if (Convert.ToInt32(inputField.text) != nextMove)
                    {
                        if (nextMove == 1)
                        {
                            timerField.text = "Dies war die falsche Angabe oder die der anderen Figur! Du darfst "+ nextMove + " Schritt vor gehen!";
                        }
                        else
                        {
                            timerField.text = "Dies war die falsche Angabe oder die der anderen Figur! Du darfst "+ nextMove + " Schritte vor gehen!";
                        }
                        
                    }
                    var otherPosition = lastMovementScript.GetPosition();
                    var position = movementScript.GetPosition();
                    if (position < otherPosition && otherPosition <= position + nextMove)
                    {
                        lastMovementScript.MoveAway(true);
                    }
                    this.movementScript.Movement(nextMove);

                    nextMove = 0;
                    //inputField.text = "";
                    inputFieldEnter = false;
                    //StartCoroutine(WaiterToEndOfMove());
                    
                    //TODO checken ob auch die Karte mit dem weiterkaufen funktioniert. Bzw. bei falscher Eingabe 
                    // sollte der Text noch länger da stehen
                }
            }
            else
            {
              
                this.movementScript.SetCameraActiv(false);
                this.inventory.gameObject.SetActive(false);
                
            }

            if (_gameState == 0)
            {
                movementScript.GetAnimator().SetInteger("Face",0);
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
            
            if (endOfGame)
            {
                EndGame();
                return;
            }
            
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
            movementScript.GetAnimator().SetInteger("Face",0);
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

        /// <summary>
        /// Gets the game state.   
        /// </summary>
        /// <returns>0 when in dice mode
        /// 1 when in draw mode
        /// 2 when in card mode
        /// 3 waiting mode</returns>
        public int GetGameState()
        {
            if (_riddleCardState||_actionCardState)  _gameState = 2;
            
            return _gameState;
        } 
        
        public void EndEditInputField()
        {
            inputFieldEnter = true;
        }

        public void SwitchInventory()
        {
            if (shownBadges == abzeichen)
            {
                Debug.Log("Switch to"+lastMovementScript.gameObject.name);
                shownBadges = lastMovementScript.gameObject.GetComponent<Abzeichen>();
            }
            else
            {
                shownBadges = abzeichen;
            }
            shownBadges.ShowAbzeichen();
        }
        
        /// <summary>
        /// Ends the game and displays the scoreboard with final scores.
        /// </summary>
        private void EndGame()
        {
            
            MovementScript winner = null;
            int winnerID = 0;
            
            // Check if the player who ended the game is not the starting player
            if (playerTurn == startingPlayer)
            {
                // Allow the second player to make one more move
                NextPlayer();
                Debug.Log("Second player gets one more move.");
                
                endOfGame = true;
            }
            else
            {
                this.gameIsRunning = false;
                
                this.yourTurnField.gameObject.SetActive(false);
                
                foreach (var playerMovement in playerMovements)
                {
                    playerMovement.SetCameraActiv(false);
                }
                
                // Calculate final scores (example logic)
                int player1Score = playerMovements[0].GetScore();
                int player2Score = playerMovements[1].GetScore();
                Rank rank1 = new Rank(1, "CRAB", player1Score);
                Rank rank2 = new Rank(2, "TURTLE", player2Score);
                Rank[] rows = { rank1, rank2 };


                if (playerMovements[0].GetEndOfGame() && playerMovements[1].GetEndOfGame())
                {
                    if (player1Score > player2Score)
                    {
                        winner = playerMovements[0];
                        winnerID = 1;
                    }
                    else if (player2Score > player1Score)
                    {
                        winner = playerMovements[1];
                        winnerID = 2;
                    }
                    else if (player1Score == player2Score)
                    {
                        // Both players have the same score
                        Debug.Log("Both players have the same score.");
                    }

                    // Both players have reached the end of the game
                    Debug.Log("Both players have reached the end of the game.");
                }else if (playerMovements[0].GetEndOfGame())
                {
                    winner = playerMovements[0];
                    winnerID = 1;

                }else if (playerMovements[1].GetEndOfGame())
                {
                    winner = playerMovements[1];
                    winnerID = 2;
                }
                

                // Display the scoreboard
                scoreboardUI.SetActive(true);
                _leaderboardUI.AddRank(rows);
                _leaderboardUI.RefreshTable();
                //scoreboardText.text = $"Final Scores:\nPlayer 1: {player1Score}\nPlayer 2: {player2Score}";
                if (winner != null)
                {
                    _leaderboardUI.SetCurrentPlayer(winnerID);
                    scoreboardText.text =winner.gameObject.name + " hat gewonnen!";
                }else
                {
                    scoreboardText.text = "Unentschieden!";
                }
               
                StopAllCoroutines();

                Debug.Log("Game ended. Final scores displayed.");
            }
            
            
        }


        public void ConfirmButtonClick()
        {
            confirmButtonEnter = true;
            inputFieldEnter = true;
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
               
                movementScript.SetCameraActiv(true);
                var otherPosition = lastMovementScript.GetPosition();
                var position = movementScript.GetPosition();
                if (position < otherPosition && otherPosition <= position + diceNumber)
                {
                    lastMovementScript.MoveAway(true);
                }
                movementScript.MoveAway(false);
                this.movementScript.Movement(diceNumber);
                _gameState = 1;

                //TODO is this part needed?
                 //lastMovementScript = movementScript;
                /*if (movementScript.GetEndOfGame())
                {
                    EndGame();
                }*/
                
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
                    AddCardStack(_cardList,cardPrefab,cardPrefabStack,-7);
                }

                if (tempCard2.GetComponent<CardFlipClick>().GetTurnState() == false)
                {
                    
                    
                    StartCoroutine(WaiterAnimator(tempCard2));
                    
                    this._riddleCardList.RemoveAt(0);
                    
                
                    MoveCardsUp(_riddleCardList);
                    AddCardStack(_riddleCardList,riddleCardPrefab,riddlePrefabStack,-23);
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
            
            //TODO Wenn hier schon dann wird der Text beim nur laufen auf der Karte nicht angezeigt
            //timerField.text = "";
            
            _diceNumber = 0;
            
            yield return new WaitForSeconds(2);
            

            if (secondPlayer)
            {
                inputField.text = "";
                _gameState = 0;
            }
            
            if (secondPlayer == false)
            {
                
                confirmButton.interactable = true;
                confirmButtonText.text = "Spielzug beenden";
                
                while (true)
                {
                    if (confirmButtonEnter)
                    {
                        _gameState = 0;
                        
                        confirmButtonText.text = "";
                        confirmButton.interactable = false;
                        
                        timerField.text = "";
                        
                        inputField.text = "";
                        
                        abzeichen.HidePopUp();
                        
                        //If the game ends after the move
                        if (movementScript.GetEndOfGame())
                        {
                            EndGame();
                            yield break;
                        }
                        
                        NextPlayer();
                        
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
                GameObject tempCard = Instantiate(riddleCardPrefab, new Vector3(-35, -23, i), riddleCardPrefab.transform.rotation);
                //tempCard.transform.rotation.Set(89,0,0,0 );
                tempCard.GetComponentInChildren<SpriteRenderer>().sprite = riddlePrefabStack[Random.Range(0,riddlePrefabStack.Length)];
                //this._cardStack[i] = tempCard;
                //Karte dort runternehmen und dann den Stapel von unten auffüllen
                _riddleCardList.Add(tempCard);
            }
            for (int i = 0; i < 5; i++)
            {
                GameObject tempCard = Instantiate(cardPrefab, new Vector3(-35, -7, i), riddleCardPrefab.transform.rotation);
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
            GameObject tempCard = Instantiate(prefab, new Vector3(-35, yPosition, 4), riddleCardPrefab.transform.rotation);
            tempCard.GetComponentInChildren<SpriteRenderer>().sprite = spriteStack[Random.Range(0,spriteStack.Length)];
            cardList.Add(tempCard);
           
        }

        public void StartRiddle(int correctAnswer, Timer timer, List<int> abzeichenList, int walkRightAnswer)
        {
            _riddleCardState = true;
            
            //TODO: is this needed?
            //this.lastMovementScript = this.movementScript;
            
            inputFieldPlaceholder.text = "Eingabefeld";
            inputField.interactable = true;
            inputField.text = "";
            inputFieldEnter = false;
            
            variablenTafel.SwitchInputFields(false);
            
            confirmButton.interactable = true;
            confirmButtonText.text = "Bestätigen";
            
            _timer =  timer;
            _correctAnswer = correctAnswer;
            nextMove = walkRightAnswer;
            
            _timer.SetTimerText("Timer: ");
            _timer.timeRemaining = 30;
            //_timer.text = this.timerField;
            _timer.StartTimer(true);
            
            movementScript.GetAnimator().SetInteger("Face",3);
            
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
                movementScript.GetAnimator().SetInteger("Face",1);
                
                //movement of player
                var otherPosition = lastMovementScript.GetPosition();
                var position = movementScript.GetPosition();
                if (position < otherPosition && otherPosition <= position + nextMove)
                {
                    lastMovementScript.MoveAway(true);
                }
                this.movementScript.Movement(nextMove);
                nextMove = 0;
                
                
                variablenTafel.SwitchGameobjectState(false);
                this.lastMovementScript.SetCameraActiv(false);
                variablenTafel.SwitchInputFields(false);
                
                abzeichen.AddAbzeichen(_abzeichenList);
                abzeichen.ShowAbzeichen();
                abzeichen.ShowPopUp();
                
                _gameState = 3;
                _riddleCardState = false;
                
                //flip Card after correct Answer
                GameObject tempCard = this._riddleCardList[0].gameObject;
                if (tempCard.GetComponent<CardFlipClick>().GetTurnState() == false)
                {
                    StartCoroutine(WaiterAnimator(tempCard));
                    this._riddleCardList.RemoveAt(0);
                    MoveCardsUp(_riddleCardList);
                    AddCardStack(_riddleCardList,riddleCardPrefab,riddlePrefabStack,-23);
                }
                
                StartCoroutine(WaiterToEndOfMove());
                

            }
            else 
            {
                
                
                Debug.Log("wrong Answer");
                inputField.text = "Falsche Antwort";
                movementScript.GetAnimator().SetInteger("Face",4);
                
                if (firstTry)
                {
                    NextPlayer();
                    movementScript.GetAnimator().SetInteger("Face",3);
                    StartCoroutine(Waiter2());  
                }
                else
                {
                    nextMove = 0;
                    
                    variablenTafel.SwitchInputFields(false);
                    variablenTafel.SwitchGameobjectState(false);
                    this.lastMovementScript.SetCameraActiv(false);
                    
                    _gameState = 3;
                    _riddleCardState = false;
                    
                    //flip Card after last Try
                    GameObject tempCard = this._riddleCardList[0].gameObject;
                    if (tempCard.GetComponent<CardFlipClick>().GetTurnState() == false)
                    {
                        StartCoroutine(WaiterAnimator(tempCard));
                        this._riddleCardList.RemoveAt(0);
                        MoveCardsUp(_riddleCardList);
                        AddCardStack(_riddleCardList,riddleCardPrefab,riddlePrefabStack,-23);
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
                    _audioSource.PlayOneShot(endOfTimerSound);
                    _timer.StopTimer();
                    

                    confirmButton.interactable = false;
                    confirmButtonEnter = false;
                    CheckAnswer(true);
                    yield break;
                } 
                yield return null;
            }
            //TODO play Sound to mark the end of the timer
            _audioSource.PlayOneShot(endOfTimerSound);
            
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
            _timer.StartTimer(false);
            
            yield return new WaitForSeconds(5);
            
            inputField.text = "";
            confirmButton.interactable = true;
            inputFieldEnter = false;
            _timer.SetTimerText("Timer: ");
            _timer.timeRemaining = 30;
            _timer.StartTimer(true);
            movementScript.GetAnimator().SetInteger("Face",3);
            
            Debug.Log("correct Answer:" + _correctAnswer);
            
            while (_timer.timerIsRunning && _timer.timeRemaining != 0)
            {
                if (inputFieldEnter)
                {
                    _audioSource.PlayOneShot(endOfTimerSound);
                    _timer.StopTimer();
                    
                    confirmButton.interactable = false;
                    confirmButtonEnter = true;
                    CheckAnswer(false);
                    //important for update if player 2 gets the right answer ?? Eigentlich nicht... da ja in CheckAnswer
                    //abzeichen.ShowAbzeichen();
                    yield break;
                } 
                yield return null;
            }
            _audioSource.PlayOneShot(endOfTimerSound);
            
            confirmButton.interactable = false;
            confirmButtonEnter = true;
            CheckAnswer(false);
            //important for update if player 2 gets the right answer
            //abzeichen.ShowAbzeichen();
            
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
                        timerField.text = "Richtige Antwort!";
                        movementScript.GetAnimator().SetInteger("Face",1);
                    }else
                    {
                        timerField.text = "Falsche Antwort!";
                        movementScript.GetAnimator().SetInteger("Face",4);
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

                    _gameState = 3;
                    _actionCardState = false;
                    
                    inputField.text = "";
                    _corectionTimes = 1;

                    //Karten vom Stapel nehmen, wenn die Karte umgedreht ist.
                    GameObject tempCard = this._cardList[0].gameObject;
                    StartCoroutine(WaiterAnimator(tempCard));
                    this._cardList.RemoveAt(0);
                    //Karten alle um eins nach oben verschieben und neue Karte unten anfügen
                    MoveCardsUp(_cardList);
                    AddCardStack(_cardList, cardPrefab, cardPrefabStack, -7);

                    

                    //sets colour in Variablentafel (-999 stands für player movement and no variable change)
                    if (_newColourNumber != -999)
                    {
                        variablenTafel.SetVar(movementScript.GetPositionColour(), _newColourNumber);

                    }

                    
                    yield break;

                }else if (_newColourNumber == -999 && nextMove == 0)
                {
                    //abzeichen.AddAbzeichen(_abzeichenList);
                    //abzeichen.ShowAbzeichen();
                    
                    confirmButton.interactable = false;
                    confirmButtonEnter = false;
                    StartCoroutine(WaiterToEndOfMove());
                    
                    inputField.text = "";
                    inputFieldPlaceholder.text = "";
                    
                    
                    this.lastMovementScript.SetCameraActiv(false);
                    
                    
                    _actionCardState = false;
                    
                    //Karten vom Stapel nehmen, wenn die Karte umgedreht ist.
                    GameObject tempCard = this._cardList[0].gameObject;
                    StartCoroutine(WaiterAnimator(tempCard));
                    this._cardList.RemoveAt(0);
                    //Karten alle um eins nach oben verschieben und neue Karte unten anfügen
                    MoveCardsUp(_cardList);
                    AddCardStack(_cardList, cardPrefab, cardPrefabStack, -7);
                    
                    
                    
                    yield break;
                }
                else if (confirmButtonEnter)
                {
                    timerField.text =
                        "Falscher Spielzug: Überprüfe die Variablentafel! Du hast eine korrektur Möglichkeit!";
                    movementScript.GetAnimator().SetInteger("Face",2);
                    _corectionTimes -= 1;
                    confirmButtonEnter = false;
                }

                yield return null;
            }
        }
    }
}
