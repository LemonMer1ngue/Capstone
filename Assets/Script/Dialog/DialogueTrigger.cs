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
    private static bool isFirstTimeLoaded = true;

    void Awake()
    {
        if (isFirstTimeLoaded)
        {
            isFirstTimeLoaded = false;
            DontDestroyOnLoad(gameObject); // Optional jika ingin memastikan objek tidak dihancurkan saat pindah scene
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "CutScene")
        {
            TriggerDialogue();
           
        }
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            DialogueManager.Instance.DialogueStartTutorial(dialogue);
        }

        PlayerPrefs.DeleteAll(); // Hapus semua data PlayerPrefs
        Debug.Log("PlayerPrefs telah dihapus.");

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
        yield return new WaitForSeconds(2);
        TriggerDialogue();
    }
}
