using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalShortcuts : MonoBehaviour
{
    public delegate void shortcutAction();
    public Dictionary<KeyCode, shortcutAction> shortcutMap;

    private void Awake()
    {
        shortcutMap = new Dictionary<KeyCode, shortcutAction>();

        shortcutMap.Add(KeyCode.Escape, OnEsc);
    }

    // Update is called once per frame
    void Update()
    {
        foreach(KeyValuePair<KeyCode, shortcutAction> pair in shortcutMap)
        {
            if (Input.GetKey(pair.Key))
            {
                pair.Value();
            }
        }
    }

    private void OnEsc()
    {
        Debug.Log("Escape key down");
        Application.Quit();
    }
}
