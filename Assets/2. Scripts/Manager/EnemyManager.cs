using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    
    public List<Transform> spawnPoints;
    public List<GameObject> enemyList;

    public List<Transform> occupiedSpace;
    public List<GameObject> currEnemys;
    public Transform playerPos;

    public float intervalMin;
    public float intervalMax;

    // Uncomment this if wanted to set a max enemy in a same time
    // public int maxEnemy;

    private Level level;

    void Start()
    {
        occupiedSpace = new List<Transform>();
        currEnemys = new List<GameObject>();
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
            // Uncomment this if wanted to set a max enemy in a same time
            // if(occupiedSpace.Count < spawnPoints.Count && occupiedSpace.Count < maxEnemy){
            if(occupiedSpace.Count < spawnPoints.Count){
                SpawnEnemy();
            }
        }
    }

    public void SpawnEnemy(){
        // Get a spawnpoint
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        while(occupiedSpace.Contains(spawnPoint)){
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        }

        // TODO: this is computation power wastage, as it keep random to find, even for small amount of enemy
        // Get a prefab to generate, only check the one that is needed
        GameObject choosenPrefab = enemyList[Random.Range(0, enemyList.Count)];
        while(!level.enemy.Contains(choosenPrefab.GetComponentInChildren<EnemyScript>().enemyName)){
            choosenPrefab = enemyList[Random.Range(0, enemyList.Count)];
        }

        // Generated the enemy
        GameObject generatedObject = Instantiate(choosenPrefab, spawnPoint.localPosition, Quaternion.identity);
        generatedObject.GetComponentInChildren<EnemyScript>().SetEnemyProfile(playerPos);
        generatedObject.transform.parent = spawnPoint;
        currEnemys.Add(generatedObject);
        occupiedSpace.Add(spawnPoint);
    }

    public void releaseSlot(GameObject enemy, GameObject spawnPoint){
        if(occupiedSpace.Contains(spawnPoint.transform)){
            currEnemys.Remove(enemy);
            occupiedSpace.Remove(spawnPoint.transform);
        }
    }

    
}
