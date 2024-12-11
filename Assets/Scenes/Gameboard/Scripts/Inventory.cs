using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Gameboard.Scripts
{
    public class Inventory : MonoBehaviour
    {
        public Image[] imageSlots;
        private BadgeObjects[] _badgesObjectsArray;
        public Image playerIcon;
        public GameObject _popUp;
        
        public GridLayoutGroup gridLayoutGroup;
        public RectTransform parentRectTransform;

        private List<BadgeObjects> _badgesObjectsList;
    
        // Start is called before the first frame update
        void Start()
        {
            _badgesObjectsArray = new BadgeObjects[imageSlots.Length];
        }

        
        /// <summary>
        ///  Set the inventory of the player with the given list of BadgeObjects
        /// </summary>
        /// <param name="badgeObjectsList"> List of BadgeObjects</param>
        public void SetInventory(List<BadgeObjects> badgeObjectsList)
        {
            this._badgesObjectsList = badgeObjectsList;
            UpdateInventory();
        
        }

        /// <summary>
        /// Update the inventory of the player with the given list of BadgeObjects
        /// Depending on the number of badges the player has, the number of the badge will be displayed on the badge
        /// </summary>
        public void UpdateInventory()
        {
            for (int i = 0; i < _badgesObjectsList.Count; i++)
            {
                _badgesObjectsArray[i] = _badgesObjectsList[i];

                int abzeichennumber = _badgesObjectsList[i].GetBadgeCount();
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

        /// <summary>
        ///  Set the player icon of the inventory
        /// </summary>
        /// <param name="playerIcon"> Sprite of the player icon</param>
        public void SetPlayerIcon(Sprite playerIcon)
        {
            this.playerIcon.sprite = playerIcon;
        }
        
        /// <summary>
        /// Set the GridLayoutGroup of the inventory
        /// Not implemented yet, but should be used if the inventory changes in size
        /// </summary>
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
