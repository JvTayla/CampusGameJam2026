using UnityEngine;


[RequireComponent(typeof(Collider))]
public class RotatableMirror : MonoBehaviour
{
    [Tooltip("The mirror's resting pose before any player rotation is applied.")]
    public Vector3 baseRotationOffset = Vector3.zero;
    [Tooltip("Which local axis the player rotates when using this mirror.")]
    public Vector3 rotationAxis = Vector3.up;
    [Tooltip("Which local axis counts as the reflective face/normal.")]
    public Vector3 normalAxisLocal = Vector3.up;
    public float rotationSpeed = 50f;

    private float currentRotation = 0f;
    private bool isFocused = false;

    void Start()
    {
        ApplyRotation();
    }

    void Update()
    {
        if (!isFocused) return;
        float input = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) input = -1f;
        else if (Input.GetKey(KeyCode.RightArrow)) input = 1f;
        currentRotation += input * rotationSpeed * Time.deltaTime;
        ApplyRotation();
    }

    void ApplyRotation()
    {
        Quaternion baseRot = Quaternion.Euler(baseRotationOffset);
        Quaternion spin = Quaternion.AngleAxis(currentRotation, rotationAxis);
        transform.localRotation = baseRot * spin;
    }

    public Vector3 WorldNormal => transform.TransformDirection(normalAxisLocal.normalized);

    public void EnterFocus() => isFocused = true;
    public void ExitFocus() => isFocused = false;
    public void ToggleFocus() => isFocused = !isFocused;
}