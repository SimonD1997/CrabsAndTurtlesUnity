using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class CardFlipClick : MonoBehaviour
{
    Camera m_Camera;
    private Animator _anim;
    private Collider _collider;
    
    void Awake()
    {
        m_Camera = Camera.main;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _collider = this.gameObject.GetComponent<Collider>();
        _anim = this.gameObject.GetComponent<Animator>();
        Debug.Log ("CLICKED ");
        //this.gameObject.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
 
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {

            Vector3 mousePosition = mouse.position.ReadValue();
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) )
            {
                //funktioniert möglicherweise --> testen !!!
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                if (hit.collider == _collider)
                {
                    _anim.SetTrigger("Klick");
                    //GetComponent<Animator>().enabled = false;
                    //this.gameObject.GetComponent<Animator>().enabled = true;

                    Debug.Log("CLICKED " + this.gameObject.name);
                }
                

                //Rotate the Object --> not smooth because, with code have to wait with 
                //rest of the programm to end the Animation
                //this.gameObject.transform.Rotate(180,0,0);
                
                //animator.SetBool("Klick",true);  
            }
            
        }
        
        
    }
/*
     void OnMouseDown()
    {
        anim.SetTrigger("Klick");
    }*/
}
