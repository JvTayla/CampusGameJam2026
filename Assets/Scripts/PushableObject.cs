using UnityEngine;

// Put this on every movable object in the captain's room (chair, drawer, box, etc).
// The objectID must be unique per object and must match the expectedObjectID
// on whichever CaptainsRoomSlot it's supposed to end up in.
public class PushableObject : MonoBehaviour
{
    [Tooltip("Unique ID for this object, e.g. 'Chair', 'DeskDrawer', 'Globe'. Must match a slot's expectedObjectID.")]
    public string objectID;
}
