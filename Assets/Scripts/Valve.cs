using UnityEngine;

// Attach to each valve object. Needs an Interactable on the same object
// (hook its onInteract event to call Turn() below).
public class Valve : MonoBehaviour
{
    public int currentPosition = 0; // 0-3, starts wrong on purpose
    public int correctPosition;     // set this per-valve in Inspector
    private float[] angles = { 0f, 90f, 180f, 270f };

    public AudioSource turnSound; // optional, drag an echoey clank/hiss clip in

    private PipePuzzleManager manager;

    void Start()
    {
        // finds the manager automatically as long as it's in the same scene
        manager = FindObjectOfType<PipePuzzleManager>();
        transform.localEulerAngles = new Vector3(0, 0, angles[currentPosition]);
    }

    // Call this from the Valve's Interactable -> onInteract event in the Inspector
    public void Turn()
    {
        currentPosition = (currentPosition + 1) % 4;
        transform.localEulerAngles = new Vector3(0, 0, angles[currentPosition]);

        if (turnSound != null) turnSound.Play();

        manager.CheckValve(this);
    }

    public bool IsCorrect => currentPosition == correctPosition;
}
