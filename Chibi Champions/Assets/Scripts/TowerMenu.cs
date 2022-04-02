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

    [SerializeField] List<GameObject> platforms = new List<GameObject>();

    [SerializeField] Image[] towerImages;
    [SerializeField] TMP_Text[] towerBaseCosts;
    [SerializeField] TMP_Text[] towerBaseDescriptions;
    [SerializeField] TMP_Text upgradeCostText;
    [SerializeField] TMP_Text upgradeNameText;
    [SerializeField] Image upgradeImageIcon;
    [SerializeField] TMP_Text towerName;
    public CharacterDatabase characterDB;
    Character character;

    [SerializeField] TMP_Text towerLevelText;
    [SerializeField] TMP_Text descriptionText;

    GameObject[] towers = new GameObject[3];
    Transform platform;
    Transform currentTower;
    PlayerController player;

    MenuState currentMenuState;

    Button[] buttons;

    bool isMaxLevel = false;

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

            for (int i = 0; i < towerBaseDescriptions.Length; ++i)
            {
                towerBaseDescriptions[i].text = character.towerDescriptions[i];
            }

            CannotPurchaseTower();
            CanPurchaseTower();

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

            UpdateUpgradeUI();
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

    public Transform GetTower()
    {
        return currentTower;
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

            FindObjectOfType<AudioManager>().Play("Build");
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

            FindObjectOfType<AudioManager>().Play("Build");
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

            FindObjectOfType<AudioManager>().Play("Build");
        }
        else
        {         
            print("Not Enough Points");
        }
    }

    public void UpgradeTower()
    {
        if (FindObjectOfType<UDPClient>() != null)
        {
            foreach (Tower tower in EntityManager.Instance.GetLocalTowers())
            {
                if (Vector3.Distance(currentTower.position, tower.transform.position) < 2 && currentTower.GetComponent<Tower>().GetTowerName() == tower.GetTowerName())
                {
                    UDPClient.Instance.SendTowerUpgrade(currentTower.position, tower.GetTowerName());
                }
            }
        }

        if (currentTower.GetComponent<Tower>().GetUpgradeCost(currentTower.GetComponent<Tower>().GetLevel()) <= player.GetComponent<PointsManager>().GetCurrentPoints())
        {
            player.GetComponent<PointsManager>().SpendPoints(currentTower.GetComponent<Tower>().GetUpgradeCost(currentTower.GetComponent<Tower>().GetLevel()));
            currentTower.GetComponent<Tower>().Upgrade();
            CanvasManager.Instance.CloseTowerMenu();

            FindObjectOfType<AudioManager>().Play("Improve");
        }
        else 
        {
            print("Not Enough Points");
        }
    }

    public void SellTower()
    {
        Vector3 towerPosition = currentTower.transform.position;

        foreach (GameObject plat in platforms)
        {
            if (Vector2.Distance(new Vector2(plat.transform.position.x, plat.transform.position.z),
                                 new Vector2(towerPosition.x, towerPosition.z)) < 5)
            {
                plat.SetActive(true);
            }
        }

        player.GetComponent<PointsManager>().AddPoints((int)(currentTower.GetComponent<Tower>().GetTotalPointsSpent() * 0.7));
        Destroy(currentTower.gameObject);
        FindObjectOfType<AudioManager>().Play("Sell");
        CanvasManager.Instance.CloseTowerMenu();
    }

    private void CannotPurchaseTower()
    {
        if (towers[0].GetComponent<Tower>().GetCost() > player.GetComponent<PointsManager>().GetCurrentPoints())
        {
            buttons[0].gameObject.GetComponentInChildren<Button>().interactable = false;
        }
        if (towers[1].GetComponent<Tower>().GetCost() > player.GetComponent<PointsManager>().GetCurrentPoints())
        {
            buttons[1].gameObject.GetComponentInChildren<Button>().interactable = false;
        }
        if (towers[2].GetComponent<Tower>().GetCost() > player.GetComponent<PointsManager>().GetCurrentPoints())
        {
            buttons[2].gameObject.GetComponentInChildren<Button>().interactable = false;
        }
    }

    private void CanPurchaseTower()
    {
        if (towers[0].GetComponent<Tower>().GetCost() <= player.GetComponent<PointsManager>().GetCurrentPoints())
        {
            buttons[0].gameObject.GetComponentInChildren<Button>().interactable = true;
        }
        if (towers[1].GetComponent<Tower>().GetCost() <= player.GetComponent<PointsManager>().GetCurrentPoints())
        {
            buttons[1].gameObject.GetComponentInChildren<Button>().interactable = true;
        }
        if (towers[2].GetComponent<Tower>().GetCost() <= player.GetComponent<PointsManager>().GetCurrentPoints())
        {
            buttons[2].gameObject.GetComponentInChildren<Button>().interactable = true;
        }
    }

    private void UpdateUpgradeUI()
    {
        if (currentTower.GetComponent<Tower>().GetLevel() == 4)
        {
            isMaxLevel = true;

            upgradeCostText.text = string.Empty;
            upgradeNameText.text = "Fully Upgraded";
            upgradeImageIcon.sprite = currentTower.GetComponent<Tower>().GetUpgradeImage(3);
            
            descriptionText.text = currentTower.GetComponent<Tower>().GetUpgradeDescriptions(3);
        }
        else
        {
            upgradeCostText.text = currentTower.GetComponent<Tower>().GetUpgradeCost(currentTower.GetComponent<Tower>().GetLevel()).ToString();       
            upgradeNameText.text = currentTower.GetComponent<Tower>().GetUpgradeName(currentTower.GetComponent<Tower>().GetLevel()).ToString();
            upgradeImageIcon.sprite = currentTower.GetComponent<Tower>().GetUpgradeImage(currentTower.GetComponent<Tower>().GetLevel());

            descriptionText.text = currentTower.GetComponent<Tower>().GetUpgradeDescriptions(currentTower.GetComponent<Tower>().GetLevel());
        }

        if (!isMaxLevel)
        {
            if (currentTower.GetComponent<Tower>().GetUpgradeCost(currentTower.GetComponent<Tower>().GetLevel()) <= player.GetComponent<PointsManager>().GetCurrentPoints())
            {
                buttons[3].gameObject.GetComponentInChildren<Button>().interactable = true;
            }
            if (currentTower.GetComponent<Tower>().GetUpgradeCost(currentTower.GetComponent<Tower>().GetLevel()) > player.GetComponent<PointsManager>().GetCurrentPoints())
            {
                buttons[3].gameObject.GetComponentInChildren<Button>().interactable = false;
            }
        }

        if (isMaxLevel)
        {
            buttons[3].gameObject.GetComponentInChildren<Button>().interactable = false;
            isMaxLevel = false;
        }
    }
}
