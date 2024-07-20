using System;
using System.Collections;
using System.Collections.Generic;
using Scenes.Gameboard.Scripts;
using UnityEngine;

public class RiddleScript : MonoBehaviour
{
    private MonoBehaviour _gameController;
    private SpriteRenderer _spriteRenderer;
    private string _cardName;
    private byte _elementeAnzahl;
    private List<byte> _elemente;
    private List<int> _elementeRightAnswer;
    private int _answer;
    

    private void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        
        _spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        _cardName = _spriteRenderer.sprite.name;
        _elementeAnzahl = Convert.ToByte(_cardName[0]);
        
        switch (_elementeAnzahl)
        {
            case 2:
                int index = 1;
                
                _elemente.Add(Convert.ToByte(_cardName[1]));
                _elementeRightAnswer.Add(ElementWert(Convert.ToByte(_cardName[1])));
                index = index + ElementPlusIndex(Convert.ToByte(_cardName[1]));
                
                if (_cardName[index].ToString() == "+")
                {
                    _elemente.Add(Convert.ToByte(_cardName[index+1]));
                    _elementeRightAnswer.Add(ElementWert(Convert.ToByte(_cardName[index+1])));
                    _answer = _elementeRightAnswer[0] + _elementeRightAnswer[1];
                }else if (_cardName[index].ToString() == "-")
                {
                    _elemente.Add(Convert.ToByte(_cardName[index+1]));
                    _elementeRightAnswer.Add(ElementWert(Convert.ToByte(_cardName[index+1])));
                    _answer = _elementeRightAnswer[0] - _elementeRightAnswer[1];
                }else if (_cardName[index].ToString() == ".")
                {
                    _elemente.Add(Convert.ToByte(_cardName[index+1]));
                    _elementeRightAnswer.Add(ElementWert(Convert.ToByte(_cardName[index+1])));
                    _answer = _elementeRightAnswer[0] * _elementeRightAnswer[1];
                }
                
                
                break;
            case 3:
                
                
                break;
            default:
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
                index += 4;
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
                
                break;
            case 2:
                
                break;
            case 3:
                
                break;
            case 4:
                
                break;
            default:
                break;
        }

        return elementWert;
    }
    
}
