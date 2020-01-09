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

    
    void Start()
    {
        StartCoroutine(Spawner());
    }
    
    public IEnumerator Spawner()
    {
        while(true){
            yield return new WaitForSeconds(Random.Range(intervalMin, intervalMax));
            GameObject choosenPrefab = spawnable[Random.Range(0, spawnable.Count)];
            GameObject generatedObject = Instantiate(choosenPrefab, spawnPoint.localPosition, Quaternion.identity);
            generatedObject.GetComponentInChildren<BuyerScript>().SetBuyerProfile(centrePoint, 3.0f);
        }

    }

}
