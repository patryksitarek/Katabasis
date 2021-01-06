using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnClickExit()
    {
        Debug.Log("Exitting application");
        Application.Quit();
    }
}
