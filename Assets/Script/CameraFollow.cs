using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("Follow Feel")]
    [Tooltip("Higher = snappier camera. 0.1-0.2 is smooth.")]
    public float smoothTime = 0.15f;

    [Tooltip("How much the camera shifts toward movement direction.")]
    public float lookAheadDistance = 1.0f;

    [Tooltip("How fast look-ahead reacts.")]
    public float lookAheadLerp = 10f;

    [Header("Optional Bounds")]
    public bool useBounds = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Vector3 velocity = Vector3.zero;
    private Vector2 lastTargetPos;
    private Vector2 lookAhead;

    void Start()
    {
        if (target == null) return;
        lastTargetPos = target.position;
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector2 currentPos = target.position;
        Vector2 delta = currentPos - lastTargetPos;
        lastTargetPos = currentPos;

        // Look-ahead based on movement direction (Pokémon-ish feel)
        Vector2 desiredLookAhead = Vector2.zero;
        if (delta.sqrMagnitude > 0.0001f)
            desiredLookAhead = delta.normalized * lookAheadDistance;

        lookAhead = Vector2.Lerp(lookAhead, desiredLookAhead, lookAheadLerp * Time.deltaTime);

        Vector3 desired = new Vector3(
            target.position.x + lookAhead.x,
            target.position.y + lookAhead.y,
            transform.position.z
        );

        // SmoothDamp for smooth follow
        Vector3 smoothed = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);

        // Clamp to bounds if enabled (for maps)
        if (useBounds)
        {
            smoothed.x = Mathf.Clamp(smoothed.x, minBounds.x, maxBounds.x);
            smoothed.y = Mathf.Clamp(smoothed.y, minBounds.y, maxBounds.y);
        }

        transform.position = smoothed;
    }
}