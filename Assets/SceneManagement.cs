using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagement : MonoBehaviour
{
    public string SceneName;
    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
