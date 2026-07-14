using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void GoToWashScene()
    {
        SceneManager.LoadScene("WashScene");
    }

    public void GoToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}