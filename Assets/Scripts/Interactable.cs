using UnityEngine;
using UnityEngine.Events;

// Attach to ANY object the player should be able to interact with
// (valves, dials, mirrors, doors, notes, items etc).
// Needs a MeshFilter + MeshRenderer on the SAME object (or set meshSource manually)
// for the outline to auto-generate.
//
// Requires a PlayerReference component on the player object for proximity glow to work.
// Requires a PlayerInteractor component on the player/camera to drive SetHighlight()
// when the player looks directly at this object.
[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    [Tooltip("Shown in the prompt UI, e.g. 'Turn valve' or 'Read note'")]
    public string promptText = "Interact";
    [Tooltip("Set false to temporarily disable this object (e.g. already solved)")]
    public bool isInteractable = true;

    [Header("Outline")]
    public bool showOutline = true;
    [Tooltip("Assign an instance of your outline shader material here")]
    public Material outlineMaterial;
    [Tooltip("If left empty, uses this object's own MeshFilter")]
    public MeshFilter meshSource;
    public Color outlineColor = Color.red;
    [Tooltip("How much bigger than the original mesh the outline copy is. 1.0 = same size (invisible). Try 1.02-1.1.")]
    public float outlineScale = 1.05f;

    [Header("Proximity Glow")]
    [Tooltip("Distance at which the object starts glowing softly, before the player is even looking at it")]
    public float glowRange = 5f;
    [Tooltip("Outline alpha/intensity when just in range (not looked at)")]
    [Range(0f, 1f)] public float proximityGlowStrength = 0.3f;
    [Tooltip("Outline alpha/intensity when actively looked at")]
    [Range(0f, 1f)] public float focusGlowStrength = 1f;

    [Header("Debug")]
    public bool debugLogging = false;

    public UnityEvent onInteract;

    private GameObject outlineObject;
    private MeshRenderer outlineRenderer;
    private bool isInProximity = false;
    private bool isFocused = false;

    void Awake()
    {
        if (showOutline)
            BuildOutlineMesh();
    }

    void Update()
    {
        if (!showOutline || outlineObject == null || PlayerReference.Instance == null) return;

        float dist = Vector3.Distance(transform.position, PlayerReference.Instance.position);
        bool nowInProximity = dist <= glowRange;

        if (nowInProximity != isInProximity)
        {
            isInProximity = nowInProximity;
            RefreshOutlineState();
        }
    }

    void BuildOutlineMesh()
    {
        if (meshSource == null)
            meshSource = GetComponent<MeshFilter>();

        if (meshSource == null || outlineMaterial == null)
        {
            showOutline = false; // no mesh or no material assigned, skip silently
            if (debugLogging)
                Debug.LogWarning($"[{name}] Outline disabled - meshSource: {(meshSource != null ? "OK" : "MISSING")}, outlineMaterial: {(outlineMaterial != null ? "OK" : "MISSING")}");
            return;
        }

        outlineObject = new GameObject("Outline");
        outlineObject.transform.SetParent(transform, false);
        outlineObject.transform.localPosition = Vector3.zero;
        outlineObject.transform.localRotation = Quaternion.identity;
        // Slightly larger than the original mesh so it pokes out as a visible rim.
        // Needs a shader that culls FRONT faces (renders only the inside surface)
        // for this to read as an outline rather than a bigger solid copy.
        outlineObject.transform.localScale = Vector3.one * outlineScale;

        MeshFilter mf = outlineObject.AddComponent<MeshFilter>();
        mf.sharedMesh = meshSource.sharedMesh;

        outlineRenderer = outlineObject.AddComponent<MeshRenderer>();
        outlineRenderer.material = outlineMaterial; // instance copy, not shared, so each object can tint independently
        outlineRenderer.material.color = outlineColor;

        outlineObject.SetActive(false); // hidden until player is in range or looking at it

        if (debugLogging)
            Debug.Log($"[{name}] Outline mesh built successfully. Shader: {outlineRenderer.material.shader.name}, initial color: {outlineRenderer.material.color}");
    }

    // Called by PlayerInteractor when the player's look-raycast hits/leaves this object
    public void SetHighlight(bool active)
    {
        isFocused = active;
        RefreshOutlineState();
    }

    void RefreshOutlineState()
    {
        if (!showOutline || outlineObject == null) return;

        bool shouldShow = isFocused || isInProximity;
        outlineObject.SetActive(shouldShow);

        if (!shouldShow)
        {
            if (debugLogging) Debug.Log($"[{name}] Outline hidden (focused: {isFocused}, inProximity: {isInProximity})");
            return;
        }

        float strength = isFocused ? focusGlowStrength : proximityGlowStrength;
        Color c = outlineColor;
        c.a = strength;
        outlineRenderer.material.color = c;

        if (debugLogging)
            Debug.Log($"[{name}] Outline ON - focused: {isFocused}, inProximity: {isInProximity}, strength: {strength}, color: {c}, renderer enabled: {outlineRenderer.enabled}, GO active: {outlineObject.activeSelf}");
    }

    public void TriggerInteract()
    {
        if (!isInteractable) return;
        onInteract?.Invoke();
    }
}