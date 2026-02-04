using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;

    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        bool isActive = !pausePanel.activeSelf;
        pausePanel.SetActive(isActive);

        Time.timeScale = isActive ? 0 : 1;
    }

    public void OnClickToTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScene");
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}