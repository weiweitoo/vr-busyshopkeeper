using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyScript : MonoBehaviour
{
    enum State
    {
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
    public AudioClip enemyDamageAudio;
    public AudioClip enemyDeadAudio;
    public AudioClip enemyAttackAudio;


    private AudioSource audioSourceComponent;
    private Animator animatorComponent;
    private State currState;
    private Transform playerPosition;
    private DissolveScript dissolveScript;

    void Start()
    {
        audioSourceComponent = GetComponent<AudioSource>();
        animatorComponent = GetComponent<Animator>();
        dissolveScript = GetComponent<DissolveScript>();
        currState = State.idle;
        StartCoroutine(ShootProjectileCoroutine());
    }

    void Update() {

        // // if it is dead, stop the update, it will self destroy later
        // if(currState == State.Dead){
        //     return;
        // }
    }

    public IEnumerator ShootProjectileCoroutine()
    {
        while (true)
        {
            // if it is dead, stop the action, it will self destroy later
            if(currState == State.Dead || GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().IsGameStop()){
                break;
            }

            yield return new WaitForSeconds(Random.Range(intervalMin, intervalMax));
            ShootProjectile();
        }
    }

    public void Trigger(float damage)
    {
        TakeDamage(damage);
    }

    public void SetEnemyProfile(Transform playerPos)
    {
        playerPosition = playerPos;
        transform.LookAt(playerPos);
    }

    private void ShootProjectile()
    {
        PlayAudio(enemyAttackAudio);
        GameObject generatedObject = Instantiate(projectile, transform.position, Quaternion.identity);
        ObjectController objectController = generatedObject.GetComponentInChildren<ObjectController>();
        objectController.setProfile(playerPosition, projectileMesh, BuyerScript.GoodsType.None, damage, false);
        objectController.Shoot(playerPosition.position, shootingSpeed);
    }

    private void TakeDamage(float dam)
    {
        StartCoroutine(TakeDamageCoroutine(dam));
    }

    private void PlayAudio(AudioClip audio){
        audioSourceComponent.clip = audio;
        audioSourceComponent.Play();
    }

    private IEnumerator TakeDamageCoroutine(float dam)
    {
        hp -= dam;
        // check if dead
        if (hp <= 0)
        {
            PlayAudio(enemyDeadAudio);
            yield return StartCoroutine(dissolveScript.Dissolve());
            currState = State.Dead;
            GameObject parentObject = transform.parent.gameObject;
            GameObject spawnPointObject = transform.parent.parent.gameObject;
            GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetEnemyManager().releaseSlot(parentObject, spawnPointObject);
            Destroy(parentObject);
        }
        else{
            PlayAudio(enemyDamageAudio);
        }
        yield return null;
    }

}
