using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Linq;

public class Visuals : MonoBehaviour
{
    //text
    [SerializeField]
    public Canvas canvas;
    public TMP_Text nameText;
    public TMP_Text infoText;
    public TMP_Text country;
    public TMP_Text habitat;
    public TMP_Text advisory;
    public Image image;
    public DictionaryStorage dictionaryStorage;

    //check if animal present
    //from image recognition
    private Boolean active = true;
    private String animal = "Red panda";

    public Button myButton;
    private Boolean camMode;
    private Sprite camSprite;
    private Sprite infoSprite;



    // Start is called before the first frame update
    void Start()
    {
        myButton.onClick.AddListener(OnClick);
        camMode = true;
        camSprite = Resources.Load<Sprite>("Camera");
        infoSprite = Resources.Load<Sprite>("Information");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(dictionaryStorage.animals.Contains(animal));
        if (active && dictionaryStorage.animals.Contains(animal))
        {
            Debug.Log("in");
            canvas.enabled = true;
            CameraMode();
        } else
        {
            CameraMode();
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
        Debug.Log("button clicked");
        if (camMode)
        {
            InformationMode();
        }
        else
        {
            CameraMode();
        }
    }

    public void CameraMode()
    {
        camMode = true;
        myButton.image.sprite = infoSprite;
        image.enabled = false;
        country.SetText(dictionaryStorage.tableDict[animal][0]);
        habitat.SetText(dictionaryStorage.tableDict[animal][1]);
        advisory.SetText(dictionaryStorage.tableDict[animal][2]);
    }

    public void InformationMode()
    {
        camMode = false;
        image.enabled = true;
        myButton.image.sprite = camSprite;
        image.sprite = TextureToSpriteConversion(dictionaryStorage.imageDict[animal]);
        infoText.SetText(dictionaryStorage.infoDict[animal]);
    }
}
