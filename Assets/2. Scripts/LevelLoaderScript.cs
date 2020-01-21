using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public struct Level{
    public string level;
    public List<string> stocks;
    public List<string> enemy;
    public List<string> weapon;
    public int normal_interval;
    public int wave_1;
    public int wave_2;
    public int total_length;
}

[System.Serializable]
public class Levels{
    public Level[] levels;
}

public enum WaveState{
    Warmup,
    Wave1,
    Buffer,
    Wave2
}

public class LevelLoaderScript : MonoBehaviour
{
    public GameObject waveProgressIndicator;
    public GameObject wave1Indicator;
    public GameObject wave2Indicator;
    public Text levelText;
    public float indicatorMax;
    public TextAsset jsonFile;
    public int currentLevel;
    public Levels levels;
    public BuyerManager buyerManager;
    public EnemyManager enemyManager;
    public WaveState waveState;

    private float timer_delta = 0f; 
    private float timer_second = 0f; 
    private float currTotalLength;
    private Level level;
    private RectTransform rectTransform;

    private void Awake() {
        currentLevel = PlayerPrefs.GetInt("Level", 0);
        levels = JsonUtility.FromJson<Levels>(jsonFile.text);
    }

    private void Start(){
        waveState = WaveState.Warmup;
        rectTransform = waveProgressIndicator.GetComponent<RectTransform>();
        level = GetLevelDetails();
        currTotalLength = GetLevelDetails().total_length;
        buyerManager.SetInterval(level.normal_interval-1-1, level.normal_interval+1-1); // buyer spawn faster 1 second then enemy
        enemyManager.SetInterval(level.normal_interval-1, level.normal_interval+1);
        SetupWaveIndicator();
        levelText.text = "Level: " + currentLevel;
    }

    private void Update(){
        UpdateTimer();
        UpdateWaveIndicator();
    }

    public bool NextLevel(){
        // Return true if next level, else return false stated all level completed
        if(currentLevel < levels.levels.Length){
            currentLevel += 1;
            currTotalLength = GetLevelDetails().total_length;
            return true;
        }
        else{
            return false;
        }
    }

    public Level GetLevelDetails(){
        return levels.levels[currentLevel];
    }

    private void UpdateTimer(){
        timer_delta += Time.deltaTime;
        timer_second = timer_delta % 60;
    }

    private void UpdateWaveState(){
        if(waveState == WaveState.Warmup && timer_second >= currTotalLength / 4){
            waveState = WaveState.Wave1;
            buyerManager.SetInterval(level.wave_1-1-1, level.wave_1+1-1); // buyer spawn faster 1 second then enemy
            enemyManager.SetInterval(level.wave_1-1, level.wave_1+1);
            Debug.Log("Enter Wave 1");
        }
        else if(waveState == WaveState.Wave1 && (timer_second >= (currTotalLength / 4) + 20)){
            waveState = WaveState.Buffer;
            buyerManager.SetInterval(level.normal_interval-1-1, level.normal_interval+1-1); // buyer spawn faster 1 second then enemy
            enemyManager.SetInterval(level.normal_interval-1, level.normal_interval+1);
            Debug.Log("Enter Buffer");
        }
        else if(waveState == WaveState.Buffer && (timer_second >= currTotalLength - 20)){
            waveState = WaveState.Wave2;
            buyerManager.SetInterval(level.wave_2-1-1, level.wave_2+1-1); // buyer spawn faster 1 second then enemy
            enemyManager.SetInterval(level.wave_2-1, level.wave_2+1);
            Debug.Log("Enter Wave 2");
        }
        else if(waveState == WaveState.Wave2 && (timer_second >= currTotalLength)){
            GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().SetWaveEnd();
            Debug.Log("End Game");
        }
    }

    private void SetupWaveIndicator(){
        float progressWave1 = indicatorMax * ((currTotalLength / 4) + 20) / currTotalLength;

        RectTransform rectTransform_1 = wave1Indicator.GetComponent<RectTransform>();
        rectTransform_1.anchoredPosition = new Vector2(progressWave1, 0);

        float progressWave2 = indicatorMax * (currTotalLength - 20) / currTotalLength;
        RectTransform rectTransform_2 = wave2Indicator.GetComponent<RectTransform>();
        rectTransform_2.anchoredPosition = new Vector2(progressWave2, 0);
    }

    private void UpdateWaveIndicator(){
        float progress = indicatorMax * (timer_second / currTotalLength);
        if(progress >= indicatorMax){
            progress = indicatorMax;
        }
        rectTransform.sizeDelta = new Vector2(progress, 4);
    }
}
