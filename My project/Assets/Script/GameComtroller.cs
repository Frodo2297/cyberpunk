using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Unity.Mathematics;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using Unity.VisualScripting;

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
    public double dpcWin;
    public double dps;
    public double health;
    public double healthCap
    {
        get
        {
            return (10 * System.Math.Pow(2, stage - 1 + newGameCount) * isBoss);
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
    public int enemyTurn;
    public int isBossKilled1;
    public int isBossKilled2;
    public int medal;
    public int newLevel;
    public int newGameCount;

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

    public Animator enemyTap;
    public GameObject enemyTapGameObject;

    //offline mode
    public DateTime currentDate;
    public DateTime oldTime;
    public float idleTime;
    public Text offlineTimeText;
    public float saveTime;
    public GameObject offlineBox;
    public int offlineLoadCount;
    public int OfflineProgressCheck;


    //multiplier
    public Text multText;
    public float timerMult;
    public float timerMultCap;
    public double multValueMoney;
    public double multValue;
    public GameObject multBox;

    //welcome
    public Text welcomeText;
    public Text welcomeText1;
    public Text welcomeText2;
    public GameObject welcomeBox;
    public GameObject accept;
    public GameObject accept1;
    public GameObject accept2;
    public GameObject reset;

    //upgrades
    public Text pCostText;
    public Text pLevelText;
    public Text pPowerText;

    //sprites
    public Image enemyImg;
    public Sprite enemy1;
    public Sprite enemy2;
    public Sprite enemy3;
    public Sprite enemy4;
    public Sprite enemy5;
    public Sprite enemy6;
    public Sprite enemy7;
    public Sprite enemy8;
    public Sprite boss2;
    public Sprite boss1;

    public Image background;

    //background
    public Sprite bg1;
    public Sprite bg2;
    public Sprite bg3;
    public Sprite bg4;
    public Sprite bg5;
    public Sprite bg6;
    public Sprite bg7;
    public Sprite bg8;
    public Sprite bg9;
    public Sprite bg10;
    public Sprite bg11;
    public Sprite bg12;
    public Sprite bg13;
    public Sprite bg14;
    public Sprite bg15;
    public Sprite bg16;
    public Sprite bgBoss1;
    public Sprite bgBoss2;

    public GameObject effect;
    public AudioSource soundPlay;

    public GameObject winScreen;
    public GameObject closeWinScreen;
    public GameObject nextWinScreen;
    public GameObject attentionScreen;
    public GameObject closeAttentionScreen;
    public GameObject districtNew;
    public Text winScreenText1;
    public Text winScreenText2;
    public Text closeWinScreenText3;
    public Text attentionScreenText;



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
            return 1/*10 * Math.Pow(1.07, cLevel)*/;
        }
    }
    public int cLevel;
    public double cPower
    {
        get
        {
            return 2000000 * cLevel;
        }
    }

    //Background
    public Image enemy;

    public void Start()
    {
        offlineBox.gameObject.SetActive(false);
        multBox.gameObject.SetActive(false);
        Load();
        if (acceptY < 1)
        {
            welcomeText.gameObject.SetActive(true);
            welcomeText1.gameObject.SetActive(false);
            welcomeText2.gameObject.SetActive(false);
            accept.gameObject.SetActive(true);
            accept1.gameObject.SetActive(false);
            accept2.gameObject.SetActive(false);
            welcomeBox.gameObject.SetActive(true);
        }
        else
            welcomeBox.gameObject.SetActive(false);

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
        welcomeText.gameObject.SetActive(false);
        welcomeText1.gameObject.SetActive(true);
        welcomeText2.gameObject.SetActive(false);
    }
    public void NextPage1()
    {
        accept.SetActive(false);
        accept1.SetActive(false);
        accept2.SetActive(true);
        welcomeText.gameObject.SetActive(false);
        welcomeText1.gameObject.SetActive(false);
        welcomeText2.gameObject.SetActive(true);
    }
    public void Exit()
    {
        welcomeBox.gameObject.SetActive(false);
        acceptY += 1;
    }
    public void Reset()
    {
        acceptY = 0;
        money = 0;
        enemyTurn = 0;
        cLevel = 0;
        pLevel = 0;
        stage = 1;
        stageMax = 1;
        kills = 0;
        dpc = 1;
        dps = 0;
        health = healthCap;
        medal = 0;
    }
    public void NewGame() 
    {
        newGameCount++;
        dpc = 1;
        money = 0;
        enemyTurn = 0;
        cLevel = 0;
        pLevel = 0;
        stage = 1;
        stageMax = 1;
        kills = 0;
        dps = 0;
        medal = 0;
        health = healthCap;
        attentionScreen.gameObject.SetActive(false);
        districtNew.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (health <= 0) kill();
        else
        { health -= dps * Time.deltaTime; }
        //switch for boss
        BgChanger();
        if (newGameCount == 0)
        {
            switch (enemyTurn)
            {
                case 0:
                    enemyImg.sprite = enemy1;
                    break;
                case 1:
                    enemyImg.sprite = enemy2;
                    break;
                case 2:
                    enemyImg.sprite = enemy3;
                    break;
                case 3:
                    enemyImg.sprite = enemy4;
                    break;
            }
        }
        else
            switch (enemyTurn)
            {
                case 0:
                    enemyImg.sprite = enemy5;
                    break;
                case 1:
                    enemyImg.sprite = enemy6;
                    break;
                case 2:
                    enemyImg.sprite = enemy7;
                    break;
                case 3:
                    enemyImg.sprite = enemy8;
                    break;
            }
        //mult
        multValueMoney = multValue * moneyPerSec;
        multText.text = "$" + multValueMoney.ToString("F2");
        if (timerMult <= 0) multBox.gameObject.SetActive(true);
        else
            timerMult -= Time.deltaTime;

        moneyText.text = "$" + WordNotation(money, "F2");
        stageText.text = "этап - " + stage;
        killsText.text = kills + "/" + killsMax;
        healthText.text = WordNotation(health, "F2") + "/" + WordNotation(healthCap, "F2") + "HP";
        dPCText.text = WordNotation(dpc, "F2") + " за клик";
        dPSText.text = WordNotation(dps, "F2") + " за секунду";

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
        if (stageMax == 11 && medal < 1)
        {
            medal++;
            winScreen.SetActive(true);
            districtNew.gameObject.SetActive(true);
            winScreenText1.gameObject.SetActive(true);
            winScreenText2.gameObject.SetActive(false);
            nextWinScreen.SetActive(true);
            closeWinScreen.gameObject.SetActive(false);
        }
        //else 
        //{
        //    districtNew.gameObject.SetActive(false);
        //}
    }
    public void BgChanger()
    {
        if (newGameCount == 0)
        {
            switch (stage)
            {
                case 1:
                    background.sprite = bg1;
                    break;
                case 2:
                    background.sprite = bg2;
                    break;
                case 3:
                    background.sprite = bg3;
                    break;
                case 4:
                    background.sprite = bg4;
                    break;
                case 5:
                    background.sprite = bgBoss1;
                    break;
                case 6:
                    background.sprite = bg5;
                    break;
                case 7:
                    background.sprite = bg6;
                    break;
                case 8:
                    background.sprite = bg7;
                    break;
                case 9:
                    background.sprite = bg8;
                    break;
                case 10:
                    background.sprite = bgBoss1;
                    break;
                default:
                    background.sprite = bg3;
                    break;
            }
        }
        else
            switch (stage)
            {
                case 1:
                    background.sprite = bg9;
                    break;
                case 2:
                    background.sprite = bg10;
                    break;
                case 3:
                    background.sprite = bg11;
                    break;
                case 4:
                    background.sprite = bg12;
                    break;
                case 5:
                    background.sprite = bgBoss2;
                    break;
                case 6:
                    background.sprite = bg13;
                    break;
                case 7:
                    background.sprite = bg14;
                    break;
                case 8:
                    background.sprite = bg15;
                    break;
                case 9:
                    background.sprite = bg16;
                    break;
                case 10:
                    background.sprite = bgBoss2;
                    break;
                default:
                    background.sprite = bg12;
                    break;
            }
            
    }


    public void Upgrades()
    {
        cCostText.text = "цена: $" + WordNotation(cCost, "F2");
        cLevelText.text = "уровень: " + cLevel;
        cPowerText.text = "+ 2 за клик";

        pCostText.text = "цена: $" + WordNotation(pCost, "F2");
        pLevelText.text = "уровень: " + pLevel;
        pPowerText.text = "+ 5 в секунду";
        dps = pPower;
        if (newGameCount > 0) { dpc = 1 + cPower + (newGameCount*2); }
        else dpc = 1 + cPower;
    }

    public void IsBossChecker()
    {
        if (newGameCount > 0)
        {
            if (stage % 5 == 0 && stageMax < 11)
            {

                isBoss = 10;
                stageText.text = "(БОСС!) этап - " + stage;
                timer -= Time.deltaTime;
                if (timer <= 0) Back();
                
                timerText.text = timer + "/" + timerCap;
                timerBar.gameObject.SetActive(true);
                timerBG.gameObject.SetActive(true);
                timerBar.fillAmount = timer / timerCap;
                killsMax = 1;
                enemyImg.sprite = boss1;
            }
            else
            {
                isBoss = 1;
                stageText.text = "этап - " + stage;
                timerText.text = "";
                timerBar.gameObject.SetActive(false);
                timerBG.gameObject.SetActive(false);
                timer = 30;
                killsMax = 10;

            }
        }
        else
        {
            if (stage % 5 == 0 && stageMax < 11)
            {

                isBoss = 10;
                stageText.text = "(БОСС!) этап - " + stage;
                timer -= Time.deltaTime;
                if (timer <= 0) Back();

                timerText.text = timer + "/" + timerCap;
                timerBar.gameObject.SetActive(true);
                timerBG.gameObject.SetActive(true);
                timerBar.fillAmount = timer / timerCap;
                killsMax = 1;
                enemyImg.sprite = boss2;
            }
            else
            {
                isBoss = 1;
                stageText.text = "этап - " + stage;
                timerText.text = "";
                timerBar.gameObject.SetActive(false);
                timerBG.gameObject.SetActive(false);
                timer = 30;
                killsMax = 10;

            }
        }
    }


    public void Hit()
    {
        health -= dpc;
        Instantiate(effect, enemy.GetComponent<RectTransform>().position.normalized, Quaternion.identity);
        soundPlay.Play();
        enemyTap.Play("EnemyTap", 0, 0);
        if (health <= 0)
        {
            kill();
        }
    }

    public void kill()
    {
        money += Math.Ceiling(healthCap / 14);
        enemyTurn += 1;
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
        if (enemyTurn >= 4) 
        {
            enemyTurn = 0;
        }

        IsBossChecker();
        health = healthCap;
        if (isBoss > 1)
        {
            timer = timerCap;
        }
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
        PlayerPrefs.SetInt("meadl", medal);

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
        cLevel = PlayerPrefs.GetInt("cLevel", 1000);
        OfflineProgressCheck = PlayerPrefs.GetInt("OfflineProgressCheck", 0);
        acceptY = PlayerPrefs.GetInt("acceptY", 0);
        medal = PlayerPrefs.GetInt("medal", 0);
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

            offlineTimeText.text = "Вас не было: " + timer.ToString(@"hh\:mm\:ss") + "\n\nВы заработали: $" + moneyToEarn.ToString("F2");

        }
    }

    public void CloseOfflineBox()
    {
        offlineBox.gameObject.SetActive(false);
    }
    public void NextWinScreen() 
    {
        winScreenText1.gameObject.SetActive(false);
        winScreenText2.gameObject.SetActive(true);
        nextWinScreen.gameObject.SetActive(false);
        closeWinScreen.gameObject.SetActive(true);
        closeWinScreenText3.gameObject.SetActive(true);
    }
    public void CloseWinScreen() 
    {
        winScreen.gameObject.SetActive(false);
    }
    public void ShowAttention() 
    {
        attentionScreen.gameObject.SetActive(true);
    }
    public void CloseAttentionScreen() 
    {
        attentionScreen.gameObject.SetActive(false);
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
