using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip mainBGM;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource.clip = mainBGM;
        audioSource.loop = true;
        audioSource.Play();
    }
}