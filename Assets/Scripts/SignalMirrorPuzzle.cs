using UnityEngine;

// Put this on an empty GameObject in the mirror room.
// Assign the mirror (rotating object), the light source point, the target sensor,
// and a LineRenderer with 3 points for the visible beam.
public class SignalMirrorPuzzle : PuzzleBase
{
    [Header("Mirror Object")]
    public Transform mirror;

    [Tooltip("The mirror's resting pose before any player rotation is applied. " +
             "Set this by eye in the Scene view, then copy the values here - " +
             "same pattern as the valve script.")]
    public Vector3 baseRotationOffset = Vector3.zero;

    [Tooltip("Which local axis the player actually rotates when using the mirror. " +
             "Usually (0,1,0) for a mirror that swivels left/right like a lazy susan.")]
    public Vector3 rotationAxis = Vector3.up;

    [Tooltip("Which local axis of the mirror counts as its reflective face/normal. " +
             "This determines how the beam bounces off it. Test in Play mode and adjust if the beam behaves oddly.")]
    public Vector3 normalAxisLocal = Vector3.up;

    private float currentRotation = 0f; // accumulated player input rotation, added on top of base pose

    [Header("Beam")]
    public Transform lightSource;
    public Transform targetSensor;
    public LineRenderer beam;
    public float rotationSpeed = 50f;
    public float maxBeamDistance = 50f;
    [Tooltip("Small offset so the reflected ray doesn't immediately hit the mirror's own collider")]
    public float raycastSkin = 0.1f;

    [Header("Audio")]
    public AudioSource lockedSound; // plays once, right when the puzzle solves

    [Header("Debug (read-only, just for watching state while testing)")]
    [SerializeField] private bool isFocused = false; // only rotate while player is actively using it

    void Start()
    {
        ApplyRotation();
    }

    void Update()
    {
        if (isFocused && !IsSolved)
        {
            float input = 0f;
            if (Input.GetKey(KeyCode.LeftArrow)) input = -1f;
            else if (Input.GetKey(KeyCode.RightArrow)) input = 1f;

            currentRotation += input * rotationSpeed * Time.deltaTime;
            ApplyRotation();
        }

        CastBeam();
    }

    void ApplyRotation()
    {
        Quaternion baseRot = Quaternion.Euler(baseRotationOffset);
        Quaternion spin = Quaternion.AngleAxis(currentRotation, rotationAxis);
        mirror.localRotation = baseRot * spin;
    }

    void CastBeam()
    {
        Vector3 mirrorNormal = mirror.TransformDirection(normalAxisLocal.normalized);
        Vector3 dirToMirror = (mirror.position - lightSource.position).normalized;
        Vector3 reflected = Vector3.Reflect(dirToMirror, mirrorNormal);

        // start the reflected raycast slightly off the mirror's surface,
        // otherwise it immediately re-hits the mirror's own collider at distance ~0
        Vector3 reflectStart = mirror.position + reflected * raycastSkin;

        beam.positionCount = 3;
        beam.SetPosition(0, lightSource.position);
        beam.SetPosition(1, mirror.position);

        if (Physics.Raycast(reflectStart, reflected, out RaycastHit hit, maxBeamDistance))
        {
            beam.SetPosition(2, hit.point);

            if (!IsSolved && hit.transform == targetSensor)
            {
                if (lockedSound != null) lockedSound.Play();
                Solve();
            }
        }
        else
        {
            beam.SetPosition(2, mirror.position + reflected * maxBeamDistance);
        }
    }

    // Call this from an Interactable's onInteract event to start controlling the mirror
    public void EnterFocus()
    {
        isFocused = true;
    }

    // Call this to stop rotating (Escape key, walking away, or interacting again)
    public void ExitFocus()
    {
        isFocused = false;
    }

    // Use this instead of separate Enter/Exit if you want a single "Press E" to both start and stop
    public void ToggleFocus()
    {
        isFocused = !isFocused;
    }
}