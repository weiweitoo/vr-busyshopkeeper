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

    private float regenTimer = 0f; 
    private bool isRegen;
    private int regenCount = 0;

    void Start() {
        currHp = maxHp;
        isRegen = false;
    }

    void Update(){
        UpdateHPText();
        UpdateTimer();
        CheckHealthRegen();
    }

    public void Trigger(float damage){
        TakeDamage(damage);
    }


    private void CheckHealthRegen(){
        // Check if can health(after interval and min), heal it then
        if(regenTimer > (healthMin + (regenCount * healInterval))){
            isRegen = true;
            heal(1);
            Debug.Log("Healing");
        }
    }
    
    private void ResetRegenTimer(){
        regenTimer = 0f;
        regenCount = 0;
    }
    
    private void UpdateTimer(){
        regenTimer += Time.deltaTime;
    }

    private void UpdateHPText(){
        HPText.text =  currHp + " HP";
    }

    private void heal(float healAmount){
        currHp += healAmount;
        regenCount += 1;
    }

    private void TakeDamage(float dam){
        currHp -= dam;
        ResetRegenTimer();
        if(currHp <= 0){
            currHp = 0;
            GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().Dead();
        }
    }

}
