using UnityEngine;

public class Interaction : MonoBehaviour
{
    IInteractable interactable;
    public bool hasExplosive = false;
    public bool hasCompletOBJ=false;
    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject+"col");

        if(other.gameObject.GetComponent<IInteractable>() != null)
        {
            interactable = other.gameObject.GetComponent<IInteractable>();
        }
        if(other.gameObject.GetComponent<Explo>()!= null)
        {
            Destroy(other);
            hasExplosive = true;
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
