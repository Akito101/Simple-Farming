using UnityEngine;

public class FishingDetector : MonoBehaviour
{
    [Header("References")]
    public GameObject fishingRod;
    public FishingMinigame minigame;

    [Header("Sound")]
    public AudioClip waterAmbience;
    public AudioClip splashSound;
    public float ambienceVolume = 0.3f;
    public float splashVolume = 0.8f;

    private AudioSource audioSource;
    private bool nearWater = false;
    private bool isFishing = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = ambienceVolume;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && nearWater && !isFishing)
        {
            StartFishing();
        }
    }

    void StartFishing()
    {
        isFishing = true;
        fishingRod.SetActive(true);

        if (splashSound != null)
            audioSource.PlayOneShot(splashSound, splashVolume);

        minigame.StartMinigame(this);
    }

    public void StopFishing()
    {
        isFishing = false;
        fishingRod.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            nearWater = true;

            if (waterAmbience != null)
            {
                audioSource.clip = waterAmbience;
                audioSource.Play();
            }

            Debug.Log("Near water!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            nearWater = false;
            audioSource.Stop();
            Debug.Log("Left water!");
        }
    }
}