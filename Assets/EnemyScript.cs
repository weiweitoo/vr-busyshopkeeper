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
    public float movingSpeed;
    public GameObject weapon;

    public string enemyName;
    private Animator animatorComponent;
    private State currState;

    void Start()
    {
        animatorComponent = GetComponent<Animator>();
        SetEnemyProfile(2, 3, 3);
        currState = State.idle;
    }

    void Update()
    {
        
    }

    public void SetEnemyProfile(float hp, float movingSpeed, float damage){
        this.hp = hp;
        this.movingSpeed = movingSpeed;
    }

    public void Trigger(float damage){
        TakeDamage(damage);
    }

    private void TakeDamage(float dam){
        hp -= dam;
        // check if dead
        if(hp <= 0){
            currState = State.Dead;
            Destroy(gameObject);
        }
    }
}
