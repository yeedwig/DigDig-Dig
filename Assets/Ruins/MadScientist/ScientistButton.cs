using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistButton : MonoBehaviour
{
    public bool isOn;
    private SpriteRenderer sp;
    [SerializeField] Sprite buttonOnSprite;
    private void Start()
    {
        isOn = false;
        sp = GetComponent<SpriteRenderer>();
    }
    public void TurnOnButton()
    {
        sp.sprite = buttonOnSprite;
        isOn = true;
    }
}
