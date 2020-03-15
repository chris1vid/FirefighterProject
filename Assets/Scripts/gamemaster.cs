using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;

public class gamemaster : MonoBehaviour
{
    /* pop your head
    Look up how to set up screen stretches and etc so ui looks good
    look up how to set up an image banner and get rid of all inputs that aren't (pause, fire, turn, or accept)

    */

    public float currScore;
    float hiScore;
    public Text hiScoreTxt;
    float currLevel = 0;
    private float[] hiScoreList;
    private float totalWindows;
    private float lastFire;
    public float timeTillNextFire = 20;
    [SerializeField] private Transform fireplaces;
    int firesLit = 1;
    bool loading;
    float timeLeft;
    public float waterLevels = 55;
    public int extinguishedFires = 0;
    private float maxTimeLeft = 99;
    public float structureHP = 99;
    [SerializeField] private Text activeFiresTx;
    [SerializeField] private Text waterLevelTx;
    [SerializeField] private Text structureHPTx;
    [SerializeField] private Text timeLeftTx;
    [SerializeField] private Text currScoreTx;
    //
    [SerializeField] private Text timeLeftScoreTx;
    [SerializeField] private Text structureHPScoreTx;
    [SerializeField] private Text totalScoreTx;
    [SerializeField] private Text levelBonusScoreTx;
    [SerializeField] private Text waterLevelsScoreTx;
    [SerializeField] private Text windowsLeftScoreTx;
    [SerializeField] private Text highScoreTx;
    [SerializeField] private Button nextLevelBtn;
    [SerializeField] private Text lvlCompleteTx;
    //
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private TogglePanel togglePanel;


    // Use this for initialization
    void Start()
    {
        currScore = 0;
        LoadLevel();
    }
    /*for each window in the level, windowstotal++, assign windowstotal to that window

    for 1-5 times based on the current level's number halved
    -cause a random(1, windowstotal) window to fire up
    -foreach window in the game, if window.number = random then window goes into flame
    -if the window is aalready firey, or is extinguished, re-do this process one more time
    Update to check if time > lastfire + firetimer
    if yes then start a new fire, updating lastfire
        */
    // Update is called once per frame
    void Update()
    {
        UpdateStats();
        if (Time.time > lastFire + timeTillNextFire && firesLit < totalWindows && !loading && levelEnded == false)
        {
            Lightfire();
        }
        if (Time.time > 5 && extinguishedFires == firesLit && levelEnded == false)
        {
            EndLevel(true);
        }
    }

    void UpdateScore()
    {
        currScoreTx.text = currScore.ToString("00000000");
    }

    bool levelEnded = false;

    void UpdateStats()
    {
        if (!levelEnded)
        {
            if (firesLit - extinguishedFires > 0)
            {
                activeFiresTx.text = (firesLit - extinguishedFires).ToString();
            }
            else
            {
                levelEnded = true;
                EndLevel(true);
            }

            if (waterLevels > 0)
            {
                waterLevelTx.text = waterLevels.ToString("00") + " %";
            }
            else
            {
                levelEnded = true;
                EndLevel(false);
                waterLevelTx.text = "00";
            }

            if (maxTimeLeft - Mathf.Ceil(Time.time) > 0)
            {
                timeLeftTx.text = (maxTimeLeft - Mathf.Ceil(Time.time)).ToString("00");
            }
            else
            {
                levelEnded = true;
                EndLevel(false);
                timeLeftTx.text = "00";
            }

            if (Mathf.Ceil(structureHP) > 0)
            {
                structureHPTx.text = Mathf.Ceil(structureHP).ToString("00");
            }
            else
            {
                levelEnded = true;
                EndLevel(false);
                structureHPTx.text = "00";
            }
        }
    }
    /*Endlevel has to reset all the fires to their defaults
    and aside from that, must reset all stats 
    Create a "level complete" panel. should include score calculation.
    Easy! You can do this!
    */

