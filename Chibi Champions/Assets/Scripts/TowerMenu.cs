using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMenu : MonoBehaviour
{
    [SerializeField] GameObject tower1;
    [SerializeField] GameObject tower2;
    [SerializeField] GameObject tower3;
    [SerializeField] GameObject buyPanel;
    [SerializeField] GameObject upgradePanel;

    Transform platform;
    Transform tower;

    MenuState currentMenuState;

    public static TowerMenu Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (currentMenuState == MenuState.Buy)
        {
            upgradePanel.SetActive(false);
            buyPanel.SetActive(true);
        }
        else if (currentMenuState == MenuState.Upgrade)
        {
            upgradePanel.SetActive(true);
            buyPanel.SetActive(false);
        }
    }

    public void SetPlatform(Transform plat)
    {
        platform = plat;
    }

    public void SetTower(Transform t)
    {
        tower = t;
    }

    public void SetMenuState(MenuState state)
    {
        currentMenuState = state;
    }

    public void BuyTower1()
    {
        Instantiate(tower1, new Vector3(platform.position.x, platform.position.y + 2.5f, platform.position.z), Quaternion.identity);
        platform.gameObject.SetActive(false);
        CanvasManager.Instance.CloseTowerMenu();
    }

    public void BuyTower2()
    {
        Instantiate(tower2, new Vector3(platform.position.x, platform.position.y + 2.5f, platform.position.z), Quaternion.identity);
        platform.gameObject.SetActive(false);
        CanvasManager.Instance.CloseTowerMenu();
    }

    public void BuyTower3()
    {
        Instantiate(tower3, new Vector3(platform.position.x, platform.position.y + 2.5f, platform.position.z), Quaternion.identity);
        platform.gameObject.SetActive(false);
        CanvasManager.Instance.CloseTowerMenu();
    }

    public void UpgradeTower()
    {
        tower.GetComponent<Tower>().Upgrade();
    }
}
