using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHolder : MonoBehaviour
{

    private DialogueManager dMan;

    private bool hasEntered;

    public string[] dialogueLines;
    public string dialogAuthor;

    void Start()
    {
        dMan = FindObjectOfType<DialogueManager>();
    }
    
  
    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.name == "Player Knight" && !hasEntered)
        {
            hasEntered = true;
            Debug.Log("Player has entered");
            //dMan.ShowBox(dialogue);

            if (!dMan.dialogActive)
            {
                dMan.dialogLines = dialogueLines;
                dMan.dialogAuthor = dialogAuthor;
                dMan.currentLine = 0;
                dMan.ShowDialogue();
            }

            
            Time.timeScale = 0;
        }
    }
}
