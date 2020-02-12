using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public GameObject gameStateManager;
    public GameObject playerController;
    public GameObject enemyManager;
    public GameObject buyerManager;
    public GameObject levelLoaderManager;
    public GameObject messageBoxManager;
    public GameObject fruitManager;
    public GameObject soundManager;
    public GameObject gameScoreBoardManager;
    public Transform playerPosition;

    public Transform GetPlayerPosition(){
        return playerPosition;
    }

    public GameStateManager GetGameStateManager(){
        print(gameStateManager);
        return gameStateManager.GetComponent<GameStateManager>();
    }
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

    public MessageBoxManager GetMessageBoxManager(){
        return messageBoxManager.GetComponent<MessageBoxManager>();
    }

    public FruitManager GetFruitManager(){
        return fruitManager.GetComponent<FruitManager>();
    }

    public SoundManager GetSoundManager(){
        return soundManager.GetComponent<SoundManager>();
    }

    public GameScoreBoardManager GetGameScoreManager(){
        return gameScoreBoardManager.GetComponent<GameScoreBoardManager>();
    }
}

