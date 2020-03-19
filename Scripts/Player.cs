using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*about Player movement
 @ walk animation
 @ idle animation
 @ the limit of field which player can move in
 */
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D theRb;
    public float playerSpeed = 10f;

    float maxNum = 1f;
    float minNum =-1f;
    float HalfOfplayer= 0.5f;
    
    public Animator myAnim;
    //we can have the only one instance in the whole world.
    public static Player instance;

    private Vector3 BottomLeftLimit;
    private Vector3 TopRightLimit;

    public string areaTransitionName;
    public bool canMove = true;
    void Start()
    {
        
        myAnim.SetFloat("LastMoveX", 0);
        myAnim.SetFloat("LastMoveY", -1);
        //canMove = true;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            //if another instance already there, destroy myself
            if (instance != this)
            {
                //destroy the new onject
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
        PlayerMove();
        //keep the camera inside the bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, BottomLeftLimit.x, TopRightLimit.x),
                                         Mathf.Clamp(transform.position.y, BottomLeftLimit.y, TopRightLimit.y),
                                         transform.position.z);
    }

    /*player move controll*/
    private void PlayerMove()
    {

        ControlPlayerWalk();
        ControlPlayerIdle();
    }

    /*player walk control*/
    private void ControlPlayerWalk()
    {
        if (canMove)
        {
            //may add time.deltataime later.
            theRb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical"))
                * playerSpeed;
            myAnim.SetFloat("MoveX", theRb.velocity.x);
            myAnim.SetFloat("MoveY", theRb.velocity.y);

        }
        else
        {
            theRb.velocity = new Vector2(0,0);

        }
       
    }

    private void ControlPlayerIdle()
    {
        //Move to the right then Horizontal is 1. 
        if (Input.GetAxisRaw("Horizontal") == maxNum || Input.GetAxisRaw("Horizontal") == minNum ||
            Input.GetAxisRaw("Vertical") == maxNum || Input.GetAxisRaw("Vertical") == minNum)
        {
            if (canMove)
            {
                myAnim.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));
                myAnim.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
            }
        }


    }

    public void SetBounds(Vector3 botLeft, Vector3 topRight)
    {
        BottomLeftLimit = botLeft + new Vector3(HalfOfplayer,HalfOfplayer,0f);
        TopRightLimit = topRight + new Vector3(-HalfOfplayer, -HalfOfplayer, 0f);

    
        
    }
}

    

