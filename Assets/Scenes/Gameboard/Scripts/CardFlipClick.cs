using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using TouchPhase = UnityEngine.TouchPhase;


public class CardFlipClick : MonoBehaviour
{
    Camera m_Camera;
    private Animator _anim;
    private Collider _collider;
    private bool _down = true;
    
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
/// <summary>
/// Check if the card is clicked, then turn the card
/// and start the riddle or the action of the card
/// </summary>
/// <param name="hit"></param>
    void ScreenHit(RaycastHit hit)
    {
        if (hit.collider == _collider)
        {
            //Karte bleibt umgedreht und soll dann nach dem nächsten Würfelwurf vom stapel genommen werden
            if (_down == true)
            {
                _anim.enabled = true;
                _anim.SetTrigger("Klick");
                _down = false;


                Debug.Log(this.gameObject.GetComponent<RiddleScript>() != null);

                if (this.gameObject.GetComponent<RiddleScript>() != null)
                {
                    Debug.Log("RiddleScript Vorhanden");
                    this.gameObject.GetComponent<RiddleScript>().StartRiddle();
                }

                if (this.gameObject.GetComponent<ActionCard>() != null)
                {
                    Debug.Log("ActionCard Vorhanden");
                    this.gameObject.GetComponent<ActionCard>().StartAction();
                }
            }
            /*
                    else
                    {
                    // TODO: zoom auf die Karte!!
                        _down = true;
                    }*/

            //GetComponent<Animator>().enabled = false;
            //this.gameObject.GetComponent<Animator>().enabled = true;

            Debug.Log("CLICKED " + this.gameObject.name);
            
            
            
            
            //Rotate the Object --> not smooth because, with code have to wait with 
            //rest of the programm to end the Animation
            //this.gameObject.transform.Rotate(180,0,0);
                
            //animator.SetBool("Klick",true);  
        }
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
                ScreenHit(hit);
                //funktioniert möglicherweise --> testen !!!
                /*if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }*/
                
            }
            
        }
        
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                // Construct a ray from the current touch coordinates
                Ray ray = m_Camera.ScreenPointToRay(Input.GetTouch(i).position);
                if (Physics.Raycast(ray, out RaycastHit hit) )
                {
                    ScreenHit(hit);
                
                }
                        
            }
        }
        
        
    }
/*
     void OnMouseDown()
    {
        anim.SetTrigger("Klick");
    }*/
    public bool GetTurnState()
    {
        return _down;
    }

    public void ClickStateActivate()
    {
        
        _anim.SetTrigger("Klick");
        
    }

    

}
