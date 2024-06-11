using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClickScript : MonoBehaviour
{
    public MonoScript scriptToRun;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MouseClick()
    {
        if (this.gameObject.name.StartsWith("Card"))
        {
            
        }
    }
}
