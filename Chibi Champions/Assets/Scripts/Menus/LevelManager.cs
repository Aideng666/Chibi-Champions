using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Image progressBar;
    private float target;

    public TMP_Text blurbText;
    public string[] blurbs;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PickRandomBlurb();
    }

    public async void LoadScene(string sceneName)
    {
        target = 0;
        progressBar.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        loadingCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);
            target = scene.progress;
        } while (scene.progress < 0.9f);

        await Task.Delay(5000);

        scene.allowSceneActivation = true;

        // Can play with this -> Just to prevent the flicker
        await Task.Delay(100);
        loadingCanvas.SetActive(false); //-> This turns off loading screen
    }

    private void Update()
    {
        progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, 3 * Time.deltaTime);
    }

    private void PickRandomBlurb()
    {
        string randomBlurb = blurbs[Random.Range(0, blurbs.Length)];
        blurbText.text = randomBlurb;
    }
}
