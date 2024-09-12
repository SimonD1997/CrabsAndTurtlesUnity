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
    private TextMeshProUGUI _inputField;
    
    public string _cardName;
    public byte _elementeAnzahl;
    
    //New Variables to check with input from players
    private int _colourVariablesNew;

    //Gets the Abzeichen possible to get after an correct answer
    private List<int> _abzeichen;
    
    //Start is called before the first frame update
    void Start()
    {
        _abzeichen = new List<int>();
        
        _gameController = FindObjectOfType<GameController>();
        _variablenTafel = this._gameController.variablenTafel;
        
        _spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        _cardName = _spriteRenderer.sprite.name;
        _elementeAnzahl = Convert.ToByte(_cardName[0].ToString());

        _inputField = _gameController.inputField.placeholder.gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CollectAnswer()
    {
        int elementWert;
        switch (_elementeAnzahl)
        {
            case 1:
                _inputField.text = "";
                _gameController.inputField.interactable = false;
                _gameController.variablenTafel.gameObject.SetActive(true);
                elementWert = Convert.ToInt32(_cardName[1..3]);
                if (_cardName[3].ToString() == "+")
                {
                    _colourVariablesNew = this._variablenTafel.GetVar(_gameController.movementScript.GetPositionColour()) +
                                          elementWert;
                    _abzeichen.Add(4);

                }else if (_cardName[3].ToString() == "=")
                {
                    _colourVariablesNew = elementWert;
                    _abzeichen.Add(4);
                }
                break;
            case 2:
                // Range first included position to last not included position
                int turtle = Convert.ToInt32(_cardName[1..3]);
                int crab = Convert.ToInt32(_cardName[4..6]);
                _inputField.text = "Wie viel darf deine Figur laufen?";
                _gameController.inputField.interactable = true;
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
                _inputField.text = "";
                _gameController.inputField.interactable = false;
                
                _abzeichen.Add(3);
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
            _abzeichen.Add(0);
            answer = firstNumber + secondNumber;
        }else if (operation == "-")
        {
            _abzeichen.Add(2);
            answer = firstNumber - secondNumber;
        }else if (operation == ".")
        {
            _abzeichen.Add(1);
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
        _gameController.ActionCard(_colourVariablesNew,_abzeichen);
        // doch direkt oben verwendet, da nicht bei allen Karten die Variablentafel gebraucht wird...
        //_gameController.variablenTafel.gameObject.SetActive(true);
    }
}
