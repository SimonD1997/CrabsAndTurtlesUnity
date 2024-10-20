using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Gameboard.Scripts
{
    public class AbzeichenObjects
    {
        private int _abzeichenNumber;
        public Sprite[] _spriteList;
        public List<Sprite> _Sprites;
        private Sprite _sprite;
        private bool _semiTransparent = true;
        
        public int _count = 0;

        public int GetAbzeichenNumber()
        {
            return _abzeichenNumber;
        }

        public int GetAbzeichenCount()
        {
            return _count;
        }

        public void SetAbzeichenCount(int count)
        {
            this._count += count;
        }
    
        public AbzeichenObjects(int spriteNumber, Sprite sprite)
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
