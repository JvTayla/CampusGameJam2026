using UnityEngine;

// Put this on each target zone (collision volume) in the "correct" reflected room.
// Only accepts the specific object it's expecting, then locks it in place and
// notifies the manager that this slot has been filled.
public class CaptainsRoomSlot : MonoBehaviour
{
    [Tooltip("Which object belongs in this slot. Must match PushableObject.objectID.")]
    public string expectedObjectID;

    [Tooltip("Assign the CaptainsRoomManager for this room.")]
    public CaptainsRoomManager manager;

    [Header("Debug")]
    public bool debugLogging = false;

    private bool isFilled = false;
    private Transform currentOccupant;

    private void OnCollisionEnter(Collision collision)
    {
        if (debugLogging)
            Debug.Log($"[{name}] Collision entered by: {collision.transform.name}");

        // Ignore if already filled
        if (isFilled)
            return;

        // Check if the object has a PushableObject component
        PushableObject pushable = collision.collider.GetComponent<PushableObject>();
        if (pushable == null)
        {
            if (debugLogging)
                Debug.Log($"[{name}] {collision.transform.name} has no PushableObject script - ignoring");
            return;
        }

        // Check if it's the correct object
        if (pushable.objectID != expectedObjectID)
        {
            if (debugLogging)
                Debug.Log($"[{name}] Wrong object - expected '{expectedObjectID}', got '{pushable.objectID}'");
            return;
        }

        // Snap object into place
        collision.transform.position = transform.position;
        collision.transform.rotation = transform.rotation;

        // Lock physics
        if (collision.rigidbody != null)
        {
            collision.rigidbody.linearVelocity = Vector3.zero;
            collision.rigidbody.angularVelocity = Vector3.zero;
            collision.rigidbody.isKinematic = true;
        }

        currentOccupant = collision.transform;
        isFilled = true;

        // Disable this slot's collider so it can't be triggered again
        Collider slotCollider = GetComponent<Collider>();
        if (slotCollider != null)
        {
            slotCollider.enabled = false;
        }

        if (debugLogging)
            Debug.Log($"[{name}] '{expectedObjectID}' correctly placed and locked.");

        manager?.NotifySlotFilled();

        if (debugLogging && manager == null)
            Debug.LogWarning($"[{name}] No manager assigned!");
    }
}