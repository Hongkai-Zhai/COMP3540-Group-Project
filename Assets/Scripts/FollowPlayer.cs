using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public float height = 10f;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -5f);

    private void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target is not set!");
            return;
        }

        transform.rotation = Quaternion.Euler(90, 0, 0);
        transform.position = target.position + Vector3.up * height + offset;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + Vector3.up * height + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}