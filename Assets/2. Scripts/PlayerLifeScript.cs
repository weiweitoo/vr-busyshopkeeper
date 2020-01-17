using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeScript : MonoBehaviour
{

    public float maxHp;
    [ReadOnly] public float currHp;

    private void Start() {
        currHp = maxHp;
    }

    public void Trigger(float damage){
        TakeDamage(damage);
    }

    private void TakeDamage(float dam){
        currHp -= dam;
        if(currHp <= 0){
            Debug.Log("You are dead!");
        }
    }

}
