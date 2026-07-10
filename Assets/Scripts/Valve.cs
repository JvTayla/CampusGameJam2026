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
    [Tooltip("The valve's correct 'facing forward' orientation before any spin is applied. " +
             "Set this in the Inspector by rotating the object in the Scene view until it looks right, " +
             "THEN copy those X/Y/Z values in here.")]
    public Vector3 baseRotationOffset = new Vector3(90f, 0f, 90f);

    [Tooltip("Which local axis the valve actually spins on when turned (usually just one of these is 1, rest 0)")]
    public Vector3 spinAxis = new Vector3(1, 0, 0); // defaults to X, change to (0,1,0) or (0,0,1) if needed

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
        // start from the base orientation (the "correct looking" pose you set in the Inspector),
        // then add the spin on top of it, rather than overwriting rotation from scratch
        Quaternion baseRot = Quaternion.Euler(baseRotationOffset);
        Quaternion spin = Quaternion.Euler(spinAxis * angles[currentPosition]);
        transform.localRotation = baseRot * spin;
    }
}