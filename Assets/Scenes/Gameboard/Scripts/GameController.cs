using System;
using System.Collections;
using System.Collections.Generic;
using DTT.Rankings.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;
namespace Scenes.Gameboard.Scripts
{
    public class GameController : MonoBehaviour
    {
        public bool debugMode = false;
        public Camera m_camera;
        
        private int _diceNumber;
        
        private bool _gameIsRunning = false;
        private int _playerTurn = 0;
        private int _startingPlayer;
        private int _nextMove;
        private int _gameState = 0; // 0 = dice mode, 1 = draw mode, 2 = card mode
        private int _newColourNumber;
        private int _corectionTimes = 1;
        private bool _actionCardState = false;
        private bool _riddleCardState = false;
        
        //Player & Movement
        public TextMeshProUGUI yourTurnField ;
        public RawImage playerImage;
        public RenderTexture[] playerImages;
        public MovementScript movementScript;
        private MovementScript _lastMovementScript;
        public List<MovementScript> playerMovements;
        
        //Inventory & Badges
        public Badges badges;
        private Badges _shownBadges;
        public GameObject inventory;
        private Inventory _inventoryScript;
        public GameObject mainMenu;
        public VariableField variableField;
        

        private Timer _timer;
        
        //Audio
        public AudioClip endOfTimerSound;
        private AudioSource _audioSource;
        
        //UI Elements
        private int _correctAnswer;
        public bool inputFieldEnter = false;
        public TMP_InputField inputField;
        public TextMeshProUGUI inputFieldPlaceholder;
        [FormerlySerializedAs("timerField")] public TextMeshProUGUI textField; 
        public TMP_Text confirmButtonText;
        public Button confirmButton;
        public bool confirmButtonEnter = false;
        
        //Cards
        public GameObject actionCardPrefab;
        public GameObject riddleCardPrefab;
        public Sprite[] actionCardPrefabStack;
        public Sprite[] riddlePrefabStack;
        
        //Cards & Badges Lists
        private List<GameObject> _actionCardList;
        private List<GameObject> _riddleCardList;
        private List<int> _badgeList;
    
        //Endgame & Scoreboard
        public GameObject scoreboardUI;
        public TextMeshProUGUI scoreboardText;
        private bool _endOfGame;
        public LeaderboardUI _leaderboardUI;
        

