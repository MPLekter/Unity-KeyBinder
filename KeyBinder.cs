using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class KeyBinder : MonoBehaviour
{
    [SerializeField] Text pressedKeyText;
    [SerializeField] Text actionToBindText;
    
    private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));

    string actionToBind;
    KeyCode keyCodeToBind;

    bool isBinding = false;
    Dictionary<string, KeyCode> myBindingsDict;


    private void Start()
    {
        //create a dictionary that will hold Action names and binded KeyCodes
        myBindingsDict = new Dictionary<string, KeyCode>();
        CreateNewDict(myBindingsDict);
        //now run a loop for every Action. Show their names and allow binding keys. 
        StartCoroutine(StartBinding(myBindingsDict));
    }

    private IEnumerator StartBinding(Dictionary<string, KeyCode> myBindingsDict)
    {
        foreach (string key in myBindingsDict.Keys.ToList()) //Use ToList, otherwise you will be modyfing the same Dict at runtime, which throws an error!
        {
            isBinding = true;
            actionToBind = key;

            while (isBinding)
            {
                ShowActionToBind(key);
                yield return null;
            }
            myBindingsDict[actionToBind] = keyCodeToBind;
            Debug.Log(myBindingsDict[actionToBind]);
            continue;
        }

        PrintTheDict();

    }

    private void PrintTheDict()
    {
        /*
        foreach (string key in myBindingsDict.Keys)
            Debug.Log(key);
        foreach (KeyCode value in myBindingsDict.Values)
            Debug.Log(value);
        */
        foreach (KeyValuePair<string, KeyCode> kvp in myBindingsDict)
            Debug.Log(kvp.ToString());
    }

    void Update()
    {
        if (Input.anyKeyDown && isBinding) 
        {
            foreach (KeyCode keyCode in keyCodes)
            {
                if (Input.GetKey(keyCode))
                {
                    //Debug.Log("KeyCode down: " + keyCode);
                    ShowPressedKey(keyCode);
                    keyCodeToBind = keyCode;
                    isBinding = false;
                    break;
                }
            }
        }
    }

    private static void CreateNewDict(Dictionary<string, KeyCode> emptyBindingsDict)
    {
        foreach (AbstractActions actionType in Enum.GetValues(typeof(AbstractActions)))
        {
            emptyBindingsDict.Add(actionType.ToString(), KeyCode.None); //TODO think about safe way to replace LeftArrow with something like null? 
        }

    }

    private void ShowActionToBind(string key)
    {
        actionToBindText.text = key;
    }

    public void ShowPressedKey(KeyCode keyCode)
    {
        pressedKeyText.text = keyCode.ToString();
    }
}
