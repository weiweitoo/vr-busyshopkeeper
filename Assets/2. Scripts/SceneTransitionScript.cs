using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionScript : MonoBehaviour
{
    public string sceneName;

    public void TransitionTo(){
        Debug.Log("123123");
        SceneManager.LoadScene(sceneName);
    }
}
