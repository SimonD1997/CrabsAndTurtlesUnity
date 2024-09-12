using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public List<Image> imageList;
    private List<Sprite> _spritesForImages;
    
    // Start is called before the first frame update
    void Start()
    {
        _spritesForImages = new List<Sprite>();
    }

    public void showImagesforSprites(List<Sprite> sprites)
    {
        _spritesForImages = sprites;

        for (int i = 0; i < _spritesForImages.Count; i++)
        {
            imageList[i].sprite = _spritesForImages[i];
            imageList[i].gameObject.SetActive(true);
        }

        if (imageList.Count - _spritesForImages.Count != 0)
        {
            for (int i = _spritesForImages.Count; i < imageList.Count; i++)
            {
                imageList[i].gameObject.SetActive(false);
            }
        }
    }
    
    
}
