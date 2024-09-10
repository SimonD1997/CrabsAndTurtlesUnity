using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Gameboard.Scripts
{
    public class Abzeichen : MonoBehaviour
    {
        public List<AbzeichenObjects> _abzeichenList;
        public Sprite[] _spriteList;
        private Inventory _inventory;
        
        
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
            foreach (var VARIABLE in abzeichenNumber)
            {
                if (_abzeichenList.Exists(objects => objects.GetAbzeichenNumber() == VARIABLE))
                {
                    _abzeichenList.Find(objects => objects.GetAbzeichenNumber() == VARIABLE).SetAbzeichenCount(1);
                }
                else
                {
                    AbzeichenObjects abzeichenObjects = new AbzeichenObjects(VARIABLE,_spriteList[VARIABLE]);
                    _abzeichenList.Add(abzeichenObjects);
                }
                
                
            }
        
            
        
        
        }

        public void AbzeichenTest()
        {
            Debug.Log("AbzeichenTest");
        }
    
        // Start is called before the first frame update
        void Start()
        {
            _abzeichenList = new List<AbzeichenObjects>();
            _inventory = FindObjectOfType<Inventory>();


        }

        public void showAbzeichen()
        {
            _inventory.SetInventory(_abzeichenList);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

  

