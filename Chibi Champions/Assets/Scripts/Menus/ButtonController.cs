using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }

    public void ShowPanel(GameObject newPanel)
    {
        transform.localScale = new Vector3(1, 1, 1);
        newPanel.SetActive(true);
    }

    public void HidePanel(GameObject oldPanel)
    {
        transform.localScale = new Vector3(1, 1, 1);
        oldPanel.SetActive(false);
    }

    public void GoToCinematicScene()
    {
        SceneManager.LoadScene("Cinematic");
    }
}
