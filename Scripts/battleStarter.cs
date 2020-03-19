using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleStarter : MonoBehaviour
{

    public BattleType[] potentialBattles;
    public bool activeOnEnter, activeOnStay, activeOnExit;
    private bool inArea;

    public float timeBetweenBattle = 1f;
    private float betweenBattleCounter;

    public bool deactivateAfterStarting;

    public bool cannotFlee;

    public bool shouldCompleteQuest;
    public string QuestToComplete;
    // Start is called before the first frame update
    void Start()
    {
        betweenBattleCounter = Random.Range(timeBetweenBattle * .5f, timeBetweenBattle * 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (inArea && Player.instance.canMove)
        {
            if(Input.GetAxisRaw("Horizontal")!=0|| Input.GetAxisRaw("Vertical") != 0)
            {
                betweenBattleCounter -= Time.deltaTime;
            }

            if (betweenBattleCounter < 0)
            {
                betweenBattleCounter = Random.Range(timeBetweenBattle * .5f, timeBetweenBattle * 1.5f);
                StartCoroutine(StartBattleCo());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (activeOnEnter)
            {
                StartCoroutine(StartBattleCo());
            }
            else
            {
                inArea = true;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inArea = false;

            if (activeOnExit)
            {
                StartCoroutine(StartBattleCo());
            }
            else
            {
                inArea = false;
            }
        }
    }

    public IEnumerator StartBattleCo()
    {
        UIFade.instance.FadeToBlack();
        GameManager.instance.battleActive = true;

        int selectedBattle = Random.Range(0, potentialBattles.Length);

        BattleManager.instance.rewardItems = potentialBattles[selectedBattle].rewardItems;
        BattleManager.instance.rewardEp = potentialBattles[selectedBattle].rewardXp;
        yield return new WaitForSeconds(1.5f);

        BattleManager.instance.BattleStart(potentialBattles[selectedBattle].enemies,cannotFlee);
        UIFade.instance.FadeFromBlack();
        if (deactivateAfterStarting)
        {
            gameObject.SetActive(false);
        }
        BattleReward.instance.markQuestComplete = shouldCompleteQuest;
        BattleReward.instance.questToMark = QuestToComplete;
    }
}