    void EndLevel(bool win)
    {
        if (win)
        {
            currScore += (totalWindows - firesLit) * 150; // add endlevel score counter?
            windowsLeftScoreTx.text = "Prevention Bonus: " + (150 * (totalWindows - firesLit)).ToString("00000000");
            currScore += 100 * Mathf.Pow(currLevel, 2);
            levelBonusScoreTx.text = "Level Bonus: " + (100 * Mathf.Pow(currLevel, 2)).ToString("00000000");
            currScore += 1000 * Mathf.Ceil(waterLevels);
            waterLevelsScoreTx.text = "Water Bonus: " + (1000 * Mathf.Ceil(waterLevels)).ToString("00000000");
            currScore -= 10 * Mathf.Ceil(99 - structureHP);
            structureHPScoreTx.text = "Integrity Penalty: -" + (10 * Mathf.Ceil(99 - structureHP)).ToString("00000000");
            currScore += (100 * (maxTimeLeft - Mathf.Ceil(Time.time)) - (80 - (5 * currLevel)));
            timeLeftScoreTx.text = "Time Bonus: " + (100 * (maxTimeLeft - Mathf.Ceil(Time.time) - (80 - (5 * currLevel)))).ToString("00000000");

            totalScoreTx.text = "Total Score: " + currScore.ToString("00000000");

            if (currLevel >= 10)
            {
                lvlCompleteTx.text = "You Win!";
                nextLevelBtn.enabled = false; 
            } 

                togglePanel.PanelToggle(4);
           

            if (currScore <= 0)
            {
                currScore = 0;
            }

            if (currScore > LoadScore())
            {
                SaveScore();
                
            }
            highScoreTx.text = "HighScore: " + (LoadScore()).ToString("00000000");

        }
        else
        {
            togglePanel.PanelToggle(3);
        }

        Debug.Log("levelended");

        foreach (Transform x in fireplaces)
        {
            if (x.tag == "Fire")
            {
                Debug.Log("ok");
                var systemsb = x.gameObject.GetComponentsInChildren<ParticleSystem>();
                foreach (var system in systemsb)
                {
                    var emission = system.emission;
                    emission.enabled = true;
                }
                x.gameObject.GetComponent<Extinguishing>().DisableCheck();
                x.gameObject.SetActive(false);
            }
        }

       // togglePanel.doPause();
    }

    public void LoadLevel()
    {
        levelEnded = false;
        currLevel++;
        timeTillNextFire = 20 / currLevel;
        totalWindows = 0;
        firesLit = 0;
        extinguishedFires = 0;
        maxTimeLeft = Time.time + 99;
        waterLevels = currLevel * 5 + 50;
        if (waterLevels > 99)
        {
            waterLevels = 99;
        }
        foreach (Transform x in fireplaces)
        {
            if (x.tag == "Fire")
            {
                totalWindows++;
                x.transform.gameObject.GetComponent<Extinguishing>().fireNum = totalWindows;
            }
        }
        for (int i = 0; i < Mathf.Ceil(currLevel / 2); i++)
        {
            Lightfire();
        }
        currScore = 0;
        togglePanel.PanelToggle(0);
    }
    /* each fire extinguished adds to extinguishedfires
    if extinguishedfires is equal to fireslit then stop the game*/

    void Lightfire()
    {
        bool fireLit = false;
        do
        {
            float fireToLight = Mathf.Round(Random.Range(1, totalWindows));


            foreach (Transform x in fireplaces)
            {
                //Debug.Log(x.gameObject.activeInHierarchy + " " + fireToLight + " " + x.transform.gameObject.GetComponent<Extinguishing>().fireNum);
                if (x.transform.gameObject.GetComponent<Extinguishing>().fireNum == fireToLight && x.tag == "Fire" && x.gameObject.activeInHierarchy == false)
                {
                    x.transform.gameObject.SetActive(true);
                    fireLit = true;
                    lastFire = Time.time;
                    firesLit++;
                } // insert code to check if all windows are occupied, if so do not run this code anymore
            }
        } while (!fireLit);
    }



    void SaveScore()
    {

        PlayerPrefs.SetFloat("HighScore_" + currLevel, currScore);

    }

    float LoadScore()
    {
        return PlayerPrefs.GetFloat("HighScore_" + currLevel, 0);
    }

}
/* at the start of a game, load the playerprefs for highscore
foreach playerpref highschore, playerpref[x] = highscore[x]

when level finishes, compare currscroe to highscore[x]
if curr is higher than highschore[x] then replace highscore[x] with currscore
and save highscore[] as a playerpref
*/