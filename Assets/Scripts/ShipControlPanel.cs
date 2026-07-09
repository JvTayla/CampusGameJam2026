using UnityEngine;
using UnityEngine.Events;

// Put this on an empty GameObject at the ship control panel/bridge.
// Drag in the 3 puzzle managers from your 3 rooms.
public class ShipControlPanel : MonoBehaviour
{
    public PuzzleBase powerPuzzle;    // boiler room
    public PuzzleBase wiringPuzzle;   // resonance room
    public PuzzleBase signalPuzzle;   // mirror room

    public UnityEvent OnAllSystemsOnline; // hook the ending sequence here

    private bool triggered = false;

    void Update()
    {
        if (triggered) return;

        if (powerPuzzle.IsSolved && wiringPuzzle.IsSolved && signalPuzzle.IsSolved)
        {
            triggered = true;
            OnAllSystemsOnline?.Invoke();
        }
    }
}
