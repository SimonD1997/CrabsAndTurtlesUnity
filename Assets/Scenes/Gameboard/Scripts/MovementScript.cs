using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Scenes.Gameboard.Scripts
{
    public class MovementScript : MonoBehaviour
    {
        public BoardFieldView canvasCamera;

        public Sprite playerIcon;
        public RenderTexture cameraImage;
        private SplineUser _splineUser;
        private Dreamteck.Splines.SplineFollower _splineFollower;
        public int currentPosition;
        private int _endPosition = 40;
        
        private Animator _anim;
        private Camera _camera;
    
        private Badges _badges;
        
        private AudioSource _audioSource;
        public AudioClip audioClipWalking;
        
        private byte _positionCard;
    
        private int _positionColour; 
    
        private bool _endOfGame = false;
        
        
        /// <summary>
        ///Array for the positions of the fields
        /// </summary>
        double[] _felder = new double[]
        {
            0,
            0.03833961,
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
            0.9642977,
            1
        };
        
        /// <summary>
        /// Colour of the fields
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
            _splineUser.SetClipRange(_felder[0], _felder[0]);

            _anim = _splineUser.gameObject.GetComponent<Animator>();
            _camera = this.gameObject.GetComponentInChildren<Camera>();
            _audioSource = this.gameObject.GetComponent<AudioSource>();
        
            currentPosition = 0;
            
            _endPosition = PlayerPrefs.GetInt("EndPosition", 40);
            
            _badges = this.gameObject.GetComponent<Badges>();

        }

        /// <summary>
        ///  Set the camera object active or inactive
        /// </summary>
        /// <param name="activ"> true = active; false = inactive </param>
        public void SetCameraActiv(bool activ)
        {
            if (activ)
            {
                canvasCamera.SetImages(cameraImage,playerIcon);
            }
            
            _camera.enabled = activ;
            canvasCamera.gameObject.SetActive(activ);
            
            
        } 
        
        /// <summary>
        /// Returns the animator of the player object
        /// </summary>
        /// <returns>animator of player object</returns>
        public Animator GetAnimator()
        {
            return _anim;
        }
    

        /// <summary>
        /// moves the player on the gameboard on a spline path
        /// </summary>
        /// <param name="steps"> steps forwards or backwards</param>
        public void Movement(int steps)
        {
            if (currentPosition == 0)
            {
                _anim.SetTrigger("toStart");
            }
            
            _audioSource.loop = true;
            _audioSource.clip = audioClipWalking;

        
            if (currentPosition + steps > _endPosition-1)
            {
                Debug.Log("End of Game Movement");
                
                _splineUser.SetClipRange(_felder[currentPosition], _felder[_endPosition]);
                _splineFollower.Restart();
                _splineFollower.Rebuild();
                
                currentPosition = _endPosition;
                _positionColour = _felderColour[currentPosition];
                
                if (!_endOfGame)
                {
                    _anim.SetBool("Walk", true);
                    _audioSource.Play();
                }
                _endOfGame = true;
                
                return;
            }
        
            if (steps > 0)
            {
                Debug.Log("Do Step Movement");

                _splineUser.SetClipRange(_felder[currentPosition], _felder[currentPosition + steps]);
                _splineFollower.Restart();
                _splineFollower.Rebuild();
                currentPosition = currentPosition + steps;
                _positionColour = _felderColour[currentPosition];

            }
            else if (steps < 0)
            {
                _splineUser.SetClipRange(_felder[currentPosition], _felder[currentPosition + steps]);
                _splineFollower.Restart();
                _splineFollower.Rebuild();
                _splineFollower.direction = Spline.Direction.Backward;
                currentPosition = currentPosition + steps;

            }

            _anim.SetBool("Walk", true);
            _audioSource.Play();
            

        }

        /// <summary>
        /// Get the position symbol on the game board so that the correct card can be drawn
        /// </summary>
        /// <returns> 0 = riddle card; 1 = action card </returns>
        public int GetPositionCard()
        {
            if (currentPosition%2 == 0)
            {
                return _positionCard = 0;
            }
            else
            {
                return _positionCard = 1;
            }
        
        }

        /// <summary>
        /// Get the current position of the player on the game board
        /// </summary>
        /// <returns> field number on the gameboard </returns>
        public int GetPosition()
        {
            return currentPosition;
        }
    
        /// <summary>
        /// Get the colour of the field on the game board
        /// </summary>
        /// <returns>colour of the field on the game board</returns>
        public int GetPositionColour()
        {
            return _positionColour;
        }
        
        /// <summary>
        /// Handles the end of the movement of the player with the animation and the audio
        /// </summary>
        public void EndOfMovement()
        {
            _anim.SetBool("Walk",false);
            _audioSource.loop = false;
            _audioSource.Stop();
        
        }
    
        
        /// <summary>
        /// Returns true if the player reached the end of the game
        /// </summary>
        /// <returns>if the player reached the end</returns>
        public bool GetEndOfGame()
        {
            return _endOfGame;
        }
    
        /// <summary>
        /// Returns the score of the player
        /// </summary>
        /// <returns>score of the player, summ of all badges</returns>
        public int GetScore()
        {
            return _badges.GetScore();
        }

        /// <summary>
        /// Moves the player to the side if the other player moves to the same field or back to the path
        /// </summary>
        /// <param name="toSide"> true = moves to side; false = moves back to path  </param>
        public void MoveAway(bool toSide)
        {
            _anim.SetBool("toSide",toSide);
        }
    }
}
