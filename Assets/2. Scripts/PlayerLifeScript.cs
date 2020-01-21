using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeScript : MonoBehaviour
{

    public Text HPText;
    public float maxHp;
    [ReadOnly] public float currHp;

    private void Start() {
        currHp = maxHp;
    }

    private void Update(){
        UpdateHPText();
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
            Debug.Log("You are dead!");
            GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().Dead();
        }
    }

}
