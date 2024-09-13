using System;
using System.Collections;
using System.Collections.Generic;
using Scenes.Gameboard.Scripts;
using UnityEngine;

public class RiddleScript : MonoBehaviour
{
    private GameController _gameController;
    private SpriteRenderer _spriteRenderer;
    private VariablenTafel _variablenTafel;
    public string _cardName;
    public byte _elementeAnzahl;
    private List<byte> _elemente;
    private List<int> _elementeRightAnswer;
    public int _answer;
    int index;
    
    //Gets the Abzeichen possible to get after an correct answer
    private List<int> _abzeichen;

    private void Start()
    {
        _abzeichen = new List<int>();
        
        _gameController = FindObjectOfType<GameController>();
        _variablenTafel = this._gameController.variablenTafel;
        _elemente = new List<byte>();
        _elementeRightAnswer = new List<int>();
        

        _spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        _cardName = _spriteRenderer.sprite.name;
        _elementeAnzahl = Convert.ToByte(_cardName[0].ToString());
    }

    private void CollectAnswer(){    
        switch (_elementeAnzahl)
        {
            case 2:
                 index = 1;
                
                _elemente.Add(Convert.ToByte(_cardName[1].ToString()));
                _elementeRightAnswer.Add(ElementWert(Convert.ToByte(_cardName[1].ToString())));
                index = index + ElementPlusIndex(Convert.ToByte(_cardName[1].ToString()));
                
                if (_cardName[index].ToString() == "+")
                {
                    _abzeichen.Add(0);
                    Debug.Log(_cardName[index].ToString());
                    _elemente.Add(Convert.ToByte(_cardName[index+1].ToString()));
                    _elementeRightAnswer.Add(ElementWert(Convert.ToByte(_cardName[index+1].ToString())));
                    _answer = _elementeRightAnswer[0] + _elementeRightAnswer[1];
                }else if (_cardName[index].ToString() == "-")
                {
                    _abzeichen.Add(2);
                    Debug.Log(_cardName[index].ToString());
                    _elemente.Add(Convert.ToByte(_cardName[index+1].ToString()));
                    _elementeRightAnswer.Add(ElementWert(Convert.ToByte(_cardName[index+1].ToString())));
                    _answer = _elementeRightAnswer[0] - _elementeRightAnswer[1];
                }else if (_cardName[index].ToString() == ".")
                {
                    _abzeichen.Add(1);
                    Debug.Log(_cardName[index].ToString());
                    _elemente.Add(Convert.ToByte(_cardName[index+1].ToString()));
                    _elementeRightAnswer.Add(ElementWert(Convert.ToByte(_cardName[index+1].ToString())));
                    _answer = _elementeRightAnswer[0] * _elementeRightAnswer[1];
                }
                
                
                break;
            case 3:
                index = 1;
                
                _elemente.Add(Convert.ToByte(_cardName[1].ToString()));
                _elementeRightAnswer.Add(ElementWert(Convert.ToByte(_cardName[1].ToString())));
                index = index + ElementPlusIndex(Convert.ToByte(_cardName[1].ToString()));
                
                if (_cardName[index].ToString() == "+")
                {
                    _abzeichen.Add(0);
                    _elemente.Add(Convert.ToByte(_cardName[index+1].ToString()));
                    _elementeRightAnswer.Add(ElementWert(Convert.ToByte(_cardName[index+1].ToString())));
                    _answer = _elementeRightAnswer[0] + _elementeRightAnswer[1];
                }else if (_cardName[index].ToString() == "-")
                {
                    _abzeichen.Add(2);
                    _elemente.Add(Convert.ToByte(_cardName[index+1].ToString()));
                    _elementeRightAnswer.Add(ElementWert(Convert.ToByte(_cardName[index+1].ToString())));
                    _answer = _elementeRightAnswer[0] - _elementeRightAnswer[1];
                }else if (_cardName[index].ToString() == ".")
                {
                    _abzeichen.Add(1);
                    _elemente.Add(Convert.ToByte(_cardName[index+1].ToString()));
                    _elementeRightAnswer.Add(ElementWert(Convert.ToByte(_cardName[index+1].ToString())));
                    _answer = _elementeRightAnswer[0] * _elementeRightAnswer[1];
                }
                Debug.Log("Index:"+index);
                index = index+1 + ElementPlusIndex(Convert.ToByte(_cardName[index+1].ToString()));
                Debug.Log("Index:"+index);
                
                
                if (_cardName[index].ToString() == "+")
                {
                    _abzeichen.Add(0);
                    _elemente.Add(Convert.ToByte(_cardName[index+1].ToString()));
                    _elementeRightAnswer.Add(ElementWert(Convert.ToByte(_cardName[index+1].ToString())));
                    // out of Bound Error??? deshalb von 2 auf 1 geändert???
                    _answer = _answer + _elementeRightAnswer[2];
                }else if (_cardName[index].ToString() == "-")
                {
                    _abzeichen.Add(2);
                    _elemente.Add(Convert.ToByte(_cardName[index+1].ToString()));
                    _elementeRightAnswer.Add(ElementWert(Convert.ToByte(_cardName[index+1].ToString())));
                    _answer = _answer - _elementeRightAnswer[2];
                }else if (_cardName[index].ToString() == ".")
                {
                    _abzeichen.Add(1);
                    _elemente.Add(Convert.ToByte(_cardName[index+1].ToString()));
                    _elementeRightAnswer.Add(ElementWert(Convert.ToByte(_cardName[index+1].ToString())));
                    _answer = _answer * _elementeRightAnswer[2];
                }
                
                break;
            default:
                _answer = -999;
                break;
        }
    }

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

    private int ElementWert(byte elementCode)
    {
        int elementWert = 0;
        switch (elementCode)
        {
            case 1:
                _abzeichen.Add(5);
                elementWert = this._gameController.movementScript.GetPosition();
                Debug.Log("Position: "+ elementWert);
                break;
            case 2:
                _abzeichen.Add(4);
                Debug.Log(_gameController.movementScript.GetPositionColour());
                Debug.Log(this._variablenTafel.name);
                Debug.Log(this._variablenTafel.GetVar(_gameController.movementScript.GetPositionColour()));
                elementWert = this._variablenTafel.GetVar(_gameController.movementScript.GetPositionColour());
                Debug.Log(elementWert);
                break;
            case 3:
                _abzeichen.Add(4);
                elementWert = this._gameController.GetDiceNumber();
                Debug.Log("DiceRoll: "+elementWert);
                break;
            case 4:
                elementWert = Convert.ToInt32(_cardName[(index+1)..(index+5)]);
                Debug.Log("Nummer auf Karte: "+elementWert);
                break;
            default:
                break;
        }

        return elementWert;
    }

    public void StartRiddle()
    {
        CollectAnswer();
        _gameController.variablenTafel.SwitchGameobjectState(true);
        Timer timer = this.gameObject.AddComponent<Timer>();
        timer.text = _gameController.timerField;
        //timer.StartTimer();
        //timer.timerIsRunning = true;
        _gameController.StartRiddle(_answer, timer, _abzeichen);
        
    }
}
