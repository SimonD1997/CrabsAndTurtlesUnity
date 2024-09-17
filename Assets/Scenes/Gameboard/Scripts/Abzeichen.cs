using System;
using System.Collections;
using System.Collections.Generic;
using Effekseer;
using UnityEngine;

namespace Scenes.Gameboard.Scripts
{
    public class Abzeichen : MonoBehaviour
    {
        private GameController _gameController;

        private List<AbzeichenObjects> _abzeichenList;
        public Sprite[] _spriteList;
        private Inventory _inventory;
        public Sprite spriteIcon;
        private PopUp _popUp;
        private GameObject _popUpGameobject;
        private List<Sprite> _popUpSprites;
        
        public EffekseerEffectAsset effect;
        public EffekseerEmitter emitter;
        
/// <summary>
/// Abzeichennummern
/// 0 = +
/// 1 = *
/// 2 = -
/// 3 = Bedingung
/// 4 = Variable
/// 5 = Konstante
/// </summary>
/// <param name="abzeichenNumber"></param>
        public void AddAbzeichen(List<int> abzeichenNumber)
        {
            int[] tempAbzeichenArray = new int[6] {9,9,9,9,9,9};
            foreach (var VARIABLE in abzeichenNumber)
            {
                if (VARIABLE == 0)
                {
                    tempAbzeichenArray[0] = 0;
                }else if (VARIABLE == 1)
                {
                    tempAbzeichenArray[1] = 1;
                }else if (VARIABLE == 2)
                {
                    tempAbzeichenArray[2] = 2;
                }else if (VARIABLE == 3)
                {
                    tempAbzeichenArray[3] = 3;
                }else if (VARIABLE == 4)
                {
                    tempAbzeichenArray[4] = 4;
                }else if (VARIABLE == 5)
                {
                    tempAbzeichenArray[5] = 5;
                }
            }
            
            
            foreach (var tempVar in tempAbzeichenArray)
            {
                if (tempVar != 9)
                {
                    _popUpSprites.Add(_spriteList[tempVar]);
                    
                }
                
                if (_abzeichenList.Exists(objects => objects.GetAbzeichenNumber() == tempVar))
                {
                    _abzeichenList.Find(objects => objects.GetAbzeichenNumber() == tempVar).SetAbzeichenCount(1);
                }
                else if (tempVar == 9)
                {
                    // do nothing
                }else
                {
                    AbzeichenObjects abzeichenObjects = new AbzeichenObjects(tempVar,_spriteList[tempVar]);
                    _abzeichenList.Add(abzeichenObjects);
                }
            }
            
            
        }

        public void AbzeichenTest()
        {
            Debug.Log("AbzeichenTest");
        }

        void Awake()
        {
            _inventory = FindFirstObjectByType<Inventory>();
            _abzeichenList = new List<AbzeichenObjects>();
            _popUpSprites = new List<Sprite>();
        }


        // Start is called before the first frame update
        void Start()
        {
            _gameController = FindFirstObjectByType<GameController>();
            //_inventory = FindFirstObjectByType<Inventory>();
            
            _popUpGameobject = _inventory._popUp;
            _popUp = _popUpGameobject.GetComponentInChildren<PopUp>();
            _popUpGameobject.SetActive(false);
            
        }

        public void ShowAbzeichen()
        {
            _inventory.SetInventory(_abzeichenList);
            _inventory.SetPlayerIcon(this.spriteIcon);
            
        }

        public void ShowPopUp()
        {
            if (_popUp == null)
            {
                _popUp = _popUpGameobject.GetComponentInChildren<PopUp>();
                
            }
            
            StartCoroutine(ShowingBadgesPopUp());
        }

        IEnumerator ShowingBadgesPopUp()
        {
            _popUpGameobject.SetActive(true);
            
            _popUp.showImagesforSprites(_popUpSprites);

            emitter.Play();
            //effect.LoadEffect();
            // Plays effect in transform.position
            //EffekseerHandle handle = EffekseerSystem.PlayEffect(effect,transform.position);
            
            
            yield return new WaitForSeconds(4);
            
            _popUpSprites.Clear();
            _popUpGameobject.SetActive(false);
        }
        
        public int GetScore()
        {
            int score = 0;
            foreach (var abzeichen in _abzeichenList)
            {
                score += abzeichen.GetAbzeichenCount();
            }
            
            return score;
        }
        
    }
}

  

