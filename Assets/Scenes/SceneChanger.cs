using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject CreditUI;
    private bool creditOpen;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Text progressText;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void NewGame()
    {
        SaveLoadManager.loaded = false;
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelAsync("TutorialScene"));
    }

    IEnumerator LoadLevelAsync(string name)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(name);

        while(!loadOperation.isDone)
        {
            float progressValue = loadOperation.progress *100f;
            progressText.text = progressValue.ToString() + "%";
            yield return null;

        }
        yield return new WaitForSeconds(0.5f);
        loadingScreen.SetActive(false);
    }

    public void LoadGame()
    {

        SaveLoadManager.loaded = true;
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelAsync("MainScene"));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ToMainMenu()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelAsync("StartingScene"));
    }
    public void CreditButton()
    {
        if(creditOpen)
        {
            CreditUI.SetActive(false);
            creditOpen = false;
        }
        else
        {
            CreditUI.SetActive(true);
            creditOpen = true;
        }

       
    }
}
