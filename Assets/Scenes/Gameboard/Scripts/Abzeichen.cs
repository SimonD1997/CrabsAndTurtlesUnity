using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Gameboard.Scripts
{
    public class Abzeichen : MonoBehaviour
    {
        public List<AbzeichenObjects> _abzeichenList;
        public Sprite[] _spriteList;
        private Inventory _inventory;
        public Sprite _spriteIcon;
        
        
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
    
        // Start is called before the first frame update
        void Start()
        {
            _abzeichenList = new List<AbzeichenObjects>();
            _inventory = FindObjectOfType<Inventory>();


        }

        public void showAbzeichen()
        {
            _inventory.SetInventory(_abzeichenList);
            _inventory.SetPlayerIcon(_spriteIcon);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

  

