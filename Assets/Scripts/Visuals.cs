using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class Visuals : MonoBehaviour
{
    //text
    [SerializeField]
    public Canvas canvas;
    public TMP_Text nameText;
    public TMP_Text descText;
    public Image image;
    public DictionaryStorage dictionaryStorage;

    //check if animal present
    //from image recognition
    private Boolean active = true;
    private string animal = "Crocodile";

    public Button myButton;
    private Boolean camMode;
    public Sprite camSprite;
    public Sprite infoSprite;



    // Start is called before the first frame update
    void Start()
    {
        myButton.onClick.AddListener(OnClick);
        camMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && dictionaryStorage.descDict.TryGetValue(animal, out string desc))
        {
            canvas.enabled = true;
            nameText.SetText(animal);
            descText.SetText(desc);
        } else
        {
            myButton.image.sprite = camSprite;
            camMode = true;
            image.enabled = false;
            canvas.enabled = false;
        }
    }
    private Sprite TextureToSpriteConversion(Texture2D texture)
    {
        // Create a new Sprite from the Texture2D
        return Sprite.Create(
            texture,
            new Rect(0.0f, 0.0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            100.0f // Pixels per unit
        );
    }

    void OnClick()
    {
        if (camMode)
        {
            myButton.image.sprite = infoSprite;
            image.enabled = true;
            image.sprite = TextureToSpriteConversion(dictionaryStorage.imageDict[animal]);
            descText.SetText(dictionaryStorage.infoDict[animal]);
        }
        else
        {
            myButton.image.sprite = camSprite;
            image.enabled = false;
            descText.SetText(dictionaryStorage.descDict[animal]);
        }
        myButton.onClick.RemoveListener(OnClick);
    }
}
