using Dreamteck.Splines;
using UnityEngine;

namespace Scenes.Gameboard.Scripts
{
    public class MovementScript : MonoBehaviour
    {
        public GameObject _canvas;
        private SplineUser _splineUser;
        private Dreamteck.Splines.SplineFollower _splineFollower;
        private int aktuellePosition;
        private Animator _anim;
        private Camera _camera;
    
        private Abzeichen _badges;
    
        /// <summary>
        /// gerade positionen sind rästelkarten und ungerade sind ereigniskarten
        /// </summary>
        private byte _positionCard;
    
        private int _positionColour; 
    
        private bool _endOfGame = false;
    
    
        // Array für distanzen der einzelnen Felder
        double[] _felder = new double[]
        {
            0,0.03833961,
            0.06101561,
            0.08522148,
            0.1099248,
            0.1344986,
            0.159537,
            0.1829261,
            0.2089308,
            0.2323698,
            0.2579329,
            0.2813587,
            0.3058194,
            0.3308209,
            0.3549798,
            0.3795657,
            0.4028783,
            0.4264711,
            0.4502209,
            0.4755804,
            0.5012724,
            0.5248798,
            0.5495136,
            0.5737366,
            0.5969448,
            0.6227521,
            0.646851,
            0.6717836,
            0.693669,
            0.7188823,
            0.7437113,
            0.7674479,
            0.7909854,
            0.8165284,
            0.8416115,
            0.8663625,
            0.8903697,
            0.9136136,
            0.9375497,
            0.9642977
        };
        /// <summary>
        /// red = 1
        /// blue = 2
        /// gruen = 3
        /// gelb = 4
        /// </summary>
        int[] _felderColour = new int[]
        {
            0,
            1,
            2,
            3,
            1,
            4,
            3,
            2,
            4,
            1,
            2,
            3,
            1,
            4,
            3,
            2,
            4,
            1,
            2,
            3,
            1,
            4,
            3,
            2,
            4,
            1,
            2,
            3,
            1,
            4,
            3,
            2,
            4,
            1,
            2,
            3,
            1,
            4,
            3,
            2,
            4,
        };


        // Start is called before the first frame update
        void Start()
        {
            _splineUser = GetComponent<SplineUser>();
            _splineFollower = GetComponent<Dreamteck.Splines.SplineFollower>();
            //setzt die Anfangs und Endanimationspunkte fest.
            _splineUser.SetClipRange(_felder[0], _felder[0]);

            _anim = _splineUser.gameObject.GetComponent<Animator>();
            _camera = this.gameObject.GetComponentInChildren<Camera>();
        
            aktuellePosition = 0;

            //setzt die Richtungsparameter bzw. lässt den start manuell auswählen
            //_splineFollower.autoStartPosition = false;
            //_splineFollower.direction = Spline.Direction.Backward;
            //
            //legt den Startpunkt durch den Abstand vom Anfang des Splines fest in Prozent 
            //deshalb möglichkeit des von der Streckenlänge in Prozent umzurechnen
            //float splineLength = _splineFollower.spline.CalculateLength();
            //_splineFollower.SetDistance(splineLength,false,false);

            //TODO noch Möglichkeit suchen den Abstand der Knoten vom Startpunkt auszulesen...!!!

            _badges = this.gameObject.GetComponent<Abzeichen>();

        }

        public void SetCameraActiv(bool activ)
        {
            _camera.enabled = activ;
            //_canvas.SetActive(activ);
        } 
        
        public Animator GetAnimator()
        {
            return _anim;
        }
    

        /// <summary>
        /// Lässt das Objekt an einem gegeben Spline entlanglaufen.
        /// </summary>
        /// <param name="steps"> legt fest wie viele Blöcke nach vorne oder zurückgelaufen werden messen</param>
        public void Movement(int steps)
        {
            // falls über oder unter die Arraygrenze kommt, schauen wie in den Regeln behandelt werden soll;
            // ob man direkt treffen muss oder solange weitergeht wie es halt geht
        
            if (aktuellePosition + steps > _felder.Length-1)
            {
                _endOfGame = true;
                _splineUser.SetClipRange(_felder[aktuellePosition], 1);
                _splineFollower.Restart();
                _splineFollower.Rebuild();
                _anim.SetBool("Walk", true);
                return;
            }
        
        
            if (steps > 0)
            {

                _splineUser.SetClipRange(_felder[aktuellePosition], _felder[aktuellePosition + steps]);
                _splineFollower.Restart();
                _splineFollower.Rebuild();
                aktuellePosition = aktuellePosition + steps;
                _positionColour = _felderColour[aktuellePosition];

            }
            else if (steps < 0)
            {
                _splineUser.SetClipRange(_felder[aktuellePosition], _felder[aktuellePosition + steps]);
                _splineFollower.Restart();
                _splineFollower.Rebuild();
                _splineFollower.direction = Spline.Direction.Backward;
                aktuellePosition = aktuellePosition + steps;

            }

            _anim.SetBool("Walk", true);

        

        }

        public int GetPositionCard()
        {
            if (aktuellePosition%2 == 0)
            {
                return _positionCard = 0;
            }
            else
            {
                return _positionCard = 1;
            }
        
        }

        public int GetPosition()
        {
            return aktuellePosition;
        }
    
        public int GetPositionColour()
        {
            return _positionColour;
        }

        public void EndOfMovement()
        {
            _anim.SetBool("Walk",false);
        
        }
    
        public bool GetEndOfGame()
        {
            return _endOfGame;
        }
    
        public int GetScore()
        {
            return _badges.GetScore();
        }
    }
}
