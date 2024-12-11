using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scenes.Gameboard.Scripts
{
    
    public class VariableField : MonoBehaviour
    {
        private int _currentVarGreen = 1;
        private int _currentVarRed = 1;
        private int _currentVarBlue = 1;
        private int _currentVarYellow = 1;
        
        private List<int> _greenVarList;
        private List<int> _redVarList;
        private List<int> _blueVarList;
        private List<int> _yellowVarList;
        
        public GameObject greenGameObject;
        public GameObject redGameObject;
        public GameObject blueGameObject;
        public GameObject yellowGameObject;
        public GameObject logTextField;
        //public Animator button;

        private TMP_InputField _greenTextField;
        private TMP_InputField _redTextField;
        private TMP_InputField _yellowTextField;
        private TMP_InputField _blueTextField;

        private TextMeshProUGUI _greenPlaceholder;
        private TextMeshProUGUI _redPlaceholder;
        private TextMeshProUGUI _yellowPlaceholder;
        private TextMeshProUGUI _bluePlaceholder;

        private TextMeshProUGUI[] _greenTextFieldList;
        private TextMeshProUGUI[] _redTextFieldList;
        private TextMeshProUGUI[] _yellowTextFieldList;
        private TextMeshProUGUI[] _blueTextFieldList;
        
        
        
        // Start is called before the first frame update
        void Start()
        {
            _greenVarList = new List<int>();
            _redVarList = new List<int>();
            _blueVarList = new List<int>();
            _yellowVarList = new List<int>();
            
            AddToList();

            //_gruen[0] = 
        }

        private void Awake()
        {
            _greenTextField = greenGameObject.GetComponentInChildren<TMP_InputField>();
            _redTextField = redGameObject.GetComponentInChildren<TMP_InputField>();
            _yellowTextField = yellowGameObject.GetComponentInChildren<TMP_InputField>();
            _blueTextField = blueGameObject.GetComponentInChildren<TMP_InputField>();

            _greenPlaceholder = _greenTextField.placeholder.gameObject.GetComponent<TextMeshProUGUI>();
            _redPlaceholder = _redTextField.placeholder.gameObject.GetComponent<TextMeshProUGUI>();
            _yellowPlaceholder = _yellowTextField.placeholder.gameObject.GetComponent<TextMeshProUGUI>();
            _bluePlaceholder = _blueTextField.placeholder.gameObject.GetComponent<TextMeshProUGUI>();

            _greenTextFieldList = greenGameObject.GetComponentsInChildren<TextMeshProUGUI>();
            _redTextFieldList = redGameObject.GetComponentsInChildren<TextMeshProUGUI>();
            _yellowTextFieldList = yellowGameObject.GetComponentsInChildren<TextMeshProUGUI>();
            _blueTextFieldList = blueGameObject.GetComponentsInChildren<TextMeshProUGUI>();
        }

        /// <summary>
        ///  Switches the Inputfields to active or inactive
        /// </summary>
        /// <param name="inputFieldsActive"> true = active, false = inactive</param>
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

        /// <summary>
        ///  Switches the Gameobject to active or inactive
        /// </summary>
        /// <param name="active"> true = active, false = inactive</param>
        public void SwitchGameobjectState(bool active)
        {
            if (active)
            {
                //button.SetBool("Active",true);
                this.gameObject.SetActive(true);
            }
            else
            {
                //button.SetBool("Active",false);
                this.gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        ///  Switches the Gameobject to active or inactive depending on the current state
        /// </summary>
        public void SwitchGameobjectState()
        {
            if (this.gameObject.activeSelf)
            {
                //button.SetBool("Active",false);
                this.gameObject.SetActive(false);
            }
            else
            {
                //button.SetBool("Active",true);
                this.gameObject.SetActive(true);
            }
        }
        
        /// <summary>
        ///  Used to get the variables of the clicked colour inputfield
        /// </summary>
        public void Green()
        {
            string input = _greenTextField.text;
            if (string.IsNullOrEmpty(input))
            {
                Debug.LogError("Input for Gruen is empty!");
                return;
            }
            
            _currentVarGreen = Convert.ToInt32(input);
            _greenPlaceholder.text = input;

        }

        /// <summary>
        ///  Used to get the variables of the clicked colour inputfield
        /// </summary>
        public void Red()
        {
            string input = _redTextField.text;
            if (string.IsNullOrEmpty(input))
            {
                Debug.LogError("Input for Rot is empty!");
                return;
            }

            _currentVarRed = Convert.ToInt32(input);
            _redPlaceholder.text = input;
            //addToList();
            //rotGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarRot.ToString();
        }

        /// <summary>
        ///  Used to get the variables of the clicked colour inputfield
        /// </summary>
        public void Blue()
        {
            string input = _blueTextField.text;
            if (string.IsNullOrEmpty(input))
            {
                Debug.LogError("Input for Blau is empty!");
                return;
            }

            _currentVarBlue = Convert.ToInt32(input);
            _bluePlaceholder.text = input;
            //addToList();
            //blauGameObject.GetComponentInChildren<TextMeshProUGUI>().text = _aktuelleVarBlau.ToString();
        }

        /// <summary>
        ///  Used to get the variables of the clicked colour inputfield
        /// </summary>
        public void Yellow()
        {
            string input =_yellowTextField.text;
            if (string.IsNullOrEmpty(input))
            {
                Debug.LogError("Input for Gelb is empty!");
                return;
            }

            _currentVarYellow = Convert.ToInt32(input);
            _yellowPlaceholder.text = input;
        }

        /// <summary>
        ///  Adds the current Variables to the List
        /// </summary>
        void AddToList()
        {
            _greenVarList.Add(_currentVarGreen);
            _redVarList.Add(_currentVarRed);
            _blueVarList.Add(_currentVarBlue);
            _yellowVarList.Add(_currentVarYellow);
            
        }

        /// <summary>
        /// Updates the Textfields and Variable Cache with the stored Data
        /// </summary>
        public void OnShow()
        {
            String stringtemp = "";
            TextMeshProUGUI logTemp = logTextField.GetComponent<TextMeshProUGUI>();
            for (int i = 0; i < _greenVarList.Count; i++)
            {
                logTemp.text.Insert(logTemp.text.Length, _currentVarGreen.ToString());
                stringtemp = stringtemp +  _greenVarList[i].ToString() + "\t" +_redVarList[i].ToString() +
                "\t" + _blueVarList[i].ToString() + "\t" + _yellowVarList[i].ToString() + System.Environment.NewLine;
                
            }
            
            logTemp.text = stringtemp;

        }

        /// <summary>
        ///  Returns the current Variable of the given Colour
        /// </summary>
        /// <param name="colour">1 = red, 2 = blue, 3 = green, 4 = yellow</param>
        /// <returns> Current Variable of the given Colour</returns>
        public int GetVar(int colour)
        {
            int colourVar = 0;
            switch (colour)
            {
                case 1: //red
                    colourVar = _currentVarRed;
                    break;
                case 2: //blue
                    colourVar = _currentVarBlue;
                    break;
                case 3: //gruen
                    colourVar = _currentVarGreen;
                    break;
                case 4: //gelb
                    colourVar = _currentVarYellow;
                    break;
            }

            return colourVar;
        }
        
        /// <summary>
        /// move all variables of the second textfield to the first textfield 
        /// </summary>
        void MoveVariablesUp()
        {
            _greenTextFieldList[0].text = _greenTextFieldList[1].text;
            _redTextFieldList[0].text = _redTextFieldList[1].text;
            _blueTextFieldList[0].text = _blueTextFieldList[1].text;
            _yellowTextFieldList[0].text = _yellowTextFieldList[1].text;
        }
        
        /// <summary>
        ///  Sets the current Variable of the given Colour
        /// </summary>
        /// <param name="colour"> 1 = red, 2 = blue, 3 = green, 4 = yellow</param>
        /// <param name="colourVar"> Variable to set</param>
        public void SetVar(int colour, int colourVar)
        {
            MoveVariablesUp();
            switch (colour)
            {
                case 1: //red
                    _currentVarRed= colourVar;
                    AddToList();
                    _redTextFieldList[1].text = _currentVarRed.ToString();
                    SwitchInputFields(false);
                    break;
                case 2: //blue
                    _currentVarBlue = colourVar;
                    AddToList();
                    _blueTextFieldList[1].text = _currentVarBlue.ToString();
                    SwitchInputFields(false);
                    break;
                case 3: //gruen
                    _currentVarGreen = colourVar;
                    AddToList();
                    _greenTextFieldList[1].text = _currentVarGreen.ToString();
                    SwitchInputFields(false);
                    break;
                case 4: //gelb
                    _currentVarYellow = colourVar;
                    AddToList();
                    _yellowTextFieldList[1].text = _currentVarYellow.ToString();
                    SwitchInputFields(false);
                    break;
            }

            
        }

    }
}
