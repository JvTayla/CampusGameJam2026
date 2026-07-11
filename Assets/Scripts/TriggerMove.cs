using UnityEngine;

public class TriggerMove : MonoBehaviour
{
   
    [SerializeField] private Transform destination;
    [SerializeField] private string playerTag = "Player";

    [Header("Optional")]
    [SerializeField] private bool matchRotation = false;
    [SerializeField] private AudioClip teleportSound;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        Teleport(other.gameObject);
    }

    private void Teleport(GameObject player)
    {
        if (destination == null)
        {
            Debug.LogWarning($"{name}: No destination assigned.");
            return;
        }

        // If using a CharacterController, disable it briefly so it doesn't
        // fight the teleport (it caches position internally each frame)
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = destination.position;
        if (matchRotation)
            player.transform.rotation = destination.rotation;

        if (cc != null) cc.enabled = true;

        if (teleportSound != null)
            AudioSource.PlayClipAtPoint(teleportSound, destination.position);
    }
}

