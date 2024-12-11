using System;
using System.Collections;
using System.Collections.Generic;
using Effekseer;
using UnityEngine;

namespace Scenes.Gameboard.Scripts
{
    public class Badges : MonoBehaviour
    {
        private GameController _gameController;

        private List<BadgeObjects> _badgeList;
        public Sprite[] _spriteList;
        private Inventory _inventory;
        public Sprite spriteIcon;
        private PopUp _popUp;
        private GameObject _popUpGameobject;
        private List<Sprite> _popUpSprites;

        //Effekts for the badge pop up
        public EffekseerEffectAsset effect;
        public EffekseerEmitter emitter;

        void Awake()
        {
            _inventory = FindFirstObjectByType<Inventory>();
            _badgeList = new List<BadgeObjects>();
            _popUpSprites = new List<Sprite>();
        }
        
        // Start is called before the first frame update
        void Start()
        {
            _gameController = FindFirstObjectByType<GameController>();

            _popUpGameobject = _inventory._popUp;
            _popUp = _popUpGameobject.GetComponentInChildren<PopUp>();
            _popUpGameobject.SetActive(false);
            InitInventory();
        }
        
        /// <summary>
        /// Add Badges to the inventory
        /// Badge Number
        /// 0 = +
        /// 1 = *
        /// 2 = -
        /// 3 = Bedingung
        /// 4 = Variable
        /// 5 = Konstante
        /// </summary>
        /// <param name="badgeNumber"> List of badges named with numbers to add to the inventory </param>
        public void AddBadges(List<int> badgeNumber)
        {
            int[] tempAbzeichenArray = new int[6] { 9, 9, 9, 9, 9, 9 };
            foreach (var VARIABLE in badgeNumber)
            {
                if (VARIABLE == 0)
                {
                    tempAbzeichenArray[0] = 0;
                }
                else if (VARIABLE == 1)
                {
                    tempAbzeichenArray[1] = 1;
                }
                else if (VARIABLE == 2)
                {
                    tempAbzeichenArray[2] = 2;
                }
                else if (VARIABLE == 3)
                {
                    tempAbzeichenArray[3] = 3;
                }
                else if (VARIABLE == 4)
                {
                    tempAbzeichenArray[4] = 4;
                }
                else if (VARIABLE == 5)
                {
                    tempAbzeichenArray[5] = 5;
                }
            }


            foreach (var tempVar in tempAbzeichenArray)
            {
                if (tempVar != 9)
                {
                    _popUpSprites.Add(_spriteList[tempVar]);

                    var badge = _badgeList[tempVar];
                    if (badge.GetBadgeCount() == 0)
                    {
                        badge.GetSetTransparent(false);
                        badge.SetBadgeCount(1);
                    }
                    else if (badge.GetBadgeCount() > 0)
                    {
                        badge.SetBadgeCount(1);
                    }
                }
            }
        }

        
        /// <summary>
        /// init the inventory with 6 badges and set them to transparent for the start of the game
        /// </summary>
        void InitInventory()
        {
            for (int i = 0; i < 6; i++)
            {
                BadgeObjects badgeObjects = new BadgeObjects(i, _spriteList[i]);
                badgeObjects.GetSetTransparent(true);
                _badgeList.Add(badgeObjects);
                ShowBadge();
            }
        }
        
        /// <summary>
        /// refresh the inventory in the UI
        /// </summary>
        public void ShowBadge()
        {
            _inventory.SetInventory(_badgeList);
            _inventory.SetPlayerIcon(this.spriteIcon);
        }

        /// <summary>
        /// show the pop up with the badges that are added to the inventory and the player gets in this round
        /// 
        /// </summary>
        public void ShowPopUp()
        {
            if (_popUp == null)
            {
                _popUp = _popUpGameobject.GetComponentInChildren<PopUp>();
            }

            StartCoroutine(ShowingBadgesPopUp());
        }

        /// <summary>
        /// Coroutine for showing the pop up with the badges with sound and effect
        /// The pop up is shown for 3 seconds
        /// </summary>
        /// <returns></returns>
        IEnumerator ShowingBadgesPopUp()
        {
            _popUpGameobject.SetActive(true);

            _popUp.ShowImagesforSprites(_popUpSprites);

            emitter.Play();
            //effect.LoadEffect();
            // Plays effect in transform.position
            //EffekseerHandle handle = EffekseerSystem.PlayEffect(effect,transform.position);


            yield return new WaitForSeconds(3);

            _popUpSprites.Clear();
            _popUpGameobject.SetActive(false);
        }
        
        /// <summary>
        /// Stop showing the pop up
        /// </summary>
        internal void HidePopUp()
        {
            StopCoroutine(ShowingBadgesPopUp());
            _popUpSprites.Clear();
            _popUpGameobject.SetActive(false);
        }

        /// <summary>
        /// calculate the score of the player and return it
        /// </summary>
        /// <returns>score of the player</returns>
        public int GetScore()
        {
            int score = 0;
            foreach (var abzeichen in _badgeList)
            {
                score += abzeichen.GetBadgeCount();
            }

            return score;
        }

        /// <summary>
        /// Return the badge list of this player
        /// </summary>
        /// <returns>all badges of the player</returns>
        public List<BadgeObjects> GetBadgeList()
        {
            return  _badgeList;        }
    }
}