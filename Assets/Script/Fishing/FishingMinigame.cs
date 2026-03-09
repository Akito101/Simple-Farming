using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishingMinigame : MonoBehaviour
{
    [Header("UI References")]
    public GameObject minigamePanel;
    public Image playerFill;
    public Image fishFill;
    public TextMeshProUGUI resultText;

    [Header("Difficulty Settings")]
    public float minFishPower = 2f;       // Slowest fish pull
    public float maxFishPower = 6f;       // Fastest fish pull
    public float playerPower = 8f;        // How hard Space pushes back
    public float maxValue = 100f;

    private float tugValue = 50f;
    private bool isPlaying = false;
    private FishingDetector detector;
    private float currentFishPower;

    [Header("Fish Item")]
    public Sprite fishSprite;
    public string fishItemName = "Fish";
    public int fishMaxStack = 64;

    void Update()
    {
        if (!isPlaying) return;

        // Fish ALWAYS pulls left (towards 0) at a constant random speed
        tugValue -= currentFishPower * Time.deltaTime;

        // Player presses Space to push right (towards 100)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tugValue += playerPower;
            Debug.Log("Space pressed! Tug value: " + tugValue);
        }

        tugValue = Mathf.Clamp(tugValue, 0f, maxValue);

        playerFill.fillAmount = tugValue / maxValue;
        fishFill.fillAmount = 1f - (tugValue / maxValue);

        if (tugValue >= maxValue)
        {
            WinFishing();
        }
        else if (tugValue <= 0f)
        {
            LoseFishing();
        }
    }

    public void StartMinigame(FishingDetector fishingDetector)
    {
        detector = fishingDetector;
        tugValue = 50f;
        isPlaying = true;
        resultText.text = "";

        // Pick a random fish power at the START of each catch
        currentFishPower = Random.Range(minFishPower, maxFishPower);
        Debug.Log("Fish power this round: " + currentFishPower);

        minigamePanel.SetActive(true);
    }

    void WinFishing()
    {
        isPlaying = false;
        resultText.text = "You caught a fish!";
        InventoryManager.Instance.AddItem(fishItemName, fishSprite, fishMaxStack);
        Invoke("EndMinigame", 1.5f);
    }

    void LoseFishing()
    {
        isPlaying = false;
        resultText.text = "Fish escaped!";
        Invoke("EndMinigame", 1.5f);
    }

    void EndMinigame()
    {
        minigamePanel.SetActive(false);
        detector.StopFishing();
    }
}