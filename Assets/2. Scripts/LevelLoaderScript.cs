using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    
    public TextAsset jsonFile;
    public int currentLevel;
    public Levels levels;
    public BuyerManager buyerManager;
    public EnemyManager enemyManager;
    public WaveState waveState;
    public float timer_delta = 0f; 
    public float timer_second = 0f; 
    public float currTotalLength;
    private Level level;

    private void Start(){
        currentLevel = 0;
        waveState = WaveState.Warmup;
        levels = JsonUtility.FromJson<Levels>(jsonFile.text);
        level = GetLevelDetails();
        currTotalLength = GetLevelDetails().total_length;
        buyerManager.SetInterval(level.normal_interval-1-1, level.normal_interval+1-1); // buyer spawn faster 1 second then enemy
        enemyManager.SetInterval(level.normal_interval-1, level.normal_interval+1);
    }

    private void Update(){
        UpdateTimer();
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
        if(waveState == WaveState.Warmup && timer_second >= currTotalLength * 4){
            waveState = WaveState.Wave1;
            buyerManager.SetInterval(level.wave_1-1-1, level.wave_1+1-1); // buyer spawn faster 1 second then enemy
            enemyManager.SetInterval(level.wave_1-1, level.wave_1+1);
            Debug.Log("Enter Wave 1");
        }
        else if(waveState == WaveState.Wave1 && (timer_second >= (currTotalLength * 4) + 20)){
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
            Debug.Log("End Game");
        }
    }
}
