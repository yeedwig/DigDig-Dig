using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void NewGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
