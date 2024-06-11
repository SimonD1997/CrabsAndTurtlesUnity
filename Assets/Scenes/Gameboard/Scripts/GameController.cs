using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    //Todo:
    //Rundenbasierte Spielmechanik einführen 

    private bool gameIsRunning = false;
    private int playerTurn = 0;
    public TextMeshProUGUI yourTurnField ;
    
    
    // Start is called before the first frame update
    void Start()
    {
        this.gameIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsRunning == true)
        {
            if (playerTurn == 0)
            {
                this.yourTurnField.gameObject.SetActive(true);
                this.yourTurnField.text = "DU BIST DRAN!";
            }
            else
            {
                TurnOfOtherPlayers();
            }
        }
        else
        {
            //todo:
            //run endscreen with scoreboard
        }
    }


    void TurnOfOtherPlayers()
    {
        //todo:
        // warten falls andere Spieler analog anwesend sind oder
        // einen virtuellen Zug machen falls gegen den Computer gekämpft wird
        
        
        
        
        
    }
}
