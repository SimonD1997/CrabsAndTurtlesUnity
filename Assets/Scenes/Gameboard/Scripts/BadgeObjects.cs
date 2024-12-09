using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Gameboard.Scripts
{
    public class BadgeObjects
    {
        private int _abzeichenNumber;
        public Sprite[] _spriteList;
        public List<Sprite> _Sprites;
        private Sprite _sprite;
        private bool _semiTransparent = true;
        
        public int _count = 0;

        public int GetBadgeNumber()
        {
            return _abzeichenNumber;
        }

        public int GetBadgeCount()
        {
            return _count;
        }

        public void SetBadgeCount(int count)
        {
            this._count += count;
        }
    
        public BadgeObjects(int spriteNumber, Sprite sprite)
        {
            this._sprite = sprite;
            this._abzeichenNumber = spriteNumber;
        }

        public Sprite GetSprite()
        {
            return _sprite;
        }

        public bool GetSetTransparent(bool transparent)
        {
            _semiTransparent = transparent;
            return _semiTransparent;
        }


    
    }
}
