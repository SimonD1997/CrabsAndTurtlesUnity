using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Diece : MonoBehaviour
{
    
    private Camera cam;
    private Vector3 initPos;
    private object initXpose;
    private Rigidbody rb;
   
    
    void Awake()
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown (0))
        {
//initial click to roll a dice
            initPos = Input.mousePosition;
 
//return x component of dice from screen to view point
            initXpose = cam.ScreenToViewportPoint (Input.mousePosition).x;
        }
 
//current position of mouse
        Vector3 currentPos = Input.mousePosition;
 
//get all position along with mouse pointer movement
        Vector3 newPos = cam.ScreenToWorldPoint (new Vector3(currentPos.x,currentPos.y,Mathf.Clamp(currentPos.y/10,10,50)));
 
//translate from screen to world coordinates  
        newPos = cam.ScreenToWorldPoint (currentPos);
 
        if (Input.GetMouseButtonUp (0))
        {
            initPos = cam.ScreenToWorldPoint (initPos);
 
//Method use to roll the dice
            RollTheDice(newPos);
//use identify face value on dice
            //StartCoroutine(GetDiceCount ());
        }
 
//Method Roll the Dice
        void RollTheDice(Vector3 lastPos)
        {
            rb.AddTorque(Vector3.Cross(lastPos, initPos) * 1000, ForceMode.Impulse);
            lastPos.y += 12;
            rb.AddForce (((lastPos - initPos).normalized) * ((Vector3.Distance (lastPos, initPos)) * 25 * rb.mass));
        } 
    }
    
    
    
}
