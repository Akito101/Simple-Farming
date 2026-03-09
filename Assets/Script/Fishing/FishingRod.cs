using UnityEngine;

public class FishingRod : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float castAngle = 45f;    // How far it rotates when casting
    public float castSpeed = 3f;     // How fast it rotates

    private Quaternion defaultRotation;
    private Quaternion castRotation;
    private bool isCasting = false;

    void Start()
    {
        defaultRotation = transform.localRotation;
        castRotation = Quaternion.Euler(0f, 0f, castAngle);
    }

    void Update()
    {
        if (isCasting)
        {
            // Smoothly rotate to cast angle
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                castRotation,
                Time.deltaTime * castSpeed
            );
        }
        else
        {
            // Smoothly rotate back to default
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                defaultRotation,
                Time.deltaTime * castSpeed
            );
        }
    }

    public void Cast()
    {
        isCasting = true;
    }

    public void ResetRod()
    {
        isCasting = false;
    }
}