using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    [Header("Settings")]
    public float dayDuration = 30f;        // Total day length in seconds
    public TextMeshProUGUI dayText;

    [Header("Day/Night")]
    public Image nightOverlay;             // Dark UI image covering the screen
    public float maxNightAlpha = 0.6f;     // How dark night gets (0-1)

    public int CurrentDay { get; private set; } = 1;
    private float timer = 0f;
    public event Action OnNewDay;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dayText.text = "Day " + CurrentDay;
        if (nightOverlay != null)
            nightOverlay.color = new Color(0f, 0f, 0.1f, 0f); // Start fully transparent
    }

    void Update()
    {
        timer += Time.deltaTime;

        UpdateDayNightCycle();

        if (timer >= dayDuration)
        {
            timer = 0f;
            CurrentDay++;
            dayText.text = "Day " + CurrentDay;
            OnNewDay?.Invoke();
            Debug.Log("New day: " + CurrentDay);
        }
    }

    void UpdateDayNightCycle()
    {
        if (nightOverlay == null) return;

        float halfDay = dayDuration / 2f;
        float alpha = 0f;

        if (timer <= halfDay)
        {
            // First half = daytime, fully bright
            alpha = 0f;
        }
        else
        {
            // Second half = gradually get darker
            float nightProgress = (timer - halfDay) / halfDay;
            alpha = Mathf.Lerp(0f, maxNightAlpha, nightProgress);
        }

        nightOverlay.color = new Color(0f, 0f, 0.1f, alpha);
    }

    public float GetDayProgress()
    {
        return timer / dayDuration;
    }
}