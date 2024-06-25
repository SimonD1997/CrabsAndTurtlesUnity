using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickScript : MonoBehaviour
{
    
    Camera m_Camera;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePosition = mouse.position.ReadValue();
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            { 
                if (hit.collider == this.gameObject.GetComponent<Collider>())
                {
                    
                    
              
                    Debug.Log ("CLICKED " + this.gameObject.name);
                }
                
                 
            }
            
        }
    }

    public void MouseClick()
    {
        if (this.gameObject.name.StartsWith("Card"))
        {
            
        }
    }
}
