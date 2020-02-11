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
        ShowLevelMessageBox();
    }

    private void ShowLevelMessageBox(){
        StartCoroutine(ShowLevelMessageBoxCoroutine());
    }

    private IEnumerator ShowLevelMessageBoxCoroutine(){
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("Level: " + PlayerPrefs.GetInt("Level", 0), 4.0f));
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
        Initiate.Fade("StartScene", Color.black, 3.0f);
    }
    
    public void Dead(){
        StartCoroutine(DeadCoroutine());
    }

    public void SetWaveEnd(){
        isWaveEnd = true;
    }

    private void NextLevel(){
        StartCoroutine(NextLevelCoroutine());
    }

    private IEnumerator SummonScoreboard(){
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonScoreBoard("Time Required/Task = 2.34s\nTotal Damage Taken = 16\nCustomer Serve = 14\nMercenary Killed = 10\n", 8.0f));
        // yield return new WaitForSeconds(8.0f);
    }

    private IEnumerator NextLevelCoroutine(){
        StartCoroutine(SummonScoreboard());
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetSoundManager().playLevelWinUntilEnd());
        Debug.Log("Move to next level");
        int newLevel = PlayerPrefs.GetInt("Level") + 1;
        PlayerPrefs.SetInt("Level", newLevel);
        // SceneManager.LoadScene("GamePlay");
        Initiate.Fade("StartScene", Color.black, 3.0f);
    }

    private IEnumerator DeadCoroutine(){
        StartCoroutine(SummonScoreboard());
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetSoundManager().playLevelLossUntilEnd());
        Initiate.Fade("StartScene", Color.black, 3.0f);
    }
}
