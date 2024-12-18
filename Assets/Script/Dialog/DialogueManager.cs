using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject Player;

    [Header("Dialgue")]
    public GameObject background;
    public Image canvasBG;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;


    public float typingSpeed = 0.01f;
    private Animator animator;

    private bool isDialogueActive = false;
    private bool isTyping = false;
    private DialogueLine currentLine;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        lines = new Queue<DialogueLine>();
        transform.Find("Name").gameObject.SetActive(false);
        transform.Find("DialogueText").gameObject.SetActive(false);
        transform.Find("Body").gameObject.SetActive(false);
        transform.Find("BgImg").gameObject.SetActive(false);
        transform.Find("BackGround").gameObject.SetActive(false);

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Player = player;
            if (Player.GetComponent<PlayerMovement>() == null)
            {
                Debug.LogError("Komponen PlayerMovement tidak ditemukan pada object Player! Pastikan Player memiliki komponen PlayerMovement.");
            }
        }
        else
        {
            Debug.LogError("Player tidak ditemukan di scene!");
        }


    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;
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
        transform.Find("Name").gameObject.SetActive(true);
        transform.Find("DialogueText").gameObject.SetActive(true);
        transform.Find("Body").gameObject.SetActive(true);
        transform.Find("BgImg").gameObject.SetActive(true);
        transform.Find("BackGround").gameObject.SetActive(true);
        isDialogueActive = true;
        animator.SetTrigger("Start");

        //if (Player != null && Player.GetComponent<PlayerMovement>() != null)
        //{
        //    Player.GetComponent<PlayerMovement>().enabled = false; // Disable PlayerMovement during dialogue
        //}

        lines.Clear();
        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DialogueStartTutorial(Dialogue dialogue)
    {

        StartCoroutine(AwakenStartDialogue(dialogue));
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentLine = lines.Dequeue();
        background.GetComponent<SpriteRenderer>().sprite = currentLine.character.bgImg;
        canvasBG.sprite = currentLine.character.canvasBgImg;
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
        isDialogueActive = false;
        yield return new WaitForSeconds(1);
        if (SceneManager.GetActiveScene().name == "CutScene")
        {
            LevelLoader.Instance.LoadNextLevelTutorial();
        }
        if (Player != null && Player.GetComponent<PlayerMovement>() != null)
        {
            Player.GetComponent<PlayerMovement>().enabled = true;
        }
        transform.Find("Name").gameObject.SetActive(false);
        transform.Find("DialogueText").gameObject.SetActive(false);
        transform.Find("Body").gameObject.SetActive(false);
        transform.Find("BgImg").gameObject.SetActive(false);
        transform.Find("BackGround").gameObject.SetActive(false);


    }

    IEnumerator AwakenStartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        transform.Find("BackGround").gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        transform.Find("Name").gameObject.SetActive(true);
        transform.Find("DialogueText").gameObject.SetActive(true);
        transform.Find("Body").gameObject.SetActive(true);
        transform.Find("BgImg").gameObject.SetActive(false);
        animator.SetTrigger("Start");

        if (Player != null && Player.GetComponent<PlayerMovement>() != null)
        {
            Player.GetComponent<PlayerMovement>().enabled = false; // Disable PlayerMovement during dialogue
        }

        lines.Clear();
        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }
}
