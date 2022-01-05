using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMenu : MonoBehaviour
{
    [SerializeField] GameObject tower1;
    [SerializeField] GameObject tower2;
    [SerializeField] GameObject tower3;

    Transform platform;

    public static TowerMenu Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SetPlatform(Transform plat)
    {
        platform = plat;
    }

    public void BuyTower1()
    {
        Instantiate(tower1, new Vector3(platform.position.x, platform.position.y + 2.5f, platform.position.z), Quaternion.identity);
        platform.gameObject.SetActive(false);
        CanvasManager.Instance.CloseTowerMenu();
    }
}
