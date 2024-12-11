using Scenes.Gameboard.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;


public class CardFlipClick : MonoBehaviour
{
    Camera m_Camera;
    private Animator _anim;
    private AudioSource _audioSource;
    private Collider _collider;
    private bool _down = true;
    private GameController _gameController;

    void Awake()
    {
        m_Camera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        _collider = this.gameObject.GetComponent<Collider>();
        _anim = this.gameObject.GetComponent<Animator>();
        _gameController = FindFirstObjectByType<GameController>();
        _audioSource = this.gameObject.GetComponent<AudioSource>();

        //this.gameObject.GetComponent<Animator>().enabled = false;
    }

    /// <summary>
    /// Check if the card is clicked, then turn the card
    /// and start the riddle or the action of the card
    /// </summary>
    /// <param name="hit"> the object that gets hit from raycast</param>
    void ScreenHit(RaycastHit hit)
    {
        if (hit.collider == _collider && (_gameController.GetGameState() == 1 || _gameController.debugMode))
        {
            // only if the card is not already turned
            if (_down == true)
            {
                _anim.enabled = true;
                _anim.SetTrigger("Klick");
                _audioSource.Play();
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
            
        }
    }

    // Update is called once per frame
    /// <summary>
    /// check if the card is clicked by mouse or touch
    /// </summary>
    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePosition = mouse.position.ReadValue();
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                ScreenHit(hit);
                
            }
        }

        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                // Construct a ray from the current touch coordinates
                Ray ray = m_Camera.ScreenPointToRay(Input.GetTouch(i).position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    ScreenHit(hit);
                }
            }
        }
    }


    /// <summary>
    /// Get the turn state of the card
    /// </summary>
    /// <returns>true = card is not turned, false = cards is turned</returns>
    public bool GetTurnState()
    {
        return _down;
    }

    /// <summary>
    /// Activate the cklick animation of the card.
    /// Used for discarding the card from the stack
    /// </summary>
    public void ClickStateActivate()
    {
        _anim.SetTrigger("Klick");
    }
}