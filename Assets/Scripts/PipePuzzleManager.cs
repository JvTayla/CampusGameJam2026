using UnityEngine;

// Put this on an empty GameObject in the boiler room (e.g. "BoilerPuzzleManager").
// Drag all Valve objects into the valves array in the Inspector.
public class PipePuzzleManager : PuzzleBase
{
    public Valve[] valves;

    public void CheckValve(Valve v)
    {
        foreach (var valve in valves)
        {
            if (!valve.IsCorrect) return; // at least one still wrong, stop here
        }

        Solve(); // all valves correct - fires OnPuzzleSolved (lights, rumble, door etc)
    }
}
