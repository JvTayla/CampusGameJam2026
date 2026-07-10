using UnityEngine;
using UnityEngine.Events;

// Put this on an empty GameObject in the captain's room.
// Assign all the CaptainsRoomSlot objects to slotsToWatch, and each slot's
// "manager" field back to this object.
public class CaptainsRoomManager : MonoBehaviour
{
    [Tooltip("How many slots need to be filled for the puzzle to solve")]
    public int totalSlots;

    [Header("Reward")]
    [Tooltip("Fires once, the moment the last slot is correctly filled. Reveal the post-it note, play a sound, unlock the safe, etc.")]
    public UnityEvent onPuzzleSolved;
    [Tooltip("Optional: fires if a previously-solved puzzle gets un-solved by a slot being emptied again")]
    public UnityEvent onPuzzleUnsolved;

    private int filledCount = 0;
    private bool isSolved = false;

    [Header("Debug")]
    public bool debugLogging = false;

    public void NotifySlotFilled()
    {
        filledCount++;
        if (debugLogging)
            Debug.Log($"[{name}] Slot filled. Count: {filledCount}/{totalSlots}");
        CheckSolved();
    }

    public void NotifySlotEmptied()
    {
        filledCount--;
        if (debugLogging)
            Debug.Log($"[{name}] Slot emptied. Count: {filledCount}/{totalSlots}");
        if (isSolved)
        {
            isSolved = false;
            if (debugLogging)
                Debug.Log($"[{name}] Puzzle UN-solved");
            onPuzzleUnsolved?.Invoke();
        }
    }

    void CheckSolved()
    {
        if (isSolved) return; // already solved, don't fire again
        if (filledCount >= totalSlots)
        {
            isSolved = true;
            if (debugLogging)
                Debug.Log($"[{name}] Puzzle SOLVED - all {totalSlots} slots correctly filled");
            onPuzzleSolved?.Invoke();
        }
    }
}