using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialToMain : MonoBehaviour
{
    [SerializeField] private SceneChanger changer;
   private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            SaveLoadManager.loaded = false;
            changer.NewGameWithoutTutorial();
        }
    }




}
