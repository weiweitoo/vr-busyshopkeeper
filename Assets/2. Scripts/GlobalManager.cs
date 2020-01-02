using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public GameObject playerController;
    public PlayerController GetPlayerController(){
        return playerController.GetComponent<PlayerController>();
    }
}
