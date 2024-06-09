using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class DictionaryStorage : MonoBehaviour
{
    //all keys are animal name
    public Dictionary<String, String> descDict = new Dictionary<String, String>(); //slide 2
    public Dictionary<String, String> infoDict = new Dictionary<String, String>(); //slide 4
    public Dictionary<String, Texture2D> imageDict = new Dictionary<string, Texture2D>(); //slide 4
    private void Start()
    {
        imageDict.Add("Crocodile",Resources.Load<Texture2D>("Images/Crocodile"));
        descDict.Add("Crocodile", "Crocodiles are large reptiles that can be found in the tropical regions of Asia, Africa, Australia and the Americas. The species that is found in Singapore is the Saltwater Crocodile, and it can grow to more than 5m in length. Do stay clam and back away slowly. Do not approach, provoke or feed the crocodile. Call ACRES Wildlife Rescue Hotline on 97837782.");
        infoDict.Add("Crocodile", "Crocodiles inhabit brackish and freshwater areas such as coastal areas and wetlands.They camouflage well and are difficult to see because they are often submerged in water.They hunt mainly at night, feeding mostly on fish, mammals, birds and carcasses. Do stay calm and back away slowly. Do not approach, provoke or feed the crocodile. Call ACRES Wildlife Rescue Hotline on 97837782.");
    }
}
