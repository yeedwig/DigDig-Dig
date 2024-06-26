using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    private float originalCameraSize;
    [SerializeField] Camera minimapCamera;
    [SerializeField] float minSize, maxSize;

    //playerManager
    [SerializeField] GameObject player;
    private PlayerManager PM;
    // Start is called before the first frame update
    void Start()
    {
        originalCameraSize = minimapCamera.orthographicSize;
        PM = player.GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PM.minimapOpened)
        {
            ChangeMinimapSize();
        }
        else
        {
            minimapCamera.orthographicSize = originalCameraSize;
        }
    }

    
    // 스크롤로 미니맵 크기 변경
    private void ChangeMinimapSize()
    {
        Vector2 wheelInput = Input.mouseScrollDelta;
        if (wheelInput.y > 0 &&minimapCamera.orthographicSize<maxSize)
        {
            minimapCamera.orthographicSize+=0.25f;
        }
        else if(wheelInput.y < 0 && minimapCamera.orthographicSize > minSize)
        {
            minimapCamera.orthographicSize-=0.25f;
        }
    }
}
