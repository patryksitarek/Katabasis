using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public GameObject dBox;
    public Text dText;
    public Text dAuthor;

    public bool dialogActive;

    public string[] dialogLines;
    public string dialogAuthor;
    public int currentLine;



    void Update()
    {
        if (dialogActive && Input.GetKeyDown(KeyCode.E)) 
        {
            currentLine++;
        }

        if (currentLine >= dialogLines.Length)
        {
            dBox.SetActive(false);
            dialogActive = false;
            currentLine = 0;
            Time.timeScale = 1;
        }

        dText.text = dialogLines[currentLine];
        dAuthor.text = dialogAuthor;
    }

    public void ShowBox(string dialogue)
    {
        dialogActive = true;
        dBox.SetActive(true);
        dText.text = dialogue;
    }

    public void ShowDialogue()
    {
        dialogActive = true;
        dBox.SetActive(true);
    }
}
