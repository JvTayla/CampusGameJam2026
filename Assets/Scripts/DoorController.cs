using UnityEngine;

// Simplest possible door: two separate objects (closed model, open model)
// occupying the same spot. Opening the door just swaps which one is active.
// Hook this to a PuzzleBase's "On Puzzle Solved" event, or an Interactable's
// "On Interact" event if the door itself is what the player clicks/presses E on.
public class DoorController : MonoBehaviour
{
    [Header("Door States")]
    public GameObject closedDoor;
    public GameObject openDoor;

    [Header("Optional")]
    public AudioSource openSound; // creaky door hinge, drag a clip in
    private bool isOpen = false;

    void Start()
    {
        // make sure it starts in the correct state regardless of what's
        // active in the editor when you hit Play
        SetDoorState(isOpen);
    }

    // Call this from a puzzle's "On Puzzle Solved" event, or an Interactable
    public void OpenDoor()
    {
        if (isOpen) return; // already open, don't re-trigger sound etc

        isOpen = true;
        SetDoorState(true);

        if (openSound != null)
            openSound.Play();
    }

    // Optional - in case you ever want a door that can be closed again
    public void CloseDoor()
    {
        isOpen = false;
        SetDoorState(false);
    }

    void SetDoorState(bool open)
    {
        if (closedDoor != null) closedDoor.SetActive(!open);
        if (openDoor != null) openDoor.SetActive(open);
    }
}