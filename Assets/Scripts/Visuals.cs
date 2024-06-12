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
    private string prevAnimal = "";
    private string animal = "";

    public Button myButton;
    private Boolean camMode;
    private Sprite camSprite;
    private Sprite infoSprite;



    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
        myButton.onClick.AddListener(OnClick);
        canvas.enabled = false;
        camSprite = Resources.Load<Sprite>("Camera");
        infoSprite = Resources.Load<Sprite>("Information");
    }
    int frameCount = 0;
    private void Update()
    {
        frameCount++;
        if (frameCount > 9)
        {
            frameCount = 0;
            VUpdate();
        }
  
    }

    // Update is called once per frame
    void VUpdate()
    {
        if (detector.getDetectionResults() == "")
        {
            canvas.enabled = false;
            return;
        }
        animal = detector.getDetectionResults();
        nameText.SetText(animal);
        if (!canvas.enabled)
        {
            canvas.enabled = true;
            CameraMode();
        }
        if (prevAnimal != animal)
        {
            CameraMode();
        }
        prevAnimal = animal;
    
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
