using UnityEngine;
using UnityEngine.SceneManagement;

public class Heli : MonoBehaviour, IInteractable
{
    public bool hasCompletOBJ;
    public bool CanInteract(GameObject interactor)
    {
        print("Objective Completion Statue :" + hasCompletOBJ);
        return hasCompletOBJ;
    }

    public void Interact(GameObject interactor)
    {
        SceneManager.LoadScene("Victory");
    }
}
