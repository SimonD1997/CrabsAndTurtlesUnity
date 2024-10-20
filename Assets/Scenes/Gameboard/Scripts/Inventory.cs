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
        
        public GridLayoutGroup gridLayoutGroup;
        public RectTransform parentRectTransform;

        private List<AbzeichenObjects> _abzeichenObjectsList;
        
        
    
        // Start is called before the first frame update
        void Start()
        {
            _abzeichenObjectsArray = new AbzeichenObjects[imageSlots.Length];
        }

        public void SetInventory(List<AbzeichenObjects> abzeichenObjectsList)
        {
            this._abzeichenObjectsList = abzeichenObjectsList;
            UpdateInventory();
        
        }

        public void UpdateInventory()
        {
            for (int i = 0; i < _abzeichenObjectsList.Count; i++)
            {
                _abzeichenObjectsArray[i] = _abzeichenObjectsList[i];
                //TODO
                //imageSlots[i].enabled = true;
                //imageSlots[i].sprite = abzeichenObjectsList[i].GetSprite();

                int abzeichennumber = _abzeichenObjectsList[i].GetAbzeichenCount();
                if (abzeichennumber > 1)
                {
                    imageSlots[i].gameObject.GetComponentInChildren<TMP_Text>().text = abzeichennumber.ToString();
                    imageSlots[i].color = Color.white;
                }else if (abzeichennumber == 0)
                {
                    imageSlots[i].color = new Color(0.5f,0.5f,0.5f,0.2f);
                    imageSlots[i].gameObject.GetComponentInChildren<TMP_Text>().text = "";
                }else
                {
                    imageSlots[i].gameObject.GetComponentInChildren<TMP_Text>().text = "";
                    imageSlots[i].color = Color.white;
                }
            
            }
            
        }

        public void SetPlayerIcon(Sprite playerIcon)
        {
            this.playerIcon.sprite = playerIcon;
        }
        
        public void SetGridLayoutGroup()
        {
            float parentWidth = parentRectTransform.rect.width;
            float parentHeight = parentRectTransform.rect.height;
            int columns = gridLayoutGroup.constraintCount;

            float cellWidth = (parentWidth - gridLayoutGroup.padding.left - gridLayoutGroup.padding.right - (columns - 1) * gridLayoutGroup.spacing.x) / columns;
            float cellHeight = (parentHeight - gridLayoutGroup.padding.top - gridLayoutGroup.padding.bottom - (gridLayoutGroup.transform.childCount / columns - 1) * gridLayoutGroup.spacing.y) / (gridLayoutGroup.transform.childCount / columns);

            gridLayoutGroup.cellSize = new Vector2(cellHeight, cellHeight);
        
        }
        
        void Update()
        {
            SetGridLayoutGroup();
        }
    
        
    }
}
