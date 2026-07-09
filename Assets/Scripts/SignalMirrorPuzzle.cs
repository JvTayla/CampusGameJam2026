using UnityEngine;

// Put this on an empty GameObject in the mirror room.
// Assign the mirror (rotating object), the light source point, the target sensor,
// and a LineRenderer with 3 points for the visible beam.
public class SignalMirrorPuzzle : PuzzleBase
{
    public Transform mirror;
    public Transform lightSource;
    public Transform targetSensor;
    public LineRenderer beam;
    public float rotationSpeed = 50f;
    public float maxBeamDistance = 50f;

    private bool isFocused = false; // only rotate the mirror while player is actively using it

    void Update()
    {
        if (isFocused)
        {
            float input = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
            mirror.Rotate(Vector3.up, input * rotationSpeed * Time.deltaTime);
        }

        CastBeam();
    }

    void CastBeam()
    {
        Vector3 dirToMirror = (mirror.position - lightSource.position).normalized;
        Vector3 reflected = Vector3.Reflect(dirToMirror, mirror.up);

        beam.positionCount = 3;
        beam.SetPosition(0, lightSource.position);
        beam.SetPosition(1, mirror.position);

        if (Physics.Raycast(mirror.position, reflected, out RaycastHit hit, maxBeamDistance))
        {
            beam.SetPosition(2, hit.point);

            if (!IsSolved && hit.transform == targetSensor)
                Solve();
        }
        else
        {
            beam.SetPosition(2, mirror.position + reflected * maxBeamDistance);
        }
    }

    // Call this from an Interactable's onInteract event to "enter" the mirror control
    public void EnterFocus()
    {
        isFocused = true;
    }

    // Call this from an Interactable, or bind to Escape key elsewhere, to "exit"
    public void ExitFocus()
    {
        isFocused = false;
    }
}
