using System;
using System.Collections;
using System.Collections.Generic;
using Scenes.Gameboard.Scripts;
using TMPro;
using UnityEngine;

public class ActionCard : MonoBehaviour
{
    private GameController _gameController;
    private SpriteRenderer _spriteRenderer;
    private VariablenTafel _variablenTafel;
    public string _cardName;
    public byte _elementeAnzahl;
    private int index;
    
    
    //New Variables to check with input from players
    private int _colourVariablesNew;
    
    //Start is called before the first frame update
    void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        _variablenTafel = this._gameController.variablenTafel;
        
        _spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        _cardName = _spriteRenderer.sprite.name;
        _elementeAnzahl = Convert.ToByte(_cardName[0].ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CollectAnswer()
    {
        index = 1;
        int elementWert;
        switch (_elementeAnzahl)
        {
            case 1:
                _gameController.variablenTafel.gameObject.SetActive(true);
                elementWert = Convert.ToInt32(_cardName[1..3]);
                if (_cardName[3].ToString() == "+")
                {
                    _colourVariablesNew = this._variablenTafel.GetVar(_gameController.movementScript.GetPositionColour()) +
                                          elementWert;

                }else if (_cardName[3].ToString() == "=")
                {
                    _colourVariablesNew = elementWert;
                }
                break;
            case 2:
                // Range first included position to last not included position
                int turtle = Convert.ToInt32(_cardName[1..3]);
                int crab = Convert.ToInt32(_cardName[4..6]);
                _gameController.inputField.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text =
                    "Wie viel darf deine Figur laufen?";
                if (_gameController.GetPlayerTurn() == 0)
                {
                    _gameController.SetNextMove(crab);
                    //_gameController.movementScript.Movement(crab);
                    //crab
                }else if (_gameController.GetPlayerTurn() == 1)
                {
                    _gameController.SetNextMove(turtle);
                    //turtle
                }

                _colourVariablesNew = -999;
                 
                break;
            case 3:
                _gameController.variablenTafel.gameObject.SetActive(true);
                int positionColour = this._variablenTafel.GetVar(_gameController.movementScript.GetPositionColour());
                if (_cardName[3].ToString() == "g")
                {
                    if (positionColour > Convert.ToInt32(_cardName[1..3]))
                    {
                        _colourVariablesNew = Calculate(_cardName[6].ToString(), positionColour,
                            Convert.ToInt32(_cardName[4..6]));
                    }
                    else
                    {
                        _colourVariablesNew = Calculate(_cardName[9].ToString(), positionColour,
                            Convert.ToInt32(_cardName[7..9]));
                    }
                }else if (_cardName[3].ToString() == "k")
                {
                    if (positionColour < Convert.ToInt32(_cardName[1..3]))
                    {
                        _colourVariablesNew = Calculate(_cardName[6].ToString(), positionColour,
                            Convert.ToInt32(_cardName[4..6]));
                    }
                    else
                    {
                        _colourVariablesNew = Calculate(_cardName[9].ToString(), positionColour,
                            Convert.ToInt32(_cardName[7..9]));
                    }
                }
                break;
        }

        
    }
    private int Calculate(String operation, int firstNumber, int secondNumber)
    {
        int answer = 0;
        if (operation == "+")
        {
            answer = firstNumber + secondNumber;
        }else if (operation == "-")
        {
            answer = firstNumber - secondNumber;
        }else if (operation == ".")
        {
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

    public void StartAction()
    {
        CollectAnswer();
        _gameController.ActionCard(_colourVariablesNew);
        // doch direkt oben verwendet, da nicht bei allen Karten die Variablentafel gebraucht wird...
        //_gameController.variablenTafel.gameObject.SetActive(true);
    }
}
