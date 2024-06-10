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
    public GameObject descBox;
    public TMP_Text infoText;
    public GameObject tableBox;
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
        if (active && dictionaryStorage.infoDict.TryGetValue(animal, out string a))
        {
            if (!canvas.enabled)
            {
                canvas.enabled = true;
                nameText.SetText(animal);
                CameraMode();
            }
        } else
        {
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
        nameText.SetText(animal);
        myButton.image.sprite = infoSprite;
        image.enabled = false;
        tableBox.SetActive(true);
        descBox.SetActive(false);
        country.SetText(dictionaryStorage.tableDict[animal][0]);
        habitat.SetText(dictionaryStorage.tableDict[animal][1]);
        advisory.SetText(dictionaryStorage.tableDict[animal][2]);
    }

    public void InformationMode()
    {
        camMode = false;
        nameText.SetText(animal);
        image.enabled = true;
        myButton.image.sprite = camSprite;
        if (dictionaryStorage.imageDict.TryGetValue(animal, out Texture2D texture))
        {
            image.sprite = TextureToSpriteConversion(texture);
        }
        tableBox.SetActive(false);
        descBox.SetActive(true);
        infoText.SetText(dictionaryStorage.infoDict[animal]);
    }
}
