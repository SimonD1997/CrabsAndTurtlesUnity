using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using TMPro;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public TMP_InputField input;
    private SplineUser _splineUser;
    private Dreamteck.Splines.SplineFollower _splineFollower;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _splineUser = GetComponent<SplineUser>();
        _splineFollower = GetComponent<Dreamteck.Splines.SplineFollower>();
        //setzt die Anfangs und Endanimationspunkte fest.
        _splineUser.SetClipRange(0, 0.5);
        
        //setzt die Richtungsparameter bzw. lässt den start manuell auswählen
        _splineFollower.autoStartPosition = false;
        _splineFollower.direction = Spline.Direction.Backward;
        //
        //legt den Startpunkt durch den Abstand vom Anfang des Splines fest in Prozent 
        //deshalb möglichkeit des von der Streckenlänge in Prozent umzurechnen
        float splineLength = _splineFollower.spline.CalculateLength();
        _splineFollower.SetDistance(splineLength,false,false);
        
        //TODO noch Möglichkeit suchen den Abstand der Knoten vom Startpunkt auszulesen...!!!
        


    }

    // Update is called once per frame
    void Update()
    {
       
    }

/// <summary>
/// Lässt das Objekt an einem gegeben Spline entlanglaufen.
/// </summary>
/// <param name="steps"> legt fest wie viele Blöcke nach vorne oder zurückgelaufen werden messen</param>
    void Movement(float steps)
    {
        // Array für distanzen der einzelnen Felder
        _splineUser.SetClipRange(0.1, 0.5);

        if (steps > 0)
        {
            
            _splineFollower.direction = Spline.Direction.Forward;
            
        }else if (steps < 0)
        {
            _splineFollower.direction = Spline.Direction.Backward;
        }
    }
}