        private void Awake()
        {
            SetHorizontalFieldOfView(90); // Set the horizontal field of view to 90 degrees --> all elements are displayed correctly
            
            _actionCardList = new List<GameObject>();
            _riddleCardList = new List<GameObject>();
            _badgeList = new List<int>();

            _inventoryScript = inventory.GetComponent<Inventory>();

            _audioSource = this.gameObject.GetComponent<AudioSource>();
            
            CardStack();
            
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
        
        // Start is called before the first frame update
        void Start()
        {
           //Sets the game to running --> all elements are displayed correctly or hide for the start
            _gameIsRunning = true;
            textField.text = "";
            inputField.textComponent.enableWordWrapping = true;
            inputFieldPlaceholder = inputField.placeholder.gameObject.GetComponent<TextMeshProUGUI>();
            inputFieldPlaceholder.text = "";
            inputField.interactable = false;
            confirmButtonText.text = "";
            confirmButton.interactable = false;
            
            variableField.SwitchInputFields(false);
            variableField.SwitchGameobjectState(false);
            
            //Starts the Game:
            _playerTurn = Random.Range(0, 2);
            _startingPlayer = _playerTurn;
            
            movementScript = playerMovements[_playerTurn];
            _lastMovementScript = playerMovements[(_playerTurn+1)%2];
            
            badges = movementScript.gameObject.GetComponent<Badges>();
            _shownBadges = badges;
            badges.ShowBadge();
        }
        
        /// <summary>
        /// Initializes the card stack.
        /// </summary>
        private void CardStack()
        {
            
            for (int i = 0; i < 5; i++)
            {
                GameObject tempCard = Instantiate(riddleCardPrefab, new Vector3(-35, -23, i), riddleCardPrefab.transform.rotation);
                tempCard.GetComponentInChildren<SpriteRenderer>().sprite = riddlePrefabStack[Random.Range(0,riddlePrefabStack.Length)];
                _riddleCardList.Add(tempCard);
            }
            for (int i = 0; i < 5; i++)
            {
                GameObject tempCard = Instantiate(actionCardPrefab, new Vector3(-35, -7, i), riddleCardPrefab.transform.rotation);
                tempCard.GetComponentInChildren<SpriteRenderer>().sprite = actionCardPrefabStack[Random.Range(0,actionCardPrefabStack.Length)];
                _actionCardList.Add(tempCard);
            }
            
            //only the first cards are clickable --> delete this an uncomment the part in SetDiceNumber if only one card should be clickable
            _riddleCardList[0].gameObject.GetComponent<CardFlipClick>().enabled = true;
            _actionCardList[0].gameObject.GetComponent<CardFlipClick>().enabled = true;
            
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
            
            if (_gameIsRunning == true)
            {
                
                
                if (_playerTurn == 0)
                {
                    
                    this.yourTurnField.gameObject.SetActive(true);
                    this.yourTurnField.text = "CRAB IST DRAN!";
                    this.playerImage.texture = playerImages[0];
                }else if (_playerTurn == 1)
                {
                    
                    this.yourTurnField.gameObject.SetActive(true);
                    this.yourTurnField.text = "TURTLE IST DRAN!";
                    this.playerImage.texture = playerImages[1];
                }
                else
                {
                    TurnOfOtherPlayers();
                }


                if (inputFieldEnter == true && _nextMove > 0 && _actionCardState == true)
                {
                    int input = 0;
                    if (!string.IsNullOrEmpty(inputField.text))
                    {
                        Debug.LogError("Input is empty!");
                        input = Convert.ToInt32(inputField.text);

                    }
                    
                    if (input != _nextMove)
                    {
                        if (_nextMove == 1)
                        {
                            textField.text = "Dies war die falsche Angabe oder die der anderen Figur! Du darfst 1 Schritt vor gehen!";
                        }
                        else
                        {
                            textField.text = "Dies war die falsche Angabe oder die der anderen Figur! Du darfst "+ _nextMove + " Schritte vor gehen!";
                        }
                        
                    }
                    var otherPosition = _lastMovementScript.GetPosition();
                    var position = movementScript.GetPosition();
                    if (position < otherPosition && otherPosition <= position + _nextMove)
                    {
                        _lastMovementScript.MoveAway(true);
                    }
                    this.movementScript.Movement(_nextMove);

                    _nextMove = 0;
                    inputFieldEnter = false;
                    
                    
                    
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
        
        /// <summary>
        /// not implemented yet --> for online game or computer player
        /// </summary>
        void TurnOfOtherPlayers()
        {
            //todo:
            //wait for other player
            //or make a computer player move
        
        }
        
         /// <summary>
        /// sets the round for new player, checks max player and starts again at player 0
        /// </summary>
        void NextPlayer()
        {
            if (_endOfGame)
            {
                EndGame();
                return;
            }
            

            this._lastMovementScript = this.movementScript;
            
            int maxPlayer = 2;
            if (_playerTurn < maxPlayer - 1)
            {
                _playerTurn += 1;
            }
            else
            {
                _playerTurn = 0;
            }
            this.movementScript = playerMovements[_playerTurn];
            movementScript.GetAnimator().SetInteger("Face",0);
            this.badges = this.movementScript.gameObject.GetComponent<Badges>();
            this.badges.ShowBadge();
        }
         
        /// <summary>
        /// Gets the current player's turn.
        /// </summary>
        /// <returns>The current player's turn.</returns>
        public int GetPlayerTurn()
        {
            return this._playerTurn;
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

        /// <summary>
        /// switches the inventory of the player to the other player's inventory. 
        ///  </summary>
        public void SwitchInventory()
        {
            if (_shownBadges == badges)
            {
                Debug.Log("Switch to"+_lastMovementScript.gameObject.name);
                _shownBadges = _lastMovementScript.gameObject.GetComponent<Badges>();
            }
            else
            {
                _shownBadges = badges;
            }
            _shownBadges.ShowBadge();
        }
        
        /// <summary>
        /// Ends the game and displays the scoreboard with final scores.
        /// Play another round if the player who ended the game is not the starting player.
        /// </summary>
        private void EndGame()
        {
            
            MovementScript winner = null;
            int winnerID = 0;
            
            // Check if the player who ended the game is not the starting player
            if (_playerTurn == _startingPlayer)
            {
                // Allow the second player to make one more move
                NextPlayer();
                Debug.Log("Second player gets one more move.");
                
                _endOfGame = true;
            }
            else
            {
                this._gameIsRunning = false;
                
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

        /// <summary>
        /// Is called when the confirm button is clicked.
        ///  </summary>
        public void ConfirmButtonClick()
        {
            confirmButtonEnter = true;
            inputFieldEnter = true;
        }

        /// <summary>
        /// Sets the next move for the player if the player has to move a certain number of steps after drawing a ActionCard.
        ///  </summary>
        public void SetNextMove(int setNextMove)
        {
            _nextMove = setNextMove;
        }
        
        /// <summary>
        /// Handles the movement of the player. And checks if the other player is in the way. If so, the other player is moved away.
        /// </summary>
        /// <param name="move"> steps to move </param>
        void MovementOfPlayer(int move)
        {
            var otherPosition = _lastMovementScript.GetPosition();
            var position = movementScript.GetPosition();
            
            //Check if the other player is in the way and move him away if necessary
            if (position < otherPosition && otherPosition <= position + move)
            {
                _lastMovementScript.MoveAway(true);
            }
            movementScript.MoveAway(false);
            this.movementScript.Movement(move);
            
        }
        
        
        /// <summary>
        /// Gets the dice number.
        /// </summary>
        /// <returns>The dice number.</returns>
        public int GetDiceNumber()
        {
            return this._diceNumber;
        }
        
        /// <summary>
        /// Sets the dice number. And starts the movement of the player.
        /// Also flips cards if they are turned and removes them from the stack.
        /// </summary>
        /// <param name="diceNumber">The dice number to set.</param>
        public void SetDiceNumber(int diceNumber)
        {
            
            inputField.text = "";

            //Not implemented --> Player can draw both cards
            /*if (diceNumber % 2 == 0)
            {
                _riddleCardList[0].gameObject.GetComponent<CardFlipClick>().enabled = true;
            }
            else
            {
                _actionCardList[0].gameObject.GetComponent<CardFlipClick>().enabled = true;
            }*/
            
            
            if ( (_actionCardState && _riddleCardState) == false)
            {
                
                textField.text = "";
                _corectionTimes = 1;
                
                this._diceNumber = diceNumber;
                
                //Camera from player enabled
                movementScript.SetCameraActiv(true);
                var otherPosition = _lastMovementScript.GetPosition();
                var position = movementScript.GetPosition();
                
                //Check if the other player is in the way and move him away if necessary
                if (position < otherPosition && otherPosition <= position + diceNumber)
                {
                    _lastMovementScript.MoveAway(true);
                }
                movementScript.MoveAway(false);
                this.movementScript.Movement(diceNumber);
                _gameState = 1;
                
                
                this.badges.ShowBadge();
            
                //Karten vom Stapel nehmen, wenn die Karte umgedreht ist.

                GameObject tempCard = this._actionCardList[0].gameObject;
                GameObject tempCard2 = this._riddleCardList[0].gameObject;
                if (tempCard.GetComponent<CardFlipClick>().GetTurnState() == false)
                {
                    
                    StartCoroutine(WaiterAnimator(tempCard));
                    
                    this._actionCardList.RemoveAt(0);
                    
                
                    //Karten alle um eins nach oben verschieben und neue Karte unten anfügen
                
                    MoveCardsUp(_actionCardList);
                    AddCardStack(_actionCardList,actionCardPrefab,actionCardPrefabStack,-7);
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
        
        
        // Scripts to handle RiddleCards
        
        /// <summary>
        /// Gets the necessary information from the riddle card and starts the riddle.
        /// </summary>
        /// <param name="correctAnswer"> the correct answer of the riddle </param>
        /// <param name="timer"> the timer object </param>
        /// <param name="badgeList"> a list of badges the player get if the answer is correct </param>
        /// <param name="walkRightAnswer">steps to move after right answer </param>
        public void StartRiddle(int correctAnswer, Timer timer, List<int> badgeList, int walkRightAnswer)
        {
            _riddleCardState = true;
            
            // Set the UI Elements to the correct state
            inputFieldPlaceholder.text = "Eingabefeld";
            inputField.interactable = true;
            inputField.text = "";
            inputFieldEnter = false;
            
            variableField.SwitchInputFields(false);
            
            confirmButton.interactable = true;
            confirmButtonText.text = "Bestätigen";
            
            _timer =  timer;
            _correctAnswer = correctAnswer;
            _nextMove = walkRightAnswer;
            
            _timer.SetTimerText("Timer: ");
            _timer.timeRemaining = 30;
            //_timer.text = this.timerField;
            _timer.StartTimer(true);
            
            movementScript.GetAnimator().SetInteger("Face",3);
            
            _badgeList = badgeList;
            
            StartCoroutine(WaiterRiddleCard1());
            
        }
        
        /// <summary>
        /// Checks the answer of the player and moves the player accordingly if the answer is correct and ends the round.
        /// If the answer is wrong, the other player gets a chance to answer the riddle.
        ///  </summary>
        void CheckAnswer(bool firstTry)
        {
            
            if (inputField.text.Equals(_correctAnswer.ToString()))
            {
                Debug.Log("correct Answer");
                textField.text = "Richtige Antwort";
                //inputField.text = "Richtige Antwort";
                movementScript.GetAnimator().SetInteger("Face",1);
                
                //movement of player
                MovementOfPlayer(_nextMove);
                
                _nextMove = 0;
                
                
                variableField.SwitchGameobjectState(false);
                this._lastMovementScript.SetCameraActiv(false);
                variableField.SwitchInputFields(false);
                
                badges.AddBadges(_badgeList);
                badges.ShowBadge();
                badges.ShowPopUp();
                
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
                //inputField.text = "Falsche Antwort";
                textField.text = "Falsche Antwort";
                movementScript.GetAnimator().SetInteger("Face",4);
                
                if (firstTry)
                {
                    NextPlayer();
                    movementScript.GetAnimator().SetInteger("Face",3);
                    StartCoroutine(WaiterRiddleCard2());  
                }
                else
                {
                    _nextMove = 0;
                    
                    variableField.SwitchInputFields(false);
                    variableField.SwitchGameobjectState(false);
                    this._lastMovementScript.SetCameraActiv(false);
                    
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
            
            
        }
        
        /// <summary>
        /// Coroutine to wait during the timer is running and the player can answer the riddle.
        ///  </summary>
        IEnumerator WaiterRiddleCard1()
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
            
            _audioSource.PlayOneShot(endOfTimerSound);
            
            confirmButton.interactable = false;
            confirmButtonEnter = false;
            CheckAnswer(true);
        }
        
        /// <summary>
        /// Waiter and Timer for second try from second player after wrong answer
        /// </summary>
        /// <returns></returns>
        IEnumerator WaiterRiddleCard2()
        {
            _timer.SetTimerText("Zeit zum weitergeben: ");
            _timer.timeRemaining = 5;
            inputField.text = "Falsche Antwort! Das andere Team bekommt die Chance!";
            _timer.StartTimer(false);
            
            yield return new WaitForSeconds(5);
            
            //TODO: Confirm Button to switch to next player or something like that
            
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
                    yield break;
                } 
                yield return null;
            }
            _audioSource.PlayOneShot(endOfTimerSound);
            
            confirmButton.interactable = false;
            confirmButtonEnter = true;
            CheckAnswer(false);
            
        }
        
        
        //Scripts to handle ActionCards
        
        /// <summary>
        ///  Gets the necessary information from the action card and starts the action. The player has to enter the correct variable.
        /// </summary>
        /// <param name="newColourNumber"> the new variable to put in the variable field. -999 if the movement card is drawn</param>
        /// <param name="badgeList"> a list of badges the player get if the answer is correct </param>
        public void ActionCard(int newColourNumber,List<int> badgeList)
        {
            inputFieldEnter = false;
            _newColourNumber = newColourNumber;
            _actionCardState = true;
            _badgeList = badgeList;
            
            variableField.SwitchInputFields(true);
            
            confirmButton.interactable = true;
            confirmButtonText.text = "Eingabe bestätigen";
            
            StartCoroutine(WaiterActionCard());

        }
        
        /// <summary>
        /// Coroutine to handle the action card.
        /// waits for the player to enter the correct variable and press the confirm button.
        /// </summary>
        /// <returns></returns>
        IEnumerator WaiterActionCard()
        {
            confirmButtonEnter = false;
            while (true)
            {
                //check if the player has entered the correct variable combination in the variable field and pressed the confirm button
                if ((_newColourNumber == variableField.GetVar(movementScript.GetPositionColour()) ||
                     _corectionTimes == 0) && confirmButtonEnter)
                {
                    variableField.SwitchGameobjectState(false);

                    if (_newColourNumber == variableField.GetVar(movementScript.GetPositionColour()))
                    {
                        textField.text = "Richtige Antwort!";
                        movementScript.GetAnimator().SetInteger("Face",1);
                        MovementOfPlayer(1);
                    }else
                    {
                        textField.text = "Falsche Antwort!";
                        movementScript.GetAnimator().SetInteger("Face",4);
                    }
                    
                    if (_corectionTimes == 1)
                    {
                        badges.AddBadges(_badgeList);
                        badges.ShowBadge();
                        badges.ShowPopUp();

                    }
                    this._lastMovementScript.SetCameraActiv(false);
                    
                    confirmButton.interactable = false;
                    confirmButtonEnter = false;
                    StartCoroutine(WaiterToEndOfMove());

                    _gameState = 3;
                    _actionCardState = false;
                    
                    inputField.text = "";
                    _corectionTimes = 1;

                    //remove the card from the stack and add a new card
                    GameObject tempCard = this._actionCardList[0].gameObject;
                    StartCoroutine(WaiterAnimator(tempCard));
                    this._actionCardList.RemoveAt(0);
                    MoveCardsUp(_actionCardList);
                    AddCardStack(_actionCardList, actionCardPrefab, actionCardPrefabStack, -7);

                    

                    //sets colour in variable field (-999 is for player movement and no variable change)
                    if (_newColourNumber != -999)
                    {
                        variableField.SetVar(movementScript.GetPositionColour(), _newColourNumber);

                    }
                    
                    yield break;

                    // if card is a movement card and no variable change. 
                    // Movement is done in Update() after the player has entered the correct number
                }else if (_newColourNumber == -999 && _nextMove == 0)
                {
                    
                    confirmButton.interactable = false;
                    confirmButtonEnter = false;
                    StartCoroutine(WaiterToEndOfMove());
                    
                    inputField.text = "";
                    inputFieldPlaceholder.text = "";
                    
                    
                    this._lastMovementScript.SetCameraActiv(false);
                    
                    
                    _actionCardState = false;
                    
                    //remove the card from the stack and add a new card
                    GameObject tempCard = this._actionCardList[0].gameObject;
                    StartCoroutine(WaiterAnimator(tempCard));
                    this._actionCardList.RemoveAt(0);
                    MoveCardsUp(_actionCardList);
                    AddCardStack(_actionCardList, actionCardPrefab, actionCardPrefabStack, -7);
                    
                    
                    
                    yield break;
                }
                else if (confirmButtonEnter)
                {
                    textField.text =
                        "Falscher Spielzug: Überprüfe die Variablentafel! Du hast eine korrektur Möglichkeit!";
                    movementScript.GetAnimator().SetInteger("Face",2);
                    _corectionTimes -= 1;
                    confirmButtonEnter = false;
                }

                yield return null;
            }
        }
        
        
        // End of Move 
        
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
            
            
            // different behaviour if the next player is already set course of a wrong answer in the riddle
            if (secondPlayer)
            {
                _gameState = 0;
                // remove the response text after a delay
                yield return new WaitForSeconds(2);
                inputField.text = "";
                textField.text = "";
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
                        
                        textField.text = "";
                        inputField.text = "";
                        
                        badges.HidePopUp();
                        
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
    }
}
