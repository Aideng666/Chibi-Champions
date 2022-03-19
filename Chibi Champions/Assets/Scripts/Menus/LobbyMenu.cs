using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    /////////OLD MENU STUFF/////////////////
    //public Button backButton;

    //public void OnClick_Back()
    //{
    //    MenuManager.OpenMenu(Menu.MODE_SELECT, gameObject);
    //    backButton.transform.localScale = new Vector3(1, 1, 1);
    //}
    ////////////////////////////////////////

    [SerializeField] GameObject clientObject;

    private void Start()
    {
        //clientObject.AddComponent<PlayerClient>();
    }
}