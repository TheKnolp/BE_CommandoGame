using UnityEngine;

public class Sub : MonoBehaviour, IInteractable
{
    public bool CanInteract(GameObject interactor)
    {
        if(interactor.GetComponent<Interaction>().hasExplosive)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Interact(GameObject interactor)
    {
        print("HasCompletObjective");
        interactor.GetComponent<Interaction>().hasCompletOBJ = true;
    }
}
