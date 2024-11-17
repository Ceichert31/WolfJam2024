using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagement : MonoBehaviour
{
    public string SceneName;
    public string Credits;
    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Credit()
    {
        SceneManager.LoadScene(Credits);
    }
}
