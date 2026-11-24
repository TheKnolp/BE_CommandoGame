using UnityEngine;
using UnityEngine.SceneManagement;

public class Heli : MonoBehaviour, IInteractable
{
   
    public bool CanInteract(GameObject interactor)
    {
        return interactor.GetComponent<Interaction>().hasCompletOBJ; 
    }

    public void Interact(GameObject interactor)
    {
        SceneManager.LoadScene("Victory");
    }
}
