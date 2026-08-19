using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneController : MonoBehaviour
{
    public void GoToReviewScene()
    {
        SceneManager.LoadScene("ReviewScene");
    }

    public void GoToWashScene()
    {
        SceneManager.LoadScene("WashScene");
    }
}