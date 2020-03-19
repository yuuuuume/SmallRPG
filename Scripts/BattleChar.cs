using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour
{
    public bool isPlayer;
    public string[] movesAvailable;

    public string charName;
    public int currentHP, currentMP, maxHP, maxMP, strength, defence, wpnPower, armrPower;
    public bool hasDied;

    public SpriteRenderer theSprite;
    public Sprite deadSprite, aliveSprite;

    private bool shouldFade;
    public float fadeSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFade)
        {
            theSprite.color = new Color(theSprite.color.r,
                                         theSprite.color.g,
                                         theSprite.color.b,
                                         Mathf.MoveTowards(theSprite.color.a,
                                                            0, fadeSpeed * Time.deltaTime));
            if(theSprite.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void EnemyFade()
    {
        shouldFade = true;

    }
}
