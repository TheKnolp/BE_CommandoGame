using UnityEngine;

public class Interaction : MonoBehaviour
{
    IInteractable interactable;
    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject+"col");

        if(other.gameObject.GetComponent<IInteractable>() != null)
        {
            interactable = other.gameObject.GetComponent<IInteractable>();
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && interactable.CanInteract(this.gameObject))
        {
            interactable.Interact(this.gameObject);
        }
    }
}
