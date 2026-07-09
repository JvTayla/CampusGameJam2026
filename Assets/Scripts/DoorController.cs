using UnityEngine;

// Simplest possible door: two separate objects (closed model, open model)
// occupying the same spot. Opening/closing just swaps which one is active.
// Hook Toggle() to an Interactable's "On Interact" event if the player should
// be able to open AND close it themselves (like a switch/lever).
// Or hook OpenDoor() specifically to a puzzle's "On Puzzle Solved" event
// if it should only ever open once and stay that way.
public class DoorController : MonoBehaviour
{
    [Header("Door States")]
    public GameObject closedDoor;
    public GameObject openDoor;

    [Header("Optional")]
    public AudioSource openSound;  // creaky door hinge opening
    public AudioSource closeSound; // door swinging shut / latching

    private bool isOpen = false;

    void Start()
    {
        // make sure it starts in the correct state regardless of what's
        // active in the editor when you hit Play
        SetDoorState(isOpen);
    }

    // Call this from an Interactable's "On Interact" event to use it as a switch
    public void Toggle()
    {
        if (isOpen)
            CloseDoor();
        else
            OpenDoor();
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

    // Call this directly if you want a dedicated "close" trigger somewhere
    public void CloseDoor()
    {
        if (!isOpen) return; // already closed, don't re-trigger sound etc

        isOpen = false;
        SetDoorState(false);

        if (closeSound != null)
            closeSound.Play();
    }

    void SetDoorState(bool open)
    {
        if (closedDoor != null) closedDoor.SetActive(!open);
        if (openDoor != null) openDoor.SetActive(open);
    }
}