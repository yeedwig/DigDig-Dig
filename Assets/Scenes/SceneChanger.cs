using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject CreditUI;
    private bool creditOpen;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void NewGame()
    {
        SaveLoadManager.loaded = false;
        SceneManager.LoadScene("TutorialScene");
    }

    public void LoadGame()
    {
        SaveLoadManager.loaded = true;
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("StartScene");
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
