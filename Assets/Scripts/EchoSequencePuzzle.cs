using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Put this on an empty GameObject in the boiler room (e.g. "BoilerPuzzleManager").
// Drag all Valve objects into the valves array, and set the solution order
// in Correct Order (using each valve's valveID, e.g. {2, 0, 1}).
public class EchoSequencePuzzle : PuzzleBase
{
    [Header("Setup")]
    public Valve[] valves;
    public int[] correctOrder; // e.g. {2, 0, 1} - the valveIDs in the order they must be turned

    [Header("Audio")]
    public AudioSource successSound;
    public float echoGap = 0.5f; // delay between each echoed note on a failed attempt
    public float echoDelayBeforeReset = 0.4f; // pause before the echo playback starts

    private List<int> playerInput = new List<int>();
    private bool isPlayingEcho = false;

    // Called by a Valve when the player interacts with it
    public void OnValveActivated(Valve v)
    {
        if (IsSolved || isPlayingEcho) return; // ignore input during solved/echo-playback states

        playerInput.Add(v.valveID);
        int i = playerInput.Count - 1;

        if (playerInput[i] != correctOrder[i])
        {
            StartCoroutine(FailAndEcho());
            return;
        }

        if (playerInput.Count == correctOrder.Length)
        {
            if (successSound != null) successSound.Play();
            Solve();
        }
    }

    IEnumerator FailAndEcho()
    {
        isPlayingEcho = true;

        // capture what the player actually played before we clear it
        List<int> attemptedSequence = new List<int>(playerInput);
        playerInput.Clear();

        yield return new WaitForSeconds(echoDelayBeforeReset);

        // play back the player's own wrong attempt, like the ship is echoing it back
        foreach (int valveID in attemptedSequence)
        {
            Valve matchingValve = System.Array.Find(valves, v => v.valveID == valveID);
            if (matchingValve != null && matchingValve.tone != null)
                matchingValve.tone.Play();

            yield return new WaitForSeconds(echoGap);
        }

        // reset all valves back to their starting rotation
        foreach (var valve in valves)
            valve.ResetValve();

        isPlayingEcho = false;
    }
}
