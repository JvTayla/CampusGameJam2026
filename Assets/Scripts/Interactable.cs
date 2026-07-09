using UnityEngine;
using UnityEngine.Events;

// Attach to ANY object the player should be able to interact with
// (valves, dials, mirrors, doors, notes, items etc).
// Needs a MeshFilter + MeshRenderer on the SAME object (or set meshSource manually)
// for the outline to auto-generate.
[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    [Tooltip("Shown in the prompt UI, e.g. 'Turn valve' or 'Read note'")]
    public string promptText = "Interact";

    [Tooltip("Set false to temporarily disable this object (e.g. already solved)")]
    public bool isInteractable = true;

    [Header("Outline")]
    public bool showOutline = true;
    public Material outlineMaterial; // assign an instance of Custom/OutlineGlow here
    [Tooltip("If left empty, uses this object's own MeshFilter")]
    public MeshFilter meshSource;

    public UnityEvent onInteract;

    private GameObject outlineObject;
    private MeshRenderer outlineRenderer;

    void Awake()
    {
        if (showOutline)
            BuildOutlineMesh();
    }

    void BuildOutlineMesh()
    {
        if (meshSource == null)
            meshSource = GetComponent<MeshFilter>();

        if (meshSource == null || outlineMaterial == null)
        {
            showOutline = false; // no mesh or no material assigned, skip silently
            return;
        }

        outlineObject = new GameObject("Outline");
        outlineObject.transform.SetParent(transform, false);
        outlineObject.transform.localPosition = Vector3.zero;
        outlineObject.transform.localRotation = Quaternion.identity;
        outlineObject.transform.localScale = Vector3.one;

        MeshFilter mf = outlineObject.AddComponent<MeshFilter>();
        mf.sharedMesh = meshSource.sharedMesh;

        outlineRenderer = outlineObject.AddComponent<MeshRenderer>();
        outlineRenderer.sharedMaterial = outlineMaterial;
        outlineObject.SetActive(false); // hidden until player looks at it
    }

    public void TriggerInteract()
    {
        if (!isInteractable) return;
        onInteract?.Invoke();
    }

    public void SetHighlight(bool active)
    {
        if (!showOutline || outlineObject == null) return;
        outlineObject.SetActive(active);
    }
}