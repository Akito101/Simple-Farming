using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueTriggerTMP : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;

    [Header("Dialogue Content")]
    public string speakerName = "Friend";
    public Sprite speakerPortrait;

    [TextArea(2, 4)]
    public string[] lines;

    private bool playerInRange = false;
    private bool isDialogueOpen = false;
    private int lineIndex = 0;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isDialogueOpen)
                StartDialogue();
            else
                NextLine();
        }

        if (isDialogueOpen && Input.GetKeyDown(KeyCode.Escape))
            EndDialogue();
    }

    void StartDialogue()
    {
        if (lines.Length == 0) return;

        isDialogueOpen = true;
        lineIndex = 0;

        dialoguePanel.SetActive(true);
        nameText.text = speakerName;
        dialogueText.text = lines[lineIndex];
        portraitImage.sprite = speakerPortrait;
    }

    void NextLine()
    {
        lineIndex++;

        if (lineIndex >= lines.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = lines[lineIndex];
    }

    void EndDialogue()
    {
        isDialogueOpen = false;
        dialoguePanel.SetActive(false);
        lineIndex = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            EndDialogue();
        }
    }
}