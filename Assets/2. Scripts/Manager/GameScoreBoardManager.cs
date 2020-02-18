using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameScoreBoardManager : MonoBehaviour
{
    private float totalTime;
    private float totalMercenary;
    private float totalBuyerServe;
    private float totalBuyerRage;
    private float totalHeal;
    private float totalDamageReceived;
    private float timer_delta = 0f; 

    void Update(){
        if(!GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().IsGameStop()){
            UpdateTimer();
        }
    }

    private void UpdateTimer(){
        timer_delta += Time.deltaTime;
        totalTime = timer_delta % 60;
    }

    public void AddMercenary(){
        totalMercenary += 1;
    }

    public void AddBuyerServe(){
        totalBuyerServe += 1;
    }

    public void AddBuyerRage(){
        totalBuyerRage += 1;
    }

    public void AddHeal(){
        totalHeal += 1;
    }

    public void AddDamage(){
        totalDamageReceived += 1;
    }

    public float GetDamage(){
        return totalDamageReceived;
    }

    public float GetHeal(){
        return totalHeal;
    }

    public float GetBuyerRage(){
        return totalBuyerRage;
    }

    public float GetBuyerServe(){
        return totalBuyerServe;
    }

    public float GetMercenary(){
        return totalMercenary;
    }

    public double GetAverageTask(){
        return Math.Round(totalTime / (totalBuyerServe + totalMercenary), 2);
    }
}

