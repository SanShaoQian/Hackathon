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
    public GameObject dialogBox;
    public TMP_Text dialogText;
    private Dictionary<String,String> descDict = new Dictionary<String,String>();

    //check if plant or animal present
    public Boolean active = true;
    private string species = "monkey";

    ARRaycastManager raycastManager;

    // Start is called before the first frame update
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinBounds))
        {
            if (hits.Count > 0)
            {
                //crosshair.transform.position = hits[0].pose.position;
                //crosshair.transform.rotation = hits[0].pose.rotation;
            }
        }

        if (active && descDict.TryGetValue(species, out string desc))
        {
            //Instantiate(objectToSpawn, crosshair.transform.position, crosshair.transform.rotation);

            if (dialogBox.activeInHierarchy)
            {
                dialogBox.SetActive(false);
            }
            else
            {
                dialogBox.SetActive(true);
                dialogText.SetText(desc);
            }
        }
    }
}
