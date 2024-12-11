using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Gameboard.Scripts
{
    public class BadgeObjects
    {
        private int _badgeNumber;
        public Sprite[] SpriteList;
        public List<Sprite> Sprites;
        private Sprite _sprite;
        private bool _semiTransparent = true;
        
        public int Count = 0;

        
        /// <summary>
        ///  Returns the badge number
        /// </summary>
        /// <returns> Badge number</returns>
        public int GetBadgeNumber()
        {
            return _badgeNumber;
        }

        /// <summary>
        ///  Returns the badge count
        /// </summary>
        /// <returns> Badge count</returns>
        public int GetBadgeCount()
        {
            return Count;
        }

        /// <summary>
        ///  Sets the badge count
        /// </summary>
        /// <param name="count"> The count to set</param>
        public void SetBadgeCount(int count)
        {
            this.Count += count;
        }
    
        /// <summary>
        ///  Constructor for the BadgeObjects class
        /// </summary>
        /// <param name="spriteNumber"> The sprite number</param>
        /// <param name="sprite"> The sprite</param>
        public BadgeObjects(int spriteNumber, Sprite sprite)
        {
            this._sprite = sprite;
            this._badgeNumber = spriteNumber;
        }

        /// <summary>
        ///  Returns the sprite of the badge
        /// </summary>
        /// <returns> The sprite of the badge</returns>
        public Sprite GetSprite()
        {
            return _sprite;
        }

        /// <summary>
        ///  Sets the sprite of the badge
        /// </summary>
        /// <param name="transparent"> The sprite to set</param>
        /// <returns> The sprite of the badge</returns>
        public bool GetSetTransparent(bool transparent)
        {
            _semiTransparent = transparent;
            return _semiTransparent;
        }


    
    }
}
