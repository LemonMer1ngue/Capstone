using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite bgImg;
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

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "CutScene")
        {
            TriggerDialogue();
        }
        else
        {
            StartCoroutine(AwakenStart());
        }
        
    }

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TriggerDialogue();
        }
    }

    IEnumerator AwakenStart()
    {
        yield return new WaitForSeconds(3);
        TriggerDialogue();
    }
}
