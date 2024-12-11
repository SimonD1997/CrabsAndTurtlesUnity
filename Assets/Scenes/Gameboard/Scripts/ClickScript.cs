using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// not used
/// </summary>
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
                Debug.Log(hit.collider.name);
                
                 
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
