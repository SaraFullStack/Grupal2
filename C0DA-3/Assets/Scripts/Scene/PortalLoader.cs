using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalLoader : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private bool isGameplayScene = true;

    public void LoadScene()
    {
        Time.timeScale = 1f;

        if (InputManager.Instance != null)
        {
            InputManager.Instance.CloseUI();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadScene();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        Time.timeScale = 1f;

        if (InputManager.Instance != null)
        {
            InputManager.Instance.CloseUI();
        }

        if (isGameplayScene)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}