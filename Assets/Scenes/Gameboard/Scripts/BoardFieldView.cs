using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BoardFieldView : MonoBehaviour
{
    public RawImage cameraImage;
    public Image playerIcon;
    
    void Awake()
    {
        cameraImage = GetComponent<RawImage>();
        playerIcon = this.gameObject.GetComponentInChildren<Image>();
    }
    
    /// <summary>
    ///  Set the player icon for the field camera
    /// </summary>
    /// <param name="cameraImage"> The camera image to be displayed</param>
    /// <param name="playerIcon"> The player icon to be displayed</param>
    public void SetImages(RenderTexture cameraImage, Sprite playerIcon)
    {
        this.cameraImage.texture = cameraImage;
        this.playerIcon.sprite = playerIcon;
    }
}
