using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logiccontroller : MonoBehaviour, dialogFocus
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void runWithExpectedOutput(DialogueContent content)
    {
        Debug.Log("??????????"+content.ToString("G"));
    }
}
