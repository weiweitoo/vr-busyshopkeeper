using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //So you can use SceneManager

public class GameStateManager : MonoBehaviour
{
    [ReadOnly] public bool isTutorial;
    [ReadOnly] public bool isWaveEnd;

    public bool enemyEnd;
    public bool buyerEnd;

    void Awake() {
        if(PlayerPrefs.GetInt("Level", 0) == 0){
            isTutorial = true;
        }
        else{
            isTutorial = false;
        }
    }

    void Start() {
        isWaveEnd = false;
    }

    private void Update() {
        if(isWaveEnd){
            enemyEnd = GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetEnemyManager().currEnemys.Count <= 0;
            buyerEnd = GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetBuyerManager().buyerList.Count <= 0;

            if(enemyEnd && buyerEnd){

                // Total 4 level(0,1,2,3)
                if(PlayerPrefs.GetInt("Level") >= 2){
                    Win();
                }
                else{
                    NextLevel();
                }
            }
        }
    }

    public bool getIsTutorial(){
        return isTutorial;
    }

    public bool getIsEnd(){
        return isWaveEnd;
    }

    public void Win(){
        Initiate.Fade("StartScene", Color.white, 3.0f);
    }
    
    public void Dead(){
        Initiate.Fade("StartScene", Color.red, 3.0f);
    }

    public void SetWaveEnd(){
        isWaveEnd = true;
    }

    private void NextLevel(){
        Debug.Log("Move to next level");
        int newLevel = PlayerPrefs.GetInt("Level") + 1;
        PlayerPrefs.SetInt("Level", newLevel);
        SceneManager.LoadScene("GamePlay");
    }
}
