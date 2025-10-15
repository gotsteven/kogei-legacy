using UnityEngine;

public class PlayerInteractive : MonoBehaviour
{
    private InteractableZone currentInteractableZone; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (currentInteractableZone != null)
            {
                currentInteractableZone.Interact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        InteractableZone zone = other.GetComponent<InteractableZone>();
        if (zone != null)
        {
            currentInteractableZone = zone;
            Debug.Log("インタラクション可能なエリアに入りました。");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        InteractableZone zone = other.GetComponent<InteractableZone>();
        if (zone != null && zone == currentInteractableZone)
        {
            currentInteractableZone = null;
            Debug.Log("インタラクション可能なエリアから出ました。");
        }
    }
}