using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    enum State{
        idle,
        Dead
    }
    public float hp;
    public string enemyName;
    public string projectileName;
    public GameObject projectile;
    public Mesh projectileMesh;
    public int damage;
    public float shootingSpeed;
    public float intervalMin;
    public float intervalMax;

    private Animator animatorComponent;
    private State currState;
    private Transform playerPosition;

    void Start(){
        animatorComponent = GetComponent<Animator>();
        currState = State.idle;
        StartCoroutine(ShootProjectileCoroutine());
    }

    public IEnumerator ShootProjectileCoroutine(){
        while(true){
            yield return new WaitForSeconds(Random.Range(intervalMin, intervalMax));
            ShootProjectile();
        }
    }

    public void Trigger(float damage){
        TakeDamage(damage);
    }

    public void SetEnemyProfile(Transform playerPos){
        playerPosition = playerPos;
        transform.LookAt(playerPos);
    }

    private void ShootProjectile(){
        GameObject generatedObject = Instantiate(projectile, transform.position, Quaternion.identity);
        ObjectController objectController = generatedObject.GetComponentInChildren<ObjectController>();
        objectController.setProfile(projectileMesh, BuyerScript.GoodsType.None, damage, false);
        objectController.Shoot(playerPosition.position, shootingSpeed);
    }

    private void TakeDamage(float dam){
        hp -= dam;
        // check if dead
        if(hp <= 0){
            currState = State.Dead;
            GameObject parentObject = transform.parent.gameObject;
            GameObject spawnPointObject = transform.parent.parent.gameObject;
            GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetEnemyManager().releaseSlot(parentObject, spawnPointObject);
            Destroy(parentObject);
        }
    }

}
