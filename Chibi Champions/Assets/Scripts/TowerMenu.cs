using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerMenu : MonoBehaviour
{
    [SerializeField] GameObject buyPanel;
    [SerializeField] GameObject upgradePanel;

    [SerializeField] GameObject towerPlatformPrefab;

    [SerializeField] Image[] towerImages;
    [SerializeField] TMP_Text[] towerBaseCosts;
    [SerializeField] TMP_Text upgradeCostText;
    [SerializeField] TMP_Text upgradeNameText;
    [SerializeField] Image upgradeImageIcon;
    [SerializeField] TMP_Text towerName;
    public CharacterDatabase characterDB;
    Character character;

    [SerializeField] TMP_Text towerLevelText;

    GameObject[] towers = new GameObject[3];
    Transform platform;
    Transform currentTower;
    PlayerController player;

    MenuState currentMenuState;

    Button[] buttons;

    public static TowerMenu Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        buttons = GetComponentsInChildren<Button>();

        character = characterDB.GetCharacter(PlayerPrefs.GetInt("CharacterIndex"));
    }

    private void Update()
    {
        if (currentMenuState == MenuState.Buy)
        {
            for (int i = 0; i < towerImages.Length; ++i)
            {
                towerImages[i].sprite = character.towerSprites[i];
            }

            for (int i = 0; i < towerBaseCosts.Length; ++i)
            {
                towerBaseCosts[i].text = character.towerBaseCosts[i].ToString();
            }

            upgradePanel.SetActive(false);
            buyPanel.SetActive(true);
        }
        else if (currentMenuState == MenuState.Upgrade)
        {
            upgradePanel.SetActive(true);
            buyPanel.SetActive(false);
            buttons[4].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = $"Sell For {currentTower.GetComponent<Tower>().GetTotalPointsSpent() * 0.7} Points";
                    
            towerLevelText.text = (currentTower.GetComponent<Tower>().GetLevel() - 1).ToString();
            towerName.text = currentTower.GetComponent<Tower>().GetTowerName();     
            
            if (currentTower.GetComponent<Tower>().GetLevel() == 4)
            {
                upgradeCostText.text = string.Empty;
                buttons[3].gameObject.GetComponentInChildren<Button>().interactable = false;
                upgradeNameText.text = "Fully Upgraded";
                upgradeImageIcon.sprite = currentTower.GetComponent<Tower>().GetUpgradeImage(3);
            }
            else
            {
                upgradeCostText.text = currentTower.GetComponent<Tower>().GetUpgradeCost(currentTower.GetComponent<Tower>().GetLevel()).ToString();
                buttons[3].gameObject.GetComponentInChildren<Button>().interactable = true;
                upgradeNameText.text = currentTower.GetComponent<Tower>().GetUpgradeName(currentTower.GetComponent<Tower>().GetLevel()).ToString();
                upgradeImageIcon.sprite = currentTower.GetComponent<Tower>().GetUpgradeImage(currentTower.GetComponent<Tower>().GetLevel());

            }
        }

        for (int i = 0; i < 3; i++)
        {
            buttons[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = towers[i].name;
        }     
    }

    public void SetPlatform(Transform plat)
    {
        platform = plat;
    }

    public void SetTower(Transform t)
    {
        currentTower = t;
    }

    public void SetPlayer(PlayerController p)
    {
        player = p;

        for (int i = 0; i < player.GetTowers().Length; i++)
        {
            towers[i] = player.GetTowers()[i];
        }
    }

    public void SetMenuState(MenuState state)
    {
        currentMenuState = state;
    }

    public void BuyTower1()
    {
        if (towers[0].GetComponent<Tower>().GetCost() <= player.GetComponent<PointsManager>().GetCurrentPoints())
        {
            Instantiate(towers[0], new Vector3(platform.position.x, towers[0].transform.position.y, platform.position.z), Quaternion.identity);
            platform.gameObject.SetActive(false);
            CanvasManager.Instance.CloseTowerMenu();
            player.GetComponent<PointsManager>().SpendPoints(towers[0].GetComponent<Tower>().GetCost());
        }
        else
        {
            print("Not Enough Points");
        }
    }

    public void BuyTower2()
    {
        if (towers[1].GetComponent<Tower>().GetCost() <= player.GetComponent<PointsManager>().GetCurrentPoints())
        {
            Instantiate(towers[1], new Vector3(platform.position.x, towers[1].transform.position.y, platform.position.z), Quaternion.identity);
            platform.gameObject.SetActive(false);
            CanvasManager.Instance.CloseTowerMenu();
            player.GetComponent<PointsManager>().SpendPoints(towers[1].GetComponent<Tower>().GetCost());
        }
        else
        {
            print("Not Enough Points");
        }
    }

    public void BuyTower3()
    {
        if (towers[2].GetComponent<Tower>().GetCost() <= player.GetComponent<PointsManager>().GetCurrentPoints())
        {
            Instantiate(towers[2], new Vector3(platform.position.x, towers[2].transform.position.y, platform.position.z), Quaternion.identity);
            platform.gameObject.SetActive(false);
            CanvasManager.Instance.CloseTowerMenu();
            player.GetComponent<PointsManager>().SpendPoints(towers[2].GetComponent<Tower>().GetCost());
        }
        else
        {
            print("Not Enough Points");
        }
    }

    public void UpgradeTower()
    {
        if (currentTower.GetComponent<Tower>().GetUpgradeCost(currentTower.GetComponent<Tower>().GetLevel()) <= player.GetComponent<PointsManager>().GetCurrentPoints())
        {
            player.GetComponent<PointsManager>().SpendPoints(currentTower.GetComponent<Tower>().GetUpgradeCost(currentTower.GetComponent<Tower>().GetLevel()));
            currentTower.GetComponent<Tower>().Upgrade();
            CanvasManager.Instance.CloseTowerMenu();
        }
        else
        {
            print("Not Enough Points");
        }
    }

    public void SellTower()
    {
        Vector3 towerPosition = currentTower.transform.position;

        Instantiate(towerPlatformPrefab, new Vector3(towerPosition.x, towerPlatformPrefab.transform.position.y, towerPosition.z), Quaternion.identity, GameObject.Find("Platforms").transform);
        player.GetComponent<PointsManager>().AddPoints((int)(currentTower.GetComponent<Tower>().GetTotalPointsSpent() * 0.7));
        Destroy(currentTower.gameObject);
        CanvasManager.Instance.CloseTowerMenu();
    }
}
