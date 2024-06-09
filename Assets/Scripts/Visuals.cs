using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Visuals : MonoBehaviour
{
    //text
    [SerializeField]
    public Canvas canvas;
    public TMP_Text nameText;
    public TMP_Text descText;
    public DictionaryStorage dictionaryStorage;

    //check if animal present
    //from image recognition
    private Boolean active = true;
    private string animal = "Crocodile";

    ARRaycastManager raycastManager;

    // Start is called before the first frame update
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
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
            canvas.enabled = false;
        }
    }
}
