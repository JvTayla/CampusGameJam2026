using UnityEngine;

// Put this on your Main Camera (or player object if camera is a child).
// It casts a ray forward every frame, checks if it hit something Interactable,
// and shows a prompt / listens for E key.
public class PlayerInteractor : MonoBehaviour
{
    public float interactRange = 3f;
    public LayerMask interactableLayer = ~0; // defaults to "everything", narrow this later if needed
    public GameObject promptUI; // a small world-space or screen-space "Press E" text object, assign in Inspector

    private Interactable currentTarget;

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

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                currentTarget = interactable;
                if (promptUI != null) promptUI.SetActive(true);
                return;
            }
        }

        // nothing hit, or hit something without Interactable
        currentTarget = null;
        if (promptUI != null) promptUI.SetActive(false);
    }
}
