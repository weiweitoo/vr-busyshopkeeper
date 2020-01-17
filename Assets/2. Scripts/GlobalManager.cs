using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public GameObject playerController;
    public GameObject enemyManager;
    public GameObject buyerManager;
    public GameObject levelLoaderManager;
    public PlayerController GetPlayerController(){
        return playerController.GetComponent<PlayerController>();
    }

    public EnemyManager GetEnemyManager(){
        return enemyManager.GetComponent<EnemyManager>();
    }

    public BuyerManager GetBuyerManager(){
        return buyerManager.GetComponent<BuyerManager>();
    }

    public LevelLoaderScript GetLevelManager(){
        return levelLoaderManager.GetComponent<LevelLoaderScript>();
    }
}

