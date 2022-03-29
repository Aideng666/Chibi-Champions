using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public static bool hasReturnedToMenu = false;

    public void SwitchScenes(string sceneName)
    {
        LevelManager.Instance.LoadScene(sceneName);
    }

    public void ReturnToMainMenu()
    {
        LevelManager.Instance.LoadScene("MenuScenes");
        hasReturnedToMenu = true;
    }

    public void PlayAgain()
    {
        LevelManager.Instance.LoadScene("Main");
    }

    public void PauseQuit()
    {
        SceneManager.LoadScene("MenuScenes");
    }
}
