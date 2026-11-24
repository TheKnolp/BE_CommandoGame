using UnityEngine;
using UnityEngine.SceneManagement;

public class Explo : MonoBehaviour, IInteractable
{
    public bool CanInteract(GameObject interactor)
    {
        return true;
    }

    public void Interact(GameObject interactor)
    {
        print("Has grab Explosive");
    }
}
