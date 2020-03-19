using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*transition the decided position when enter the new scene*/
public class AreaEntrance : MonoBehaviour
{

    public string transitionName;
    
    
    // Start is called before the first frame update
    //何故か、読み込んだ時にこのスクリプトもスタートしてしまうので、バグ修正のため、一時的にここでポジションを呼ぶ

    void Start()
    {

        if (transitionName == Player.instance.areaTransitionName)
        {

            
            Player.instance.transform.position = transform.position;
        }
        
        else if (Player.instance.areaTransitionName=="continue")
        {
            
            Player.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x")
           , PlayerPrefs.GetFloat("Player_Position_y")
           , PlayerPrefs.GetFloat("Player_Position_z"));
        }
        

        
       
   
        UIFade.instance.FadeFromBlack();
        GameManager.instance.fadingBetweenAreas = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
