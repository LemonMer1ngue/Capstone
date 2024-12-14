using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite bgImg;
    public Sprite canvasBgImg;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    Collider2D Collider;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "CutScene")
        {
            TriggerDialogue();
           
        }
        
        
        Collider = GetComponent<Collider2D>();
    }


    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            DialogueManager.Instance.DialogueStartTutorial(dialogue);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TriggerDialogue();
            Collider.enabled = false;
            Destroy(gameObject);
        }
    }

    IEnumerator AwakenStart()
    {
        yield return new WaitForSeconds(2);
        TriggerDialogue();
    }
}
