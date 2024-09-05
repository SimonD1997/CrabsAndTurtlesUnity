using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes.Gameboard.Scripts
{
    
    public class VariablenTafel : MonoBehaviour
    {
        int _aktuelleVarGruen = 1;
        int _aktuelleVarRot = 1;
        int _aktuelleVarBlau = 1;
        int _aktuelleVarGelb = 1;
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
            _aktuelleVarRot = Convert.ToInt32(rotGameObject.GetComponentInChildren<TMP_InputField>().text);
            addToList();
            rotGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarRot.ToString();
        }
        
        public void Blau()
        {
            _aktuelleVarBlau = Convert.ToInt32(blauGameObject.GetComponentInChildren<TMP_InputField>().text);
            addToList();
            blauGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarBlau.ToString();
        }
        
        public void Gelb()
        {
            _aktuelleVarGelb = Convert.ToInt32(gelbGameObject.GetComponentInChildren<TMP_InputField>().text);
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
                "\t" + _blauVarList[i].ToString() + "\t" + _gelbVarList[i].ToString() + System.Environment.NewLine;
                
            }
            
            logTemp.text = stringtemp;

        }

        
        public int GetVar(int colour)
        {
            int colourVar = 0;
            switch (colour)
            {
                case 1: //red
                    colourVar = _aktuelleVarRot;
                    break;
                case 2: //blue
                    colourVar = _aktuelleVarBlau;
                    break;
                case 3: //gruen
                    colourVar = _aktuelleVarGruen;
                    break;
                case 4: //gelb
                    colourVar = _aktuelleVarGelb;
                    break;
            }

            return colourVar;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="colour"></param>
        /// <param name="colourVar"></param>
        public void SetVar(int colour, int colourVar)
        {
            
            switch (colour)
            {
                case 1: //red
                    _aktuelleVarRot= colourVar;
                    addToList();
                    rotGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarRot.ToString();
                    break;
                case 2: //blue
                    _aktuelleVarBlau = colourVar;
                    addToList();
                    blauGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarBlau.ToString();
                    break;
                case 3: //gruen
                    _aktuelleVarGruen = colourVar;
                    addToList();
                    gruenGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarGruen.ToString();
                    break;
                case 4: //gelb
                    _aktuelleVarGelb = colourVar;
                    addToList();
                    gelbGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarGelb.ToString();
                    break;
            }

            
        }

    }
}
