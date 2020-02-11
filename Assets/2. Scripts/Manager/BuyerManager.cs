using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyerManager : MonoBehaviour
{

    public List<GameObject> spawnable;
    public float intervalMin;
    public float intervalMax;
    public Transform centrePoint;
    public Transform spawnPoint;
    public GameObject buyerManager;
    public List<GameObject> buyerList;
    public int maxBuyer;

    private Level level;
    
    void Start()
    {
        LoadLevel();

        // If is not tutorial then start main gameplay
        if(!GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().getIsTutorial()){
            StartCoroutine(Spawner());
        }
    }

    private void LoadLevel(){
        level = GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetLevelManager().GetLevelDetails();
    }

    public void SetInterval(int min, int max){
        intervalMin = min;
        intervalMax = max;
    }
    
    public IEnumerator Spawner()
    {
        while(!GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().getIsEnd()){
            yield return new WaitForSeconds(Random.Range(intervalMin, intervalMax));
            if(buyerList.Count < maxBuyer){
                SpawnBuyer();
            }
        }
    }

    public GameObject SpawnBuyer(){
        GameObject choosenPrefab = spawnable[Random.Range(0, spawnable.Count)];
        // Debug.Log(choosenPrefab.GetComponentInChildren<BuyerScript>().buyerName);
        while(!level.stocks.Contains(choosenPrefab.GetComponentInChildren<BuyerScript>().buyerName)){
            choosenPrefab = spawnable[Random.Range(0, spawnable.Count)];
        }
        // Generated the buyer
        GameObject generatedObject = Instantiate(choosenPrefab, spawnPoint.localPosition, Quaternion.identity);
        generatedObject.GetComponentInChildren<BuyerScript>().SetBuyerProfile(centrePoint, 3.0f);
        generatedObject.transform.SetParent(buyerManager.transform);
        buyerList.Add(generatedObject);
        return generatedObject;
    }

    public GameObject SpawnBuyerByIndex(int index){
        if (index > spawnable.Count){
            Debug.Log("Out of range for SpawnBuyerByIndex for index " + index);
            return null;
        }

        GameObject choosenPrefab = spawnable[index];

        // Generated the buyer
        GameObject generatedObject = Instantiate(choosenPrefab, spawnPoint.localPosition, Quaternion.identity);
        generatedObject.GetComponentInChildren<BuyerScript>().SetBuyerProfile(centrePoint, 3.0f);
        generatedObject.transform.SetParent(buyerManager.transform);
        buyerList.Add(generatedObject);
        return generatedObject;
    }

    public void releaseSlot(GameObject buyer){
        if(buyerList.Contains(buyer)){
            buyerList.Remove(buyer);
        }
    }

    public int getBuyerCount(){
        return buyerList.Count;
    }
}
