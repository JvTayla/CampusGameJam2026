using UnityEngine;

// Attach this to ANY object the player should be able to interact with
// (valves, dials, mirrors, doors, notes etc).
// It just fires an event - what happens on interact is defined per-object
// via the UnityEvent in the Inspector, OR by other scripts calling OnInteract() directly.
public class Interactable : MonoBehaviour
{
    [TextArea] public string promptText = "Press E to interact";
    public UnityEngine.Events.UnityEvent onInteract;

    public void TriggerInteract()
    {
        onInteract?.Invoke();
    }
}
