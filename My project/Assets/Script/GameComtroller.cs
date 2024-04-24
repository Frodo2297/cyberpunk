using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GameController : MonoBehaviour
{
    public double money;
    public double moneyPerSec
    {
        get
        {
            return Math.Ceiling(healthCap / 14) / healthCap * dps;
        }
    }
    public double dpc;
    public double dps;
    public double health;
    public double healthCap
    {
        get
        {
            return (10 * System.Math.Pow(2, stage - 1) * isBoss);
        }
    }

    public float timer;

    public int stage;
    public int stageMax;
    public int kills;
    public int killsMax;
    public int isBoss;
    public int timerCap;
    public int acceptY;

    public Text moneyText;
    public Text dPCText;
    public Text dPSText;
    public Text stageText;
    public Text killsText;
    public Text healthText;
    public Text timerText;

    public GameObject back;
    public GameObject forward;

    public Image healthBar;
    public Image timerBar;
    public Image timerBG;

    public Animator gearExplode;
    public GameObject gearExplodeGameObject;

    //offline mode
    public DateTime currentDate;
    public DateTime oldTime;
    public int OfflineProgressCheck;
    public float idleTime;
    public Text offlineTimeText;
    public float saveTime;
    public GameObject offlineBox;
    public int offlineLoadCount;

    //multiplier
    public Text multText;
    public double multValue;
    public float timerMult;
    public float timerMultCap;
    public double multValueMoney;
    public GameObject multBox;

    //welcome
    public Text usernameText;
    public Text usernameText1;
    public Text usernameText2;
    public GameObject usernameBox;
    public GameObject accept;
    public GameObject accept1;
    public GameObject accept2;
    public GameObject reset;

    //upgrades
    public Text pCostText;
    public Text pLevelText;
    public Text pPowerText;
    public double pCost
    {
        get
        {
            return 10 * Math.Pow(1.07, pLevel);
        }
    }
    public int pLevel;
    public double pPower
    {
        get
        {
            return 5 * pLevel;
        } 
    }
    public Text cCostText;
    public Text cLevelText;
    public Text cPowerText;
    public double cCost
    {
        get
        {
            return 10 * Math.Pow(1.07, cLevel);
        }
    }
    public int cLevel;
    public double cPower
    {
        get
        {
            return 2 * cLevel;
        }
    }

    //Background
    public Image bgBoss;
    public Image boss;
    public Image enemy;
    

    public void Start()
    {
        offlineBox.gameObject.SetActive(false);
        multBox.gameObject.SetActive(false);
        Load();
        if (acceptY < 1)
        {
            usernameText.gameObject.SetActive(true);
            usernameText1.gameObject.SetActive(false);
            usernameText2.gameObject.SetActive(false);
            accept1.gameObject.SetActive(false);
            accept2.gameObject.SetActive(false);
            usernameBox.gameObject.SetActive(true);
        }
        else
            usernameBox.gameObject.SetActive(false);

        IsBossChecker();
        health = healthCap;
        timerCap = 30;
        multValue = new System.Random().Next(20, 100);
        timerMultCap = new System.Random().Next(5, 10);
        timerMult = timerMultCap;
    }
    public void NextPage()
    {
        accept.SetActive(false);
        accept1.SetActive(true);
        accept2.SetActive(false);
        usernameText.gameObject.SetActive(false);
        usernameText1.gameObject.SetActive(true);
        usernameText2.gameObject.SetActive(false);
    }
    public void NextPage1()
    {
        accept.SetActive(false);
        accept1.SetActive(false);
        accept2.SetActive(true);
        usernameText.gameObject.SetActive(false);
        usernameText1.gameObject.SetActive(false);
        usernameText2.gameObject.SetActive(true);
    }
    public void Exit()
    {
        usernameBox.gameObject.SetActive(false);
        acceptY += 1;
    }
    public void Reset()
    {
        acceptY = 0;
    }

    public void Update()
    {
        if (health <= 0) kill();
        else
        { health -= dps * Time.deltaTime; }

        //mult
        multValueMoney = multValue * moneyPerSec;
        multText.text = "$" + multValueMoney.ToString("F2");
        if (timerMult <= 0) multBox.gameObject.SetActive(true);
        else
            timerMult -= Time.deltaTime;

        moneyText.text = "$" + WordNotation(money, "F2");
        stageText.text = "stage - " + stage;
        killsText.text = kills + "/" + killsMax;
        healthText.text = WordNotation(health, "F2") + "/" + WordNotation(healthCap, "F2") + "HP";
        dPCText.text = WordNotation(dpc, "F2") + " Per Click";
        dPSText.text = WordNotation(dps, "F2") + " Per Second";

        healthBar.fillAmount = (float)(health / healthCap);

        if (stage > 1) back.gameObject.SetActive(true);
        else
            back.gameObject.SetActive(false);

        if (stage != stageMax) forward.gameObject.SetActive(true);
        else
            forward.gameObject.SetActive(false);

        IsBossChecker();
        Upgrades();

        saveTime += Time.deltaTime;
        if (saveTime >= 5)
        {
            saveTime = 0;
            save();
        }
    }

    public void Upgrades()
    {
        cCostText.text = "Cost: $" + WordNotation(cCost, "F2");
        cLevelText.text = "Level: " + cLevel;
        cPowerText.text = "+ 2 per hit";

        pCostText.text = "Cost: $" + WordNotation(pCost, "F2");
        pLevelText.text = "Level: " + pLevel;
        pPowerText.text = "+ 5 per second";
        dps = pPower;
        dpc = 1 + cPower;
    }

    public void IsBossChecker()
    { 
        if (stage % 5 == 0)
        {
            isBoss = 10;
            stageText.text = "(BOSS!) Stage - " + stage;
            timer -= Time.deltaTime;
            if (timer <= 0) Back();

            timerText.text = timer + "/" + timerCap;
            timerBar.gameObject.SetActive(true);
            timerBG.gameObject.SetActive(true);
            timerBar.fillAmount = timer / timerCap;
            killsMax = 1;
            bgBoss.gameObject.SetActive(true);
            boss.gameObject.SetActive(true);
            enemy.gameObject.SetActive(false);
        }
        else
        {
            isBoss = 1;
            stageText.text = "Stage - " + stage;
            timerText.text = "";
            timerBar.gameObject.SetActive(false);
            timerBG.gameObject.SetActive(false);
            timer = 30;
            killsMax = 10;
            bgBoss.gameObject.SetActive(false);
            boss.gameObject.SetActive (false);
            enemy.gameObject.SetActive(true);
        }
    }

    public void Hit()
    {
        health -= dpc;
        if (health <= 0)
        {
            kill();
        }
    }

    public void kill()
    {
        money += Math.Ceiling(healthCap / 14);
        gearExplode.Play("GearsExplode", 0, 0);
        if (stage == stageMax)
        {
            kills += 1;
            if (kills > +killsMax)
            {
                kills = 0;
                stage += 1;
                stageMax += 1;
            }
        }
        IsBossChecker();
        health = healthCap;
        if (isBoss > 1) timer = timerCap;
        killsMax = 10;
    }

    public void Back()
    {
        stage -= 1;
        IsBossChecker();
        health = healthCap;
    }

    public void Forward()
    {
        stage += 1;
        IsBossChecker();
        health = healthCap;
    }

    public void BuyUpgrades(string id)
    {
        switch (id)
        {
            case "p1":
                if (money >= pCost) UpgradeDefaults(ref pLevel, pCost); 
                break;
            case "c1":
                if(money >=cCost) UpgradeDefaults(ref cLevel, cCost); 
                break;    
        }
    }

    public void UpgradeDefaults(ref int level, double cost)
    {
        money -= cost;
        level++;
    }

    public string WordNotation(double number, string didgits)
    {
        double didgitsTemp = Math.Floor(Math.Log10(number));
        IDictionary<double,string> prefixes = new Dictionary<double,string>()
        {
            {3,"K"},
            {6,"M"},
            {9,"B"},
            {12,"T"},
            {15,"Qa"},
            {18,"Qi"},
            {21,"Se"},
            {24,"Sep"}
        };
        double didgitsEvery3 = 3 * Math.Floor(didgitsTemp / 3);
        if (number >= 1000)
            return (number / Math.Pow(10, didgitsEvery3)).ToString(didgits) + prefixes[didgitsEvery3];
        return number.ToString(didgits);
    }

    public void save()
    {
        OfflineProgressCheck = 1;
        PlayerPrefs.SetString("money", money.ToString());
        PlayerPrefs.SetString("dpc", dpc.ToString());
        PlayerPrefs.SetString("dps", dps.ToString());
        PlayerPrefs.SetInt("acceptY", acceptY);
        PlayerPrefs.SetInt("stage", stage);
        PlayerPrefs.SetInt("stageMax", stageMax);
        PlayerPrefs.SetInt("kills", kills);
        PlayerPrefs.SetInt("killsMax", killsMax);
        PlayerPrefs.SetInt("isBoss", isBoss);
        PlayerPrefs.SetInt("cLevel", cLevel);
        PlayerPrefs.SetInt("pLevel", pLevel);
        PlayerPrefs.SetInt("OfflineProgressCheck", OfflineProgressCheck);

        PlayerPrefs.SetString("OfflineTime", DateTime.Now.ToBinary().ToString());
    }

    public void Load()
    {
        money = double.Parse(PlayerPrefs.GetString("money", "0"));
        dpc = double.Parse(PlayerPrefs.GetString("dpc", "1"));
        dps = double.Parse(PlayerPrefs.GetString("dps", "1"));
        stage = PlayerPrefs.GetInt("stage", 1);
        stageMax = PlayerPrefs.GetInt("stageMax", 1);
        kills = PlayerPrefs.GetInt("kills", 0);
        killsMax = PlayerPrefs.GetInt("killsMax", 10);
        isBoss = PlayerPrefs.GetInt("isBoss", 1);
        pLevel = PlayerPrefs.GetInt("pLevel", 0);
        cLevel = PlayerPrefs.GetInt("cLevel", 0);
        OfflineProgressCheck = PlayerPrefs.GetInt("OfflineProgressCheck", 0);
        acceptY = PlayerPrefs.GetInt("acceptY", 0);
        LoadOfflineProduction();
    }

    public void LoadOfflineProduction()
    {
        if (acceptY > 0)
        {
            offlineBox.gameObject.SetActive(true);
            long previousTime = Convert.ToInt64(PlayerPrefs.GetString("OfflineTime"));
            oldTime = DateTime.FromBinary(previousTime);
            currentDate = DateTime.Now;
            TimeSpan difference = currentDate.Subtract(oldTime);
            idleTime = (float)difference.TotalSeconds;

            var moneyToEarn = Math.Ceiling(healthCap / 14) / healthCap * (dps / 5) * idleTime;
            money += moneyToEarn;
            TimeSpan timer = TimeSpan.FromSeconds(idleTime);

            offlineTimeText.text = "You were gone for: " + timer.ToString(@"hh\:mm\:ss") + "\n\nYou earned: $" + moneyToEarn.ToString("F2");

        }
    }

    public void CloseOfflineBox()
    {
        offlineBox.gameObject.SetActive(false);
    }

    public void OpenMult()
    {
        multBox.gameObject.SetActive(false);
        money += multValueMoney;
        timerMultCap = new System.Random().Next(5, 10);
        timerMult = timerMultCap;
        multValue = new System.Random().Next(20, 100);
    }
}