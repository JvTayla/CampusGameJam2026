using UnityEngine;
using UnityEngine.Events;

// Every puzzle in the game inherits from this.
// Drag the "OnPuzzleSolved" event in the Inspector to trigger doors, lights, sounds etc.
public abstract class PuzzleBase : MonoBehaviour
{
    public bool IsSolved { get; protected set; }
    public UnityEvent OnPuzzleSolved;

    protected virtual void Solve()
    {
        if (IsSolved) return; // stop it firing more than once
        IsSolved = true;
        OnPuzzleSolved?.Invoke();
        Debug.Log(gameObject.name + " puzzle solved!");
    }
}
