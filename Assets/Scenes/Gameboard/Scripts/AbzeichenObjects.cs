using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Gameboard.Scripts
{
    public class AbzeichenObjects : ScriptableObject
    {
        private int _abzeichenNumber;
        public Sprite[] _spriteList;
        public List<Sprite> _Sprites;
        private Sprite _sprite;
        public int _count = 1;

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


    
    }
}
