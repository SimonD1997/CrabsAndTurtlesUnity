using System;
using System.Collections;
using System.Collections.Generic;
using Scenes.Gameboard.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class RiddleScript : MonoBehaviour
{
    private GameController _gameController;
    private SpriteRenderer _spriteRenderer;
    private VariableField _variableField;
    public string cardName;
    public byte elementsNumber;
    private List<byte> _elements;
    private List<int> _elementsRightAnswer;
    private int _walkRightAnswer = 0; 
    public int answer;
    private int _index;
    
    
    private List<int> _badges; //possible badges for the player to earn if riddle solved correctly

    private void Start()
    {
        _badges = new List<int>();
        
        _gameController = FindObjectOfType<GameController>();
        _variableField = this._gameController.variableField;
        _elements = new List<byte>();
        _elementsRightAnswer = new List<int>();
        
        _spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        cardName = _spriteRenderer.sprite.name;
        elementsNumber = Convert.ToByte(cardName[0].ToString());
    }

    
    /// <summary>
    ///  Collects the answer from the card and stores it in the answer variable
    ///  Cards used a specific naming convention to hand over the card information
    /// </summary>
    private void CollectAnswer(){    
        switch (elementsNumber)
        {
            case 2:
                 _index = 1;
                
                _elements.Add(Convert.ToByte(cardName[1].ToString()));
                _elementsRightAnswer.Add(ElementWert(Convert.ToByte(cardName[1].ToString())));
                _index = _index + ElementPlusIndex(Convert.ToByte(cardName[1].ToString()));
                
                if (cardName[_index].ToString() == "+")
                {
                    _badges.Add(0);
                    Debug.Log(cardName[_index].ToString());
                    _elements.Add(Convert.ToByte(cardName[_index+1].ToString()));
                    _elementsRightAnswer.Add(ElementWert(Convert.ToByte(cardName[_index+1].ToString())));
                    answer = _elementsRightAnswer[0] + _elementsRightAnswer[1];
                }else if (cardName[_index].ToString() == "-")
                {
                    _badges.Add(2);
                    Debug.Log(cardName[_index].ToString());
                    _elements.Add(Convert.ToByte(cardName[_index+1].ToString()));
                    _elementsRightAnswer.Add(ElementWert(Convert.ToByte(cardName[_index+1].ToString())));
                    answer = _elementsRightAnswer[0] - _elementsRightAnswer[1];
                }else if (cardName[_index].ToString() == ".")
                {
                    _badges.Add(1);
                    Debug.Log(cardName[_index].ToString());
                    _elements.Add(Convert.ToByte(cardName[_index+1].ToString()));
                    _elementsRightAnswer.Add(ElementWert(Convert.ToByte(cardName[_index+1].ToString())));
                    answer = _elementsRightAnswer[0] * _elementsRightAnswer[1];
                }
                _index = _index+1 + ElementPlusIndex(Convert.ToByte(cardName[_index+1].ToString()));
                _walkRightAnswer = Convert.ToInt32(cardName[_index].ToString());
                 
                
                break;
            case 3:
                _index = 1;
                
                _elements.Add(Convert.ToByte(cardName[1].ToString()));
                _elementsRightAnswer.Add(ElementWert(Convert.ToByte(cardName[1].ToString())));
                _index = _index + ElementPlusIndex(Convert.ToByte(cardName[1].ToString()));
                
                if (cardName[_index].ToString() == "+")
                {
                    _badges.Add(0);
                    _elements.Add(Convert.ToByte(cardName[_index+1].ToString()));
                    _elementsRightAnswer.Add(ElementWert(Convert.ToByte(cardName[_index+1].ToString())));
                    answer = _elementsRightAnswer[0] + _elementsRightAnswer[1];
                }else if (cardName[_index].ToString() == "-")
                {
                    _badges.Add(2);
                    _elements.Add(Convert.ToByte(cardName[_index+1].ToString()));
                    _elementsRightAnswer.Add(ElementWert(Convert.ToByte(cardName[_index+1].ToString())));
                    answer = _elementsRightAnswer[0] - _elementsRightAnswer[1];
                }else if (cardName[_index].ToString() == ".")
                {
                    _badges.Add(1);
                    _elements.Add(Convert.ToByte(cardName[_index+1].ToString()));
                    _elementsRightAnswer.Add(ElementWert(Convert.ToByte(cardName[_index+1].ToString())));
                    answer = _elementsRightAnswer[0] * _elementsRightAnswer[1];
                }
                Debug.Log("Index:"+_index);
                _index = _index+1 + ElementPlusIndex(Convert.ToByte(cardName[_index+1].ToString()));
                Debug.Log("Index:"+_index);
                
                
                if (cardName[_index].ToString() == "+")
                {
                    _badges.Add(0);
                    _elements.Add(Convert.ToByte(cardName[_index+1].ToString()));
                    _elementsRightAnswer.Add(ElementWert(Convert.ToByte(cardName[_index+1].ToString())));
                    // out of Bound Error??? deshalb von 2 auf 1 geändert???
                    answer = answer + _elementsRightAnswer[2];
                }else if (cardName[_index].ToString() == "-")
                {
                    _badges.Add(2);
                    _elements.Add(Convert.ToByte(cardName[_index+1].ToString()));
                    _elementsRightAnswer.Add(ElementWert(Convert.ToByte(cardName[_index+1].ToString())));
                    answer = answer - _elementsRightAnswer[2];
                }else if (cardName[_index].ToString() == ".")
                {
                    _badges.Add(1);
                    _elements.Add(Convert.ToByte(cardName[_index+1].ToString()));
                    _elementsRightAnswer.Add(ElementWert(Convert.ToByte(cardName[_index+1].ToString())));
                    answer = answer * _elementsRightAnswer[2];
                }
                
                _index = _index+1 + ElementPlusIndex(Convert.ToByte(cardName[_index+1].ToString()));
                _walkRightAnswer = Convert.ToInt32(cardName[_index].ToString());
                
                break;
            default:
                answer = -999;
                break;
        }
    }

    /// <summary>
    ///  Returns the index of the next element depending on the element content for reading the answer from card name
    /// </summary>
    /// <param name="elementCode"> element code  </param>
    /// <returns> how long is the element content </returns>
    private int ElementPlusIndex(byte elementCode)
    {
        var index= 0;
        switch (elementCode)
        {
            case 1:
                index += 1;
                break;
            case 2:
                index += 1;
                break;
            case 3:
                index += 1;
                break;
            case 4:
                index += 5;
                break;
            default:
                break;
        }
        
        return index;
    }

    /// <summary>
    ///  Returns the value of the element depending on the element code
    /// </summary>
    /// <param name="elementCode"> element code  </param>
    /// <returns> value of the element </returns>
    private int ElementWert(byte elementCode)
    {
        int elementWert = 0;
        switch (elementCode)
        {
            case 1:
                _badges.Add(5);
                elementWert = this._gameController.movementScript.GetPosition();
                Debug.Log("Position: "+ elementWert);
                break;
            case 2:
                _badges.Add(4);
                elementWert = this._variableField.GetVar(_gameController.movementScript.GetPositionColour());
                Debug.Log(elementWert);
                break;
            case 3:
                _badges.Add(4);
                elementWert = this._gameController.GetDiceNumber();
                Debug.Log("DiceRoll: "+elementWert);
                break;
            case 4:
                elementWert = Convert.ToInt32(cardName[(_index+1)..(_index+5)]);
                Debug.Log("Nummer auf Karte: "+elementWert);
                break;
            default:
                break;
        }

        return elementWert;
    }

    /// <summary>
    /// Starts the riddle and initializes the timer
    /// </summary>
    public void StartRiddle()
    {
        CollectAnswer();
        _gameController.variableField.SwitchGameobjectState(true);
        Timer timer = this.gameObject.AddComponent<Timer>();
        timer.InitTimer(_gameController.textField);
        _gameController.StartRiddle(answer, timer, _badges, _walkRightAnswer);
        
    }
}
