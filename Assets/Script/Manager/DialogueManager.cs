using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    //public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;
    public bool isDialogueActive = false;

    public float typingSpeed = 0.02f; // Typing speed more realistic
    private Animator animator;

    private bool isTyping = false;
    private DialogueLine currentLine;
    private void Start()
    {
        animator = GetComponent<Animator>();

        if (Instance == null)
            Instance = this;

        lines = new Queue<DialogueLine>(); // Initialize queue
    }

    private void Update()
    {
        // Detect left mouse click or Space key
        if (isDialogueActive && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
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
        isDialogueActive = true;
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
        //characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        isTyping = true; // Set typing status

        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false; // Typing finished
    }

    void EndDialogue()
    {

        StartCoroutine(End());
       
    }

    IEnumerator End()
    {
        isDialogueActive = false;
        animator.SetTrigger("End");
        LevelLoader.Instance.LoadNextLevelTutorial();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        
    }
}
