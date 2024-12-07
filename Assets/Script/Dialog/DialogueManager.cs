using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Image background;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;
    

    public float typingSpeed = 0.01f; 
    private Animator animator;

    private bool isTyping = false;
    private DialogueLine currentLine;
    private void Awake()
    {
        gameObject.SetActive(false);
        animator = GetComponent<Animator>();

        if (Instance == null)
            Instance = this;

        lines = new Queue<DialogueLine>(); 
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            if (isTyping)
            {
                // Instantly finish typing if typing is ongoing
                StopAllCoroutines();
                dialogueArea.text = currentLine.line; // Show full line
                isTyping = false;
            }
            else
            {
                DisplayNextDialogueLine(); // Move to next line
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        gameObject.SetActive (true);
        animator.SetTrigger("Start");

        lines.Clear();
        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentLine = lines.Dequeue();
        background.sprite = currentLine.character.bgImg;
        characterName.text = currentLine.character.name;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        isTyping = true; 

        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false; 
    }

    void EndDialogue()
    {

        StartCoroutine(End());
       
    }

    IEnumerator End()
    {
        
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1);
        if (SceneManager.GetActiveScene().name == "CutScene")
        {
            LevelLoader.Instance.LoadNextLevelTutorial();
        }
            
            gameObject.SetActive(false);
    }
}
