using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerInteract : MonoBehaviour
{

    [Header("Raycast Settings")]
    public float interactRange = 3f;
    public LayerMask interactableLayer = ~0; // "everything" by default

    [Header("UI")]
    public GameObject promptRoot;   // parent object of your prompt UI, toggled on/off
    public TMP_Text promptLabel;    // TextMeshProUGUI (for canvas) or TextMeshPro (for world space) both work here

    private Interactable currentTarget;
    private Interactable previousTarget;

    void Start()
    {
        if (promptRoot != null) promptRoot.SetActive(false);
    }

    void Update()
    {
        CheckForInteractable();

        if (currentTarget != null && Input.GetKeyDown(KeyCode.E))
        {
            currentTarget.TriggerInteract();
        }
    }

    void CheckForInteractable()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Interactable hitInteractable = null;

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            hitInteractable = hit.collider.GetComponent<Interactable>();
        }

        // target changed since last frame - update highlight + prompt
        if (hitInteractable != previousTarget)
        {
            if (previousTarget != null)
                previousTarget.SetHighlight(false);

            if (hitInteractable != null)
                hitInteractable.SetHighlight(true);

            previousTarget = hitInteractable;
        }

        currentTarget = (hitInteractable != null && hitInteractable.isInteractable) ? hitInteractable : null;

        if (promptRoot != null)
        {
            promptRoot.SetActive(currentTarget != null);
            if (currentTarget != null && promptLabel != null)
                promptLabel.text = "Press E to " + currentTarget.promptText;
        }
    }

    // Optional - draws the interaction ray in Scene view so you can debug range/aim easily
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * interactRange);
    }
}