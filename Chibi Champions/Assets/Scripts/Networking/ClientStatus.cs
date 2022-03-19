using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClientStatus : MonoBehaviour
{
    [SerializeField] GameObject confirmRequestPanel;

    public void SendRequest()
    {
        LobbyManager.Instance.SendRequestName(GetComponentInChildren<TextMeshProUGUI>().text);

        print("Name Sent");

        Instantiate(confirmRequestPanel, transform.parent);

        print("Spawned Confirm");
    }

    public void ConfirmRequest()
    {
        LobbyManager.Instance.SendRequestMessage();
    }
}
