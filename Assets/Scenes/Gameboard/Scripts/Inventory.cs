using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Gameboard.Scripts
{
    public class Inventory : MonoBehaviour
    {
        public Image[] imageSlots;
        private AbzeichenObjects[] _abzeichenObjectsArray;
        public Image playerIcon;
        public GameObject _popUp;
    
        // Start is called before the first frame update
        void Start()
        {
            _abzeichenObjectsArray = new AbzeichenObjects[imageSlots.Length];
        }

        public void SetInventory(List<AbzeichenObjects> abzeichenObjectsList)
        {
            for (int i = 0; i < abzeichenObjectsList.Count; i++)
            {
                _abzeichenObjectsArray[i] = abzeichenObjectsList[i];
                imageSlots[i].enabled = true;
                imageSlots[i].sprite = abzeichenObjectsList[i].GetSprite();

                int abzeichennumber = abzeichenObjectsList[i].GetAbzeichenCount();
                if (abzeichennumber > 1)
                {
                    imageSlots[i].gameObject.GetComponentInChildren<TMP_Text>().text = abzeichennumber.ToString();
                }
                else
                {
                    imageSlots[i].gameObject.GetComponentInChildren<TMP_Text>().text = "";
                }
            
            }

            for (int i = abzeichenObjectsList.Count; i < (imageSlots.Length - (abzeichenObjectsList.Count)); i++)
            {
                imageSlots[i].enabled = false;
            }
        
        }

        public void SetPlayerIcon(Sprite playerIcon)
        {
            this.playerIcon.sprite = playerIcon;
        }
    
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
