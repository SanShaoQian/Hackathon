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
    public ObjectDetector detector;
    private string animal;

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
        if (detector.getDetectionResults() == "")
        {
            return;
        }
        String animal = detector.getDetectionResults();
        if (dictionaryStorage.infoDict.TryGetValue(animal, out string a))
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
        return Sprite.Create(
            texture,
            new Rect(0.0f, 0.0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            100.0f
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
        if (dictionaryStorage.tableDict.TryGetValue(animal, out string[] array))
        {
            country.SetText(array[0]);
            habitat.SetText(array[1]);
            advisory.SetText(array[2]);
        }
        else
        {
            country.SetText("");
            habitat.SetText("");
            advisory.SetText("");
        }
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
        if (dictionaryStorage.infoDict.TryGetValue(animal, out string a))
        {
            infoText.SetText(a);
        }
    }
}
