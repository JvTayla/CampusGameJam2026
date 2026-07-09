using UnityEngine;

// Attach to each valve object. Needs an Interactable on the same object
// (hook its onInteract event to call Turn() below).
// Each valve has its own distinct tone - this is its "voice" in the sequence.
public class Valve : MonoBehaviour
{
    [Header("Identity")]
    [Tooltip("Unique index for this valve - must match its position in the manager's Correct Order array")]
    public int valveID;

    [Header("Rotation (visual feedback only, not part of the solve logic)")]
    public int currentPosition = 0; // 0-3
    private float[] angles = { 0f, 90f, 180f, 270f };

    [Header("Audio")]
    [Tooltip("This valve's unique tone - plays every time it's turned")]
    public AudioSource tone;

    private EchoSequencePuzzle manager;

    void Start()
    {
        manager = FindObjectOfType<EchoSequencePuzzle>();
        ApplyRotation();
    }

    // Call this from the Valve's Interactable -> onInteract event in the Inspector
    public void Turn()
    {
        currentPosition = (currentPosition + 1) % 4;
        ApplyRotation();

        if (tone != null) tone.Play();

        manager.OnValveActivated(this);
    }

    // Used by the manager to snap this valve back to 0 on a failed attempt
    public void ResetValve()
    {
        currentPosition = 0;
        ApplyRotation();
    }

    void ApplyRotation()
    {
        transform.localEulerAngles = new Vector3(0, 0, angles[currentPosition]);
    }
}