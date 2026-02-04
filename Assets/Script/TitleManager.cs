using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene("SimulationScene");
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}