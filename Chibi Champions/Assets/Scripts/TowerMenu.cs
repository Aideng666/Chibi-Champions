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
    PlayerController player;

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

    public void SetPlayer(PlayerController p)
    {
        player = p;
    }

    public void SetMenuState(MenuState state)
    {
        currentMenuState = state;
    }

    public void BuyTower1()
    {
        if (tower1.GetComponent<Tower>().GetCost() <= player.GetComponent<PointsManager>().GetCurrentPoints())
        {
            Instantiate(tower1, new Vector3(platform.position.x, platform.position.y + 2.5f, platform.position.z), Quaternion.identity);
            platform.gameObject.SetActive(false);
            CanvasManager.Instance.CloseTowerMenu();
            player.GetComponent<PointsManager>().SpendPoints(tower1.GetComponent<Tower>().GetCost());
        }
        else
        {
            print("Not Enough Points");
        }
    }

    public void BuyTower2()
    {
        if (tower2.GetComponent<Tower>().GetCost() <= player.GetComponent<PointsManager>().GetCurrentPoints())
        {
            Instantiate(tower2, new Vector3(platform.position.x, platform.position.y + 2.5f, platform.position.z), Quaternion.identity);
            platform.gameObject.SetActive(false);
            CanvasManager.Instance.CloseTowerMenu();
            player.GetComponent<PointsManager>().SpendPoints(tower2.GetComponent<Tower>().GetCost());
        }
        else
        {
            print("Not Enough Points");
        }
    }

    public void BuyTower3()
    {
        if (tower3.GetComponent<Tower>().GetCost() <= player.GetComponent<PointsManager>().GetCurrentPoints())
        {
            Instantiate(tower3, new Vector3(platform.position.x, platform.position.y + 2.5f, platform.position.z), Quaternion.identity);
            platform.gameObject.SetActive(false);
            CanvasManager.Instance.CloseTowerMenu();
            player.GetComponent<PointsManager>().SpendPoints(tower3.GetComponent<Tower>().GetCost());
        }
        else
        {
            print("Not Enough Points");
        }
    }

    public void UpgradeTower()
    {
        if (tower.GetComponent<Tower>().GetUpgradeCost(tower.GetComponent<Tower>().GetLevel()) <= player.GetComponent<PointsManager>().GetCurrentPoints())
        {
            player.GetComponent<PointsManager>().SpendPoints(tower.GetComponent<Tower>().GetUpgradeCost(tower.GetComponent<Tower>().GetLevel()));
            tower.GetComponent<Tower>().Upgrade();
        }
        else
        {
            print("Not Enough Points");
        }
    }
}
