using UnityEngine;

// Put this on the player root (whatever object actually moves around the ship).
// Lets any Interactable check "how far am I from the player" without needing
// a manual drag-and-drop reference on every single object, and without
// expensive FindObjectOfType calls every frame.
public class PlayerReference : MonoBehaviour
{
    public static Transform Instance;

    void Awake()
    {
        Instance = transform;
    }
}