using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiceText : MonoBehaviour
{
    public TMP_Text diceText;
    // Start is called before the first frame update
    void Start()
    {
        diceText = this.gameObject.GetComponent<TMP_Text>();
        Dice();
        Debug.Log("DiceText");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
    public void Dice()
    {
       var diceRoll = Random.Range(1, 6);
       diceText.text = diceRoll.ToString(); 
       Debug.Log(diceRoll);
    }
}
