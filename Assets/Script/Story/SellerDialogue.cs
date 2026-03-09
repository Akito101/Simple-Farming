using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SellerDialogue : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;

    [Header("Seller Info")]
    public string speakerName = "Merchant";
    public Sprite speakerPortrait;

    [Header("Required Items")]
    public string item1Name = "Egg";
    public string item2Name = "Tomato";
    public string item3Name = "Carrot";
    public string item4Name = "Fish";
    public int requiredAmount = 5;

    [Header("Reward UI")]
    public GameObject rewardPanel;         // Panel showing the reward item
    public Image rewardImage;             // The reward item image
    public Sprite rewardSprite;           // Drag sprite here in Inspector

    [TextArea(2, 4)]
    public string[] linesBeforeTalkedToGrandpa = {
        "Welcome to my shop!",
        "I don't have anything for strangers though...",
        "Maybe ask around the village first!"
    };

    [TextArea(2, 4)]
    public string[] linesBeforeTrade = {
        "Ah! The old grandpa sent you!",
        "I have what he needs...",
        "Bring me 5 Eggs, 5 Fish, 5 Tomatoes and 5 Carrots!",
        "Come back when you have them all!"
    };

    [TextArea(2, 4)]
    public string[] linesMissingItems = {
        "You don't have everything yet!",
        "I need 5 Eggs, 5 Fish, 5 Tomatoes AND 5 Carrots.",
        "Come back when you have them all!"
    };

    [TextArea(2, 4)]
    public string[] linesAfterTrade = {
        "Well done! You brought everything!",
        "Here, take this back to the old man.",
        "Tell him the merchant says hello!"
    };

    [TextArea(2, 4)]
    public string[] linesAlreadyTraded = {
        "You already have what you need!",
        "Go back to the grandpa, quickly!"
    };

    private bool playerInRange = false;
    private bool isDialogueOpen = false;
    private bool tradeComplete = false;
    private bool endingAfterDialogue = false;
    private int lineIndex = 0;
    private string[] currentLines;

    void Start()
    {
        dialoguePanel.SetActive(false);
        if (rewardPanel != null)
            rewardPanel.SetActive(false);
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

        if (tradeComplete)
        {
            currentLines = linesAlreadyTraded;
        }
        else if (!GrandpaDialogue.hasSpokenToGrandpa)
        {
            // Player hasn't talked to grandpa yet!
            currentLines = linesBeforeTalkedToGrandpa;
        }
        else if (HasRequiredItems())
        {
            currentLines = linesAfterTrade;
            endingAfterDialogue = true;
        }
        else
        {
            currentLines = linesBeforeTrade;
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

        if (endingAfterDialogue)
        {
            endingAfterDialogue = false;
            CompleteTrade();
        }
    }

    bool HasRequiredItems()
    {
        return InventoryManager.Instance.GetItemCount(item1Name) >= requiredAmount &&
               InventoryManager.Instance.GetItemCount(item2Name) >= requiredAmount &&
               InventoryManager.Instance.GetItemCount(item3Name) >= requiredAmount &&
               InventoryManager.Instance.GetItemCount(item4Name) >= requiredAmount;
    }

    void CompleteTrade()
    {
        tradeComplete = true;

        // Remove items
        for (int i = 0; i < requiredAmount; i++)
        {
            InventoryManager.Instance.RemoveItem(item1Name);
            InventoryManager.Instance.RemoveItem(item2Name);
            InventoryManager.Instance.RemoveItem(item3Name);
            InventoryManager.Instance.RemoveItem(item4Name);
        }

        // Tell grandpa player has the item
        GrandpaDialogue.hasItemForGrandpa = true;

        // Show reward popup
        if (rewardPanel != null)
        {
            rewardPanel.SetActive(true);
            if (rewardImage != null && rewardSprite != null)
            {
                rewardImage.sprite = rewardSprite;
                rewardImage.color = new Color(1f, 1f, 1f, 1f); // Make sure alpha is visible!
            }
        }

        Debug.Log("Trade complete! Go back to Grandpa!");
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