using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes.Gameboard.Scripts
{
    
    public class VariablenTafel : MonoBehaviour
    {
        int _aktuelleVarGruen;
        int _aktuelleVarRot;
        int _aktuelleVarBlau;
        int _aktuelleVarGelb;
        private List<int> _gruenVarList;
        private List<int> _rotVarList;
        private List<int> _blauVarList;
        private List<int> _gelbVarList;
        public GameObject gruenGameObject;
        public GameObject rotGameObject;
        public GameObject blauGameObject;
        public GameObject gelbGameObject;
        
        
        
        // Start is called before the first frame update
        void Start()
        {
            _gruenVarList = new List<int>();
            
            //_gruen[0] = 

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Gruen()
        {
            _aktuelleVarGruen = Convert.ToInt32(gruenGameObject.GetComponentInChildren<TMP_InputField>().text);
            _gruenVarList.Add(_aktuelleVarGruen);
        }
        
        public void Rot()
        {
            _rotVarList.Add(Convert.ToInt32(rotGameObject.GetComponentInChildren<TextMeshPro>().text));
        }
        
        public void Blau()
        {
            _blauVarList.Add(Convert.ToInt32(blauGameObject.GetComponentInChildren<TextMeshPro>().text));
        }
        
        public void Gelb()
        {
            _gelbVarList.Add(Convert.ToInt32(gelbGameObject.GetComponentInChildren<TMP_InputField>().text));
            
        }

        /// <summary>
        /// Updates the Textfields and Variable Cache with the stored Data
        /// </summary>
        public void onShow()
        {
            gruenGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarGruen.ToString();
        }
        
        
    }
}
