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
    public ParticleSystem healingEffect;

    private float regenTimer = 0f;
    public bool isHealing;
    private int regenCount = 0;

    void Start()
    {
        currHp = maxHp;
        isHealing = false;
    }

    void Update()
    {
        UpdateHPText();
        UpdateTimer();
        CheckHealthRegen();
        playHealingEffect();
    }

    public void Trigger(float damage)
    {
        TakeDamage(damage);
    }

    private void playHealingEffect()
    {
        print(healingEffect.isPlaying);
        if (isHealing && !healingEffect.isPlaying)
        {
            healingEffect.Play();
        }
        else if (!isHealing && healingEffect.isPlaying)
        {
            print("stopiuung");
            healingEffect.Stop();
        }
    }

    private void CheckHealthRegen()
    {
        // Check if can health(after interval and min), heal it then
        if (regenTimer > (healthMin + (regenCount * healInterval)))
        {
            if (isHealing == false)
            {
                isHealing = true;
            }
            heal(1);
            Debug.Log("Healing");
        }


    }

    private void ResetRegenTimer()
    {
        regenTimer = 0f;
        regenCount = 0;
        isHealing = false;
    }

    private void UpdateTimer()
    {
        regenTimer += Time.deltaTime;
    }

    private void UpdateHPText()
    {
        HPText.text = currHp + " HP";
    }

    private void heal(float healAmount)
    {
        currHp += healAmount;
        regenCount += 1;
    }

    private void TakeDamage(float dam)
    {
        currHp -= dam;
        ResetRegenTimer();
        if (currHp <= 0)
        {
            currHp = 0;
            GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().Dead();
        }
    }

}
