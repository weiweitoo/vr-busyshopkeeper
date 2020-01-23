using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject weaponsParents;
    public Transform weaponSpawnPoint;
    public GameObject ProjectilePrefab;
    public List<Mesh> weaponMeshs;
    public List<string> weaponName;
    public List<BuyerScript.GoodsType> goodsType;
    public List<int> damage;
    public int numweapon;
    public Transform playerPosition;

    private Level level;

    void Start()
    {
        LoadLevel();

        // If is not tutorial then start main gameplay
        if(!GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().getIsTutorial()){
            GenerateWeapon(numweapon);
        }
    }

    public void GenerateWeapon(int weapon){
        for (int i = 0; i < weapon; i++)
        {
            // Get only the containable name
            int index = Random.Range(0, weaponName.Count);
            string choosenWeaponName = weaponName[index];
            while(!level.weapon.Contains(choosenWeaponName)){
                index = Random.Range(0, weaponName.Count);
                choosenWeaponName = weaponName[index];
            }

            // Retreive out the weapon mesh
            Mesh choosenWeapon = weaponMeshs[index];
            GameObject generatedObject = Instantiate(ProjectilePrefab, getSpawnLocation(), Quaternion.identity);
            generatedObject.GetComponentInChildren<ObjectController>().setProfile(playerPosition, choosenWeapon, goodsType[index], damage[index], true);
            generatedObject.transform.parent = weaponsParents.transform;
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
            newX = weaponSpawnPoint.localPosition.x + RandomOfRanges(-0.2f, -1f, 0.2f, 1f);
            newZ = weaponSpawnPoint.localPosition.z + RandomOfRanges(-0.2f, -1f, 0.2f, 1f);
            newPos = new Vector3(newX, weaponSpawnPoint.localPosition.y, newZ);
            distance = Vector3.Distance(newPos, weaponSpawnPoint.localPosition);
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
