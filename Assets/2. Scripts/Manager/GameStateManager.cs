using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tengio;
using UnityEngine.SceneManagement; //So you can use SceneManager

public class GameStateManager : MonoBehaviour
{
    [ReadOnly] public bool isTutorial;
    [ReadOnly] public bool isWaveEnd;

    public bool enemyEnd;
    public bool buyerEnd;
    public bool isPause;
    public int currLevel;

    void Awake() {
        currLevel = PlayerPrefs.GetInt("Level", 0);
        if(currLevel == 0){
            isTutorial = true;
        }
        else{
            isTutorial = false;
        }
    }

    void Start() {
        isWaveEnd = false;
        isPause = false;
        ShowLevelMessageBox();
    }

    private void ShowLevelMessageBox(){
        StartCoroutine(ShowLevelMessageBoxCoroutine());
    }

    private IEnumerator ShowLevelMessageBoxCoroutine(){
        // yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("Level: " + currLevel, 4.0f));
        if(currLevel == 1){
            yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("All the best!", 4.0f));
        }
        if(currLevel == 2){
            yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("Level: " + currLevel + "\n[New Challange]\n" + "Red Apple Buyer: Shorter\nKnight Enemy: Attack frequent and durable", 4.0f));
        }
        else if(currLevel == 3){
            yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("Level: " + currLevel + "\n[New Challange]\n" + "Orange Pear Buyer: Faster get rage\nViking Enemy: High damage but less attack", 4.0f));
        }
    }

    void Update() {
        if(isWaveEnd && isPause == false){
            enemyEnd = GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetEnemyManager().currEnemys.Count <= 0;
            buyerEnd = GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetBuyerManager().buyerList.Count <= 0;

            if(enemyEnd && buyerEnd){
                isPause = true;

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
        GetComponent<SceneLoader>().LoadScene("StartScene");
    }
    
    public void Dead(){
        isPause = true;
        StartCoroutine(DeadCoroutine());
    }

    public void SetWaveEnd(){
        isWaveEnd = true;
    }

    public bool IsGameStop(){
        return isPause || isWaveEnd;
    }

    private void NextLevel(){
        StartCoroutine(NextLevelCoroutine());
    }

    private IEnumerator SummonScoreboard(){
        float totalDamage = GameObject.Find("GameScoreBoardManager").GetComponent<GameScoreBoardManager>().GetDamage();
        float totalHeal = GameObject.Find("GameScoreBoardManager").GetComponent<GameScoreBoardManager>().GetHeal();
        float totalMercenary = GameObject.Find("GameScoreBoardManager").GetComponent<GameScoreBoardManager>().GetMercenary();
        float totalBuyerRage = GameObject.Find("GameScoreBoardManager").GetComponent<GameScoreBoardManager>().GetBuyerRage();
        float totalBuyerServe = GameObject.Find("GameScoreBoardManager").GetComponent<GameScoreBoardManager>().GetBuyerServe();
        double averageTask = GameObject.Find("GameScoreBoardManager").GetComponent<GameScoreBoardManager>().GetAverageTask();

        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonScoreBoard("Time Required/Task = " + averageTask + "s\nTotal Damage Taken = " + totalDamage + "\nCustomer Serve = " + totalBuyerServe + "\nCustomer Rage = " + totalBuyerRage + "\nMercenary Killed = " + totalMercenary + "\n", 10.0f));
        // yield return new WaitForSeconds(8.0f);
    }

    private IEnumerator NextLevelCoroutine(){
        yield return StartCoroutine(SummonScoreboard());
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetSoundManager().playLevelWinUntilEnd());
        Debug.Log("Move to next level");
        int newLevel = PlayerPrefs.GetInt("Level") + 1;
        PlayerPrefs.SetInt("Level", newLevel);
        // SceneManager.LoadScene("GamePlay");
        GetComponent<SceneLoader>().LoadScene("GamePlay");
        // Initiate.Fade("StartScene", Color.black, 3.0f);
    }

    private IEnumerator WinCoroutine(){
        yield return StartCoroutine(SummonScoreboard());
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetSoundManager().playLevelWinUntilEnd());
        Debug.Log("Won");
        int newLevel = PlayerPrefs.GetInt("Level") + 1;
        PlayerPrefs.SetInt("Level", newLevel);
        // SceneManager.LoadScene("GamePlay");
        GetComponent<SceneLoader>().LoadScene("StartScene");
    }

    private IEnumerator DeadCoroutine(){
        yield return StartCoroutine(SummonScoreboard());
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetSoundManager().playLevelLossUntilEnd());
        
        GetComponent<SceneLoader>().LoadScene("StartScene");
        // Initiate.Fade("StartScene", Color.black, 3.0f);
    }
}
