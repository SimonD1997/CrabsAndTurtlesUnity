using System;
using System.Collections;
using System.Collections.Generic;
using Scenes.Gameboard.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ActionCard : MonoBehaviour
{
    private GameController _gameController;
    private SpriteRenderer _spriteRenderer;
    private VariableField _variableField;
    private TextMeshProUGUI _inputField;
    
    public string cardName;
    public byte elementsNumber;
    
    //New Variables to check with input from players
    private int _colourVariablesNew;
    
    private List<int> _badges; // possible badges to get after an correct answer
    
    //Start is called before the first frame update
    void Start()
    {
        _badges = new List<int>();
        
        _gameController = FindObjectOfType<GameController>();
        _variableField = this._gameController.variableField;
        
        _spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        cardName = _spriteRenderer.sprite.name;
        elementsNumber = Convert.ToByte(cardName[0].ToString());

        _inputField = _gameController.inputField.placeholder.gameObject.GetComponent<TextMeshProUGUI>();
    }
    

    /// <summary>
    /// calculates the answer of the card and sets the new colour variable the player has to change into the variable field
    /// Cards used a specific naming convention to hand over the card information
    /// </summary>
    private void CollectAnswer()
    {
        int elementWert;
        switch (elementsNumber)
        {
            case 1:
                _inputField.text = "";
                _gameController.inputField.interactable = false;
                _gameController.variableField.SwitchGameobjectState(true);
                elementWert = Convert.ToInt32(cardName[1..3]);
                if (cardName[3].ToString() == "+")
                {
                    _colourVariablesNew = this._variableField.GetVar(_gameController.movementScript.GetPositionColour()) +
                                          elementWert;
                    _badges.Add(4);

                }else if (cardName[3].ToString() == "=")
                {
                    _colourVariablesNew = elementWert;
                    _badges.Add(4);
                }
                break;
            case 2:
                // Range first included position to last not included position
                int turtle = Convert.ToInt32(cardName[1..3]);
                int crab = Convert.ToInt32(cardName[4..6]);
                _inputField.text = "Wie viel darf deine Figur laufen?";
                _gameController.inputField.interactable = true;
                if (_gameController.GetPlayerTurn() == 0)
                {
                    //crab
                    _gameController.SetNextMove(crab);
                    
                }else if (_gameController.GetPlayerTurn() == 1)
                {
                    //turtle
                    _gameController.SetNextMove(turtle);
                }

                _colourVariablesNew = -999;
                 
                break;
            case 3:
                _inputField.text = "";
                _gameController.inputField.interactable = false;
                
                _badges.Add(3);
                _gameController.variableField.SwitchGameobjectState(true);
                int positionColour = this._variableField.GetVar(_gameController.movementScript.GetPositionColour());
                if (cardName[3].ToString() == "g")
                {
                    if (positionColour > Convert.ToInt32(cardName[1..3]))
                    {
                        _colourVariablesNew = Calculate(cardName[6].ToString(), positionColour,
                            Convert.ToInt32(cardName[4..6]));
                    }
                    else
                    {
                        _colourVariablesNew = Calculate(cardName[9].ToString(), positionColour,
                            Convert.ToInt32(cardName[7..9]));
                    }
                }else if (cardName[3].ToString() == "k")
                {
                    if (positionColour < Convert.ToInt32(cardName[1..3]))
                    {
                        _colourVariablesNew = Calculate(cardName[6].ToString(), positionColour,
                            Convert.ToInt32(cardName[4..6]));
                    }
                    else
                    {
                        _colourVariablesNew = Calculate(cardName[9].ToString(), positionColour,
                            Convert.ToInt32(cardName[7..9]));
                    }
                    
                }
                break;
        }

        
    }
    
    /// <summary>
    /// Calculates two numbers with a given string operation
    /// </summary>
    /// <param name="operation"> string operation: + , - , . , :  </param>
    /// <param name="firstNumber"> number bevor operation</param>
    /// <param name="secondNumber">number after operation</param>
    /// <returns></returns>
    private int Calculate(String operation, int firstNumber, int secondNumber)
    {
        int answer = 0;
        if (operation == "+")
        {
            _badges.Add(0);
            answer = firstNumber + secondNumber;
        }else if (operation == "-")
        {
            _badges.Add(2);
            answer = firstNumber - secondNumber;
        }else if (operation == ".")
        {
            _badges.Add(1);
            answer = firstNumber * secondNumber;
        }else if (operation == ":")
        {
            answer = firstNumber / secondNumber;
        }
        else
        {
            //default error number to debug
            answer = -999;
        }
        return answer;
    }

    
    /// <summary>
    /// Start the action of the card and send the correct answer to the game controller
    /// </summary>
    public void StartAction()
    {
        CollectAnswer();
        _gameController.ActionCard(_colourVariablesNew,_badges);
        
        
    }
}
