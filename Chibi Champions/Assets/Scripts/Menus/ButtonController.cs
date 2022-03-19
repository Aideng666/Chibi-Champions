using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void ShowPanel(GameObject newPanel)
    {
        newPanel.SetActive(true);
    }

    public void HidePanel(GameObject oldPanel)
    {
        oldPanel.SetActive(false);
    }
}
