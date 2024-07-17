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
        public GameObject logTextField;
        
        
        // Start is called before the first frame update
        void Start()
        {
            _gruenVarList = new List<int>();
            _rotVarList = new List<int>();
            _blauVarList = new List<int>();
            _gelbVarList = new List<int>();

            //_gruen[0] = 

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        // gibt nach jedem Input alle Variablen aller Spalten in neu in die Liste ein um 
        // um ein nachvollziehbaren log zu erhalten...
        
        public void Gruen()
        {
            _aktuelleVarGruen = Convert.ToInt32(gruenGameObject.GetComponentInChildren<TMP_InputField>().text);
            addToList();
            gruenGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarGruen.ToString();
            //eingabe erfolgreich und an den Gamecontroller zurückgeben...
        }
        
        public void Rot()
        {
            _rotVarList.Add(Convert.ToInt32(rotGameObject.GetComponentInChildren<TextMeshPro>().text));
            addToList();
            rotGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarRot.ToString();
        }
        
        public void Blau()
        {
            _blauVarList.Add(Convert.ToInt32(blauGameObject.GetComponentInChildren<TextMeshPro>().text));
            addToList();
            blauGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarBlau.ToString();
        }
        
        public void Gelb()
        {
            _gelbVarList.Add(Convert.ToInt32(gelbGameObject.GetComponentInChildren<TMP_InputField>().text));
            addToList();
            gelbGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarGelb.ToString();
        }

        void addToList()
        {
            _gruenVarList.Add(_aktuelleVarGruen);
            _rotVarList.Add(_aktuelleVarRot);
            _blauVarList.Add(_aktuelleVarBlau);
            _gelbVarList.Add(_aktuelleVarGelb);
            
        }

        /// <summary>
        /// Updates the Textfields and Variable Cache with the stored Data
        /// </summary>
        public void onShow()
        {
            String stringtemp = "";
            TextMeshProUGUI logTemp = logTextField.GetComponent<TextMeshProUGUI>();
            for (int i = 0; i < _gruenVarList.Count; i++)
            {
                logTemp.text.Insert(logTemp.text.Length, _aktuelleVarGruen.ToString());
                stringtemp = stringtemp +  _gruenVarList[i].ToString() + "\t" +_rotVarList[i].ToString() +
                "\t" + _rotVarList[i].ToString() + "\t" + _gelbVarList[i].ToString() + System.Environment.NewLine;
                
            }
            
            
            logTemp.text = stringtemp;
            
            

        }
        
        
    }
}
