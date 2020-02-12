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
    public AudioClip healingSound;

    private float regenTimer = 0f;
    public bool isHealing;
    private int regenCount = 0;
    private bool allowHealing = false;
    private AudioSource audioSourceComponent;

    void Start()
    {
        currHp = maxHp;
        isHealing = false;
        audioSourceComponent = GetComponent<AudioSource>();

        // If is not tutorial then start main gameplay
        if(!GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().getIsTutorial()){
            allowHealing = true;
        }
    }

    void Update()
    {
        if(allowHealing == true && currHp < maxHp){
            UpdateHPText();
            UpdateTimer();
            CheckHealthRegen();
            playHealingEffect();
        }
    }

    public void Trigger(float damage)
    {
        TakeDamage(damage);
    }

    private void playHealingEffect()
    {
        if (isHealing && !healingEffect.isPlaying)
        {
            PlayHealingSound(true);
            healingEffect.Play();
        }
        else if (!isHealing && healingEffect.isPlaying)
        {
            PlayHealingSound(false);
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
        }


    }

    private void PlayHealingSound(bool play){
        if(play == true){
            audioSourceComponent.clip = healingSound;
            audioSourceComponent.Play();
        }
        else{
            audioSourceComponent.Stop();
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
        GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameScoreManager().GetComponent<GameScoreBoardManager>().AddHeal();
    }

    private void TakeDamage(float dam)
    {
        currHp -= dam;
        ResetRegenTimer();
        GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameScoreManager().GetComponent<GameScoreBoardManager>().AddDamage();
        if (currHp <= 0)
        {
            currHp = 0;
            GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().Dead();
        }
    }

}
