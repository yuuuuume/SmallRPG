using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*control camera position
 audio track each scene*/
public class CameraController : MonoBehaviour
{

    public Transform target;
    public Tilemap theMap;
    
    //make a boundary for camera
    private Vector3 BottomLeftLimit;
    private Vector3 TopRightLimit;
    private float halfHeight;
    private float halfWidth;

    public int musicToPlay;
    private bool musicStarted;
    // Start is called before the first frame update
    void Start()
    {
        //this was not work for me
       // target = FindObjectOfType<Player>().transform;
       
        
        GetSize();
        SetCameraBoundary();

    }

    /*get the half length of x and y size to add the each boundary limit*/
    private void GetSize()
    {
        //orthographicSize is the half size of the vertical viewing volume
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }

    private void SetCameraBoundary()
    {
        BottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth,halfHeight,0f);
        TopRightLimit = theMap.localBounds.max + new Vector3(-halfWidth,-halfHeight,0f);
        Player.instance.SetBounds(theMap.localBounds.min, theMap.localBounds.max);
    }

    // LateUpdate is called once per frame after update
    void LateUpdate()
    {
       
        transform.position = new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y, transform.position.z);
        //keep the camera inside the bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, BottomLeftLimit.x , TopRightLimit.x),
                                         Mathf.Clamp(transform.position.y, BottomLeftLimit.y , TopRightLimit.y),
                                         transform.position.z);

        if (!musicStarted)
        {
            musicStarted = true;
            AudioManager.instance.PlayBGM(musicToPlay);
        }
    }
}
