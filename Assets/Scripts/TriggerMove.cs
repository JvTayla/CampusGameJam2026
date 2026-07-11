using UnityEngine;

public class TriggerMove : MonoBehaviour
{

    [SerializeField] private Transform destination;
    [SerializeField] private string playerTag = "Player";

    [Header("Optional")]
    [SerializeField] private bool matchRotation = false;
    [SerializeField] private AudioClip teleportSound;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (destination == null)
        {
            Debug.LogWarning($"{name}: No destination assigned.");
            return;
        }

        Rigidbody rb = other.attachedRigidbody;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // Unity 6+ naming (was rb.velocity pre-6)
            rb.angularVelocity = Vector3.zero;
        }

        other.transform.position = destination.position;
        if (matchRotation)
            other.transform.rotation = destination.rotation;

        if (teleportSound != null)
            AudioSource.PlayClipAtPoint(teleportSound, destination.position);
    }
}

