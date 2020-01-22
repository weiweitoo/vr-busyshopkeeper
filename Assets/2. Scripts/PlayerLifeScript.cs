using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeScript : MonoBehaviour
{

    public Text HPText;
    public float maxHp;
    public float currHp;

    public float healthMin;
    public float healInterval;
    private float timer_delta = 0f; 
    private float timer_second = 0f; 
    private bool isRegen;

    private void Start() {
        currHp = maxHp;
        isRegen = false;
    }

    private void Update(){
        UpdateHPText();
        UpdateTimer();
        CheckHealthRegen();
    }


    private void CheckHealthRegen(){
        if(timer_second > healthMin){
            isRegen = true;
        }
    }
    
    private void UpdateTimer(){
        timer_delta += Time.deltaTime;
        timer_second = timer_delta % 60;
    }

    private void UpdateHPText(){
        HPText.text =  currHp + " HP";
    }

    public void Trigger(float damage){
        TakeDamage(damage);
    }

    private void TakeDamage(float dam){
        currHp -= dam;
        if(currHp <= 0){
            currHp = 0;
            GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().Dead();
        }
    }

}
