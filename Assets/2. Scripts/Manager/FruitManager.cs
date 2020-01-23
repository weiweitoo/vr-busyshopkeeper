using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    public GameObject fruitsParents;
    public Transform fruitSpawnPoint;
    public GameObject ProjectilePrefab;
    public List<Mesh> fruitMeshs;
    public List<string> fruitName;
    public List<BuyerScript.GoodsType> goodsType;
    public List<int> damage;
    public int numFruit;
    public Transform playerPosition;

    private Level level;

    void Start()
    {
        LoadLevel();

        // If is not tutorial then start main gameplay
        if(!GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().getIsTutorial()){
            GenerateFruit(numFruit);
        }
    }

    public void GenerateFruit(int fruit){
        for (int i = 0; i < fruit; i++)
        {
            // Get only the containable name
            int index = Random.Range(0, fruitName.Count);
            string choosenFruitName = fruitName[index];
            while(!level.stocks.Contains(choosenFruitName)){
                index = Random.Range(0, fruitName.Count);
                choosenFruitName = fruitName[index];
            }

            // Retreive out the fruit mesh
            Mesh choosenFruit = fruitMeshs[index];
            GameObject generatedObject = Instantiate(ProjectilePrefab, getSpawnLocation(), Quaternion.identity);
            generatedObject.GetComponentInChildren<ObjectController>().setProfile(playerPosition, choosenFruit, goodsType[index], damage[index], true);
            generatedObject.transform.parent = fruitsParents.transform;
        }
    }

    private Vector3 getSpawnLocation()
    {
        float distance = 0;
        float newX = 0;
        float newZ = 0;
        Vector3 newPos = new Vector3(0, 0, 0);
        while (distance <= 1)
        {
            newX = fruitSpawnPoint.localPosition.x + RandomOfRanges(-0.2f, -1f, 0.2f, 1f);
            newZ = fruitSpawnPoint.localPosition.z + RandomOfRanges(-0.2f, -1f, 0.2f, 1f);
            newPos = new Vector3(newX, fruitSpawnPoint.localPosition.y, newZ);
            distance = Vector3.Distance(newPos, fruitSpawnPoint.localPosition);
        }
        return newPos;
    }

    private float RandomOfRanges(float r1start, float r1end, float r2start, float r2end)
    {
        return randomBoolean() ? Random.Range(r1start, r1end) : Random.Range(r2start, r2end);
    }

    private void LoadLevel(){
        level = GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetLevelManager().GetLevelDetails();
    }

    private bool randomBoolean()
    {
        if (Random.value >= 0.5)
        {
            return true;
        }
        return false;
    }

}
