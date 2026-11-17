using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void GoToMain()
    {
        SceneManager.LoadScene("Main");
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
