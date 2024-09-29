using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardFieldView : MonoBehaviour
{
    public RawImage _cameraImage;
    public Image _playerIcon;
    
    void Awake()
    {
        _cameraImage = GetComponent<RawImage>();
        _playerIcon = this.gameObject.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetImages(RenderTexture cameraImage, Sprite playerIcon)
    {
        _cameraImage.texture = cameraImage;
        _playerIcon.sprite = playerIcon;
    }
}
