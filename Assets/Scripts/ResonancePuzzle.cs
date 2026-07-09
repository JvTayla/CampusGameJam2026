using UnityEngine;
using System.Collections;

// Put this on an empty GameObject in the wiring room.
// Hook a UI Slider's "On Value Changed" event to call OnDialChanged(float).
public class ResonancePuzzle : PuzzleBase
{
    [Header("Audio")]
    public AudioSource hum;           // clean tone, loops
    public AudioSource staticNoise;   // distorted/screech layer, loops, starts at volume 0

    [Header("Tuning")]
    [Range(0f, 1f)] public float targetFrequency = 0.75f;
    public float tolerance = 0.03f;
    public float holdTimeRequired = 1.5f;

    private float currentValue = 0f;
    private Coroutine holdRoutine;

    // Call this from the Slider's OnValueChanged(float) event in the Inspector
    public void OnDialChanged(float sliderValue)
    {
        if (IsSolved) return;

        currentValue = sliderValue;

        // map slider 0-1 to a pitch range - tweak 0.5/2f to taste
        hum.pitch = Mathf.Lerp(0.5f, 2f, currentValue);

        float diff = Mathf.Abs(currentValue - targetFrequency);

        // static gets louder the further off you are - clean silence when correct
        if (staticNoise != null)
            staticNoise.volume = Mathf.Clamp01(diff * 6f);

        if (diff <= tolerance)
        {
            if (holdRoutine == null)
                holdRoutine = StartCoroutine(HoldToConfirm());
        }
        else
        {
            if (holdRoutine != null)
            {
                StopCoroutine(holdRoutine);
                holdRoutine = null;
            }
        }
    }

    IEnumerator HoldToConfirm()
    {
        yield return new WaitForSeconds(holdTimeRequired);

        // re-check in case they drifted off target during the wait
        float diff = Mathf.Abs(currentValue - targetFrequency);
        if (diff <= tolerance)
        {
            if (staticNoise != null) staticNoise.volume = 0f;
            Solve();
        }

        holdRoutine = null;
    }
}
