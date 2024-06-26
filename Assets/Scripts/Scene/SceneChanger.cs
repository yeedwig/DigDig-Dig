using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject CreditUI;
    private bool creditOpen;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Text progressText;

    private static readonly string SAVE_FOLDER = Application.dataPath + "/SaveAndLoad/";

    // Update is called once per frame
    public void NewGame()
    {
        SaveLoadManager.loaded = false;
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelAsync("TutorialScene"));
    }


    public void NewGameWithoutTutorial()
    {
        SaveLoadManager.loaded = false;
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelAsync("MainScene"));
    }

    IEnumerator LoadLevelAsync(string name)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(name);

        while(!loadOperation.isDone)
        {
            float progressValue = loadOperation.progress *100f + 10.0f;
            progressText.text = progressValue.ToString() + "%";
            yield return null;

        }
        yield return new WaitForSeconds(1.0f);
        loadingScreen.SetActive(false);
    }

    public void LoadGame()
    {
        if (File.Exists(SAVE_FOLDER + "/PlayerSave.txt"))
        {
            SaveLoadManager.loaded = true;
            loadingScreen.SetActive(true);
            StartCoroutine(LoadLevelAsync("MainScene"));
        }
        else
        {
            SaveLoadManager.loaded = false;
            loadingScreen.SetActive(true);
            StartCoroutine(LoadLevelAsync("TutorialScene"));
        }
        
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
