using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{

    public GameObject fruitsParents;
    public Transform fruitSpawnPoint;
    public GameObject ProjectilePrefab;
    public List<Mesh> fruitMeshs;
    public List<int> damage;
    public int numFruit;

    void Start()
    {
        for (int i = 0; i < numFruit; i++)
        {
            int index = Random.Range(0, fruitMeshs.Count);
            Mesh choosenFruit = fruitMeshs[index];
            GameObject generatedObject = Instantiate(ProjectilePrefab, getSpawnLocation(), Quaternion.identity);
            generatedObject.GetComponentInChildren<ObjectController>().setProfile(choosenFruit, "Fruit", damage[index], true);
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

    private bool randomBoolean()
    {
        if (Random.value >= 0.5)
        {
            return true;
        }
        return false;
    }

}
