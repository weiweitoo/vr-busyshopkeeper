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
        StartCoroutine(Spawner());
        LoadLevel();
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
        while(true){
            yield return new WaitForSeconds(Random.Range(intervalMin, intervalMax));
            if(buyerList.Count < maxBuyer){
                // TODO: this is computation power wastage, as it keep random to find, even for small amount of enemy
                // Get a prefab to generate, only check the one that is needed
                GameObject choosenPrefab = spawnable[Random.Range(0, spawnable.Count)];
                Debug.Log(choosenPrefab.GetComponentInChildren<BuyerScript>().buyerName);
                while(!level.stocks.Contains(choosenPrefab.GetComponentInChildren<BuyerScript>().buyerName)){
                    choosenPrefab = spawnable[Random.Range(0, spawnable.Count)];
                }

                // Generated the buyer
                GameObject generatedObject = Instantiate(choosenPrefab, spawnPoint.localPosition, Quaternion.identity);
                generatedObject.GetComponentInChildren<BuyerScript>().SetBuyerProfile(centrePoint, 3.0f);
                generatedObject.transform.SetParent(buyerManager.transform);
                buyerList.Add(generatedObject);
            }
        }
    }

    public void releaseSlot(GameObject buyer){
        if(buyerList.Contains(buyer)){
            buyerList.Remove(buyer);
        }
    }

}
