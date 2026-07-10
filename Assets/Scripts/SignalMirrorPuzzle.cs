using System.Collections.Generic;
using UnityEngine;

public class SignalMirrorPuzzle : PuzzleBase
{
    [Header("Beam")]
    public Transform lightSource; // beam fires along lightSource.forward
    public Transform targetSensor;
    public LineRenderer beam;
    public float maxBeamDistance = 50f;
    public float raycastSkin = 0.1f;
    [Tooltip("Safety cap on bounces, in case mirrors are angled into a loop")]
    public int maxBounces = 8;

    [Header("Audio")]
    public AudioSource lockedSound;

    void Update()
    {
        if (!IsSolved) CastBeam();
    }

    void CastBeam()
    {
        List<Vector3> points = new List<Vector3> { lightSource.position };
        Vector3 origin = lightSource.position;
        Vector3 direction = lightSource.forward;
        bool solved = false;

        for (int bounce = 0; bounce <= maxBounces; bounce++)
        {
            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxBeamDistance))
            {
                points.Add(hit.point);

                RotatableMirror mirrorHit = hit.transform.GetComponent<RotatableMirror>();
                if (mirrorHit != null)
                {
                    direction = Vector3.Reflect(direction, mirrorHit.WorldNormal);
                    origin = hit.point + direction * raycastSkin;
                    continue; // keep bouncing
                }

                if (hit.transform == targetSensor) solved = true;
                break; // hit something solid that isn't a mirror - beam stops
            }
            else
            {
                points.Add(origin + direction * maxBeamDistance);
                break;
            }
        }

        beam.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
            beam.SetPosition(i, points[i]);

        if (solved && !IsSolved)
        {
            if (lockedSound != null) lockedSound.Play();
            Solve();
        }
    }
}