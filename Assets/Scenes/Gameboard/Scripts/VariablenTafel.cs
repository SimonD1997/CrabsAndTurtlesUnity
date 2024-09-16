using System;
using System.Collections.Generic;
using System.Net.Mime;
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
        public Animator button;

        private TMP_InputField _greenTextField;
        private TMP_InputField _redTextField;
        private TMP_InputField _yellowTextField;
        private TMP_InputField _blueTextField;

        private TextMeshProUGUI _greenPlaceholder;
        private TextMeshProUGUI _redPlaceholder;
        private TextMeshProUGUI _yellowPlaceholder;
        private TextMeshProUGUI _bluePlaceholder;
        
        
        
        // Start is called before the first frame update
        void Start()
        {
            _gruenVarList = new List<int>();
            _rotVarList = new List<int>();
            _blauVarList = new List<int>();
            _gelbVarList = new List<int>();

            //_gruen[0] = 
        }

        private void Awake()
        {
            _greenTextField = gruenGameObject.GetComponentInChildren<TMP_InputField>();
            _redTextField = rotGameObject.GetComponentInChildren<TMP_InputField>();
            _yellowTextField = gelbGameObject.GetComponentInChildren<TMP_InputField>();
            _blueTextField = blauGameObject.GetComponentInChildren<TMP_InputField>();

            _greenPlaceholder = _greenTextField.placeholder.gameObject.GetComponent<TextMeshProUGUI>();
            _redPlaceholder = _redTextField.placeholder.gameObject.GetComponent<TextMeshProUGUI>();
            _yellowPlaceholder = _yellowTextField.placeholder.gameObject.GetComponent<TextMeshProUGUI>();
            _bluePlaceholder = _blueTextField.placeholder.gameObject.GetComponent<TextMeshProUGUI>();
        }

        public void SwitchInputFields(bool inputFieldsActive)
        {
            if (inputFieldsActive)
            {
                _greenTextField.interactable = true;
                _redTextField.interactable = true;
                _blueTextField.interactable = true;
                _yellowTextField.interactable = true;

                _greenPlaceholder.text = "Eingabefeld";
                _redPlaceholder.text = "Eingabefeld";
                _bluePlaceholder.text = "Eingabefeld";
                _yellowPlaceholder.text = "Eingabefeld";
            }
            else
            {
                _greenTextField.interactable = false;
                _redTextField.interactable = false;
                _blueTextField.interactable = false;
                _yellowTextField.interactable = false;
                
                _greenPlaceholder.text = "";
                _redPlaceholder.text = "";
                _bluePlaceholder.text = "";
                _yellowPlaceholder.text = "";
            }
        }

        public void SwitchGameobjectState(bool active)
        {
            if (active)
            {
                button.SetBool("Active",true);
                this.gameObject.SetActive(true);
            }
            else
            {
                button.SetBool("Active",false);
                this.gameObject.SetActive(false);
            }
        }
        
        public void SwitchGameobjectState()
        {
            if (this.gameObject.activeSelf)
            {
                button.SetBool("Active",false);
                this.gameObject.SetActive(false);
            }
            else
            {
                button.SetBool("Active",true);
                this.gameObject.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        // gibt nach jedem Input alle Variablen aller Spalten in neu in die Liste ein um 
        // um ein nachvollziehbaren log zu erhalten...
        
        public void Gruen()
        {
            string input = gruenGameObject.GetComponentInChildren<TMP_InputField>().text;
            if (string.IsNullOrEmpty(input))
            {
                Debug.LogError("Input for Gruen is empty!");
                return;
            }

            _aktuelleVarGruen = Convert.ToInt32(input);
            addToList();
            gruenGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarGruen.ToString();
        }

        public void Rot()
        {
            string input = rotGameObject.GetComponentInChildren<TMP_InputField>().text;
            if (string.IsNullOrEmpty(input))
            {
                Debug.LogError("Input for Rot is empty!");
                return;
            }

            _aktuelleVarRot = Convert.ToInt32(input);
            addToList();
            rotGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarRot.ToString();
        }

        public void Blau()
        {
            string input = blauGameObject.GetComponentInChildren<TMP_InputField>().text;
            if (string.IsNullOrEmpty(input))
            {
                Debug.LogError("Input for Blau is empty!");
                return;
            }

            _aktuelleVarBlau = Convert.ToInt32(input);
            addToList();
            blauGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarBlau.ToString();
        }

        public void Gelb()
        {
            string input = gelbGameObject.GetComponentInChildren<TMP_InputField>().text;
            if (string.IsNullOrEmpty(input))
            {
                Debug.LogError("Input for Gelb is empty!");
                return;
            }

            _aktuelleVarGelb = Convert.ToInt32(input);
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
