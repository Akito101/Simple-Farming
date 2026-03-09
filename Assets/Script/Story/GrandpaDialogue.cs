using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GrandpaDialogue : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;

    [Header("Grandpa Info")]
    public string speakerName = "Grandpa";
    public Sprite speakerPortrait;

    [Header("End Screen")]
    public GameObject gameEndPanel;        // The REAL end screen, shown after returning

    [TextArea(2, 4)]
    public string[] linesFirstMeet = {
        "Ah, welcome young one! This farm has been in our family for generations.",
        "I need a special item to keep this land alive...",
        "Head to the merchant in the village!",
        "But she won't give it for free — bring her 5 Eggs, 5 Fish, 5 Tomatoes and 5 Carrots.",
        "Now go! The farm is counting on you!"
    };

    [TextArea(2, 4)]
    public string[] linesWaiting = {
        "Have you been to the merchant yet?",
        "Remember — 5 Eggs, 5 Fish, 5 Tomatoes and 5 Carrots!",
        "She lives in the village, hurry!"
    };

    [TextArea(2, 4)]
    public string[] linesEnding = {
        "You got it! I knew you could do it!",
        "With this, the farm will thrive for generations...",
        "Thank you, young one. You've saved us all.",
        "This land is yours now. Take care of it!"
    };

    // Static so SellerDialogue can set it from anywhere
    public static bool hasSpokenToGrandpa = false;
    public static bool hasItemForGrandpa = false;

    private bool playerInRange = false;
    private bool isDialogueOpen = false;
    private bool endingTriggered = false;
    private int lineIndex = 0;
    private string[] currentLines;

    void Start()
    {
        dialoguePanel.SetActive(false);
        if (gameEndPanel != null)
            gameEndPanel.SetActive(false);
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
        isDialogueOpen = true;
        lineIndex = 0;
        dialoguePanel.SetActive(true);
        nameText.text = speakerName;
        portraitImage.sprite = speakerPortrait;

        if (hasItemForGrandpa)
        {
            // Player got the item from seller!
            currentLines = linesEnding;
            endingTriggered = true;
        }
        else if (hasSpokenToGrandpa)
        {
            // Already talked, still waiting
            currentLines = linesWaiting;
        }
        else
        {
            // First time meeting grandpa
            currentLines = linesFirstMeet;
            hasSpokenToGrandpa = true;
        }

        dialogueText.text = currentLines[lineIndex];
    }

    void NextLine()
    {
        lineIndex++;
        if (lineIndex >= currentLines.Length)
        {
            EndDialogue();
            return;
        }
        dialogueText.text = currentLines[lineIndex];
    }

    void EndDialogue()
    {
        isDialogueOpen = false;
        dialoguePanel.SetActive(false);
        lineIndex = 0;

        if (endingTriggered)
        {
            endingTriggered = false;
            TriggerGameEnd();
        }
    }

    void TriggerGameEnd()
    {
        if (gameEndPanel != null)
            gameEndPanel.SetActive(true);

        // Wait 3 seconds then go to main menu
        Invoke("LoadMainMenu", 3f);
        Debug.Log("Game ending, returning to main menu!");
    }

    void LoadMainMenu()
    {
        Time.timeScale = 1f; // Reset time in case it was paused
        SceneManager.LoadScene("MainMenu"); // ← Change this to your exact scene name!
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            EndDialogue();
        }
    }
}