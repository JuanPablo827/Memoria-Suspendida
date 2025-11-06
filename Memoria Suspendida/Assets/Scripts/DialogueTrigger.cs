using UnityEngine;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject bubbleE;
    public GameObject dialogueBox;
    public TMP_Text dialogueText;

    [Header("Dialogue Lines")]
    [TextArea(2, 4)]
    public string[] lines;

    private int index = 0;
    private bool playerNear = false;
    private bool dialogueActive = false;

    void Start()
    {
        if (bubbleE) bubbleE.SetActive(false);
        if (dialogueBox) dialogueBox.SetActive(false);
    }

    void Update()
    {
        if (playerNear && !dialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }

        if (dialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }

    void StartDialogue()
    {
        dialogueActive = true;
        bubbleE.SetActive(false);
        dialogueBox.SetActive(true);
        index = 0;
        dialogueText.text = lines[index];
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = lines[index];
        }
        else
        {
            dialogueActive = false;
            dialogueBox.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            if (!dialogueActive) bubbleE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            bubbleE.SetActive(false);

            if (dialogueActive)
            {
                dialogueActive = false;
                dialogueBox.SetActive(false);
            }
        }
    }
}