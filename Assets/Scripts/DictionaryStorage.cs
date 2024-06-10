using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Linq;

public class DictionaryStorage : MonoBehaviour
{
    //all keys are animal name
    public Dictionary<String, String[]> tableDict = new Dictionary<String, String[]>(); 
    public Dictionary<String, String> infoDict = new Dictionary<String, String>(); 
    public Dictionary<String, Texture2D> imageDict = new Dictionary<string, Texture2D>();
    public String[] animals;
   
    private void Start()
    {
        animals = Resources.Load<UnityEngine.TextAsset>("Animals").text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (String anim in animals)
        { 
            Texture2D image = Resources.Load<Texture2D>("Images/" + anim + ".jpg");
            if (image != null)
            {
                Debug.Log("in");
                imageDict.Add(anim, image);
                Debug.Log(anim);
            }
        }
        string[] text = Resources.Load<UnityEngine.TextAsset>("Text").text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < text.Length; i += 5)
        {
            if (i + 1 < text.Length)
            {
                string key = text[i].Trim();
                string value = text[i + 4].Trim();
                infoDict[key] = value;
                tableDict[key] = text.Skip(i + 1).Take(3).ToArray();
            }
        }
    }
}
