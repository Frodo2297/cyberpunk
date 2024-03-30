using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public double money;
    public double dpc;
    public double health;
    public double healthCap
    {
        get
        {
            return 10 * System.Math.Pow(2, stage - 1) * isBoss;
        }

    }


    public int stage;
    public int stageMax;
    public int kills;
    public int killsMax;
    public int isBoss;
    public int timerCap;

    public float timer;

    public Text moneyText;
    public Text dPCText;
    public Text stageText;
    public Text killsText;
    public Text healthText;
    public Text timerText;

    public GameObject back;
    public GameObject forward;

    public Image heathBar;
    public Image timerBar;

    public void Start()
    {
        dpc = 1;
        stage = 1;
        stageMax = 1;
        killsMax = 10;
        health = 10;
        isBoss = 1;
        timerCap = 30;
    }

    public void Update()
    {
        moneyText.text = "$" + money.ToString("F2");
        dPCText.text = dpc + "Damage";
        stageText.text = "Stage - " +stage;
        killsText.text = kills + "/" + killsMax + " kills";
        healthText.text = health + "/" + healthCap + " HP";


        heathBar.fillAmount = (float)(health / healthCap);

        if (stage > 1) back.gameObject.SetActive(true);
        else back.gameObject.SetActive(false);

        if (stage != stageMax) forward.gameObject.SetActive(true);
        else forward.gameObject.SetActive(false);

        IsBossChecker();
    }

    public void IsBossChecker()
    {
        if (kills % 10 == 0)
        {
            isBoss = 2;
            timerText.text = timer + "/" + timerCap;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                stage -= 1;
                health = healthCap;
            }

        }
        else
        {
            isBoss = 1;
            timerText.text = "";
            
        }

                

    }

    public void Hit()
    {
        health -= dpc;
        if (health <= 0) 
        {
            money += math.ceil(healthCap/14);
            if (stage == stageMax) 
            {
                kills += 1;
                if (kills >= killsMax)
                {
                    kills = 0;
                    stage += 1;
                    stageMax += 1;
                }

            }
            IsBossChecker();
            health = healthCap;
            if (isBoss == 2)
            {
                timer = timerCap;
                killsMax = 1;
            }
            killsMax = 10;

        }

    }

    public void Back()
    {
        if (stage > 1) stage -= 1;
    }
    public void Forward()
    {
        if (stage != stageMax) stage += 1;
    }
}
