using UnityEngine;
using UnityEngine.SceneManagement;

public class BaeminManager : MonoBehaviour
{
        public void GoBackToMain2()
    {
        SceneManager.LoadScene(
            "MainScene"
        );
    }

    public void GoBackToReview()
    {
        SceneManager.LoadScene(
            "ReviewScene"
        );
    }
}
