using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneController : MonoBehaviour
{
    public void GoToReviewScene()
    {
        SceneManager.LoadScene("BaeminScene");
    }

    public void GoToWashScene()
    {
        SceneManager.LoadScene("WashScene");
    }
}