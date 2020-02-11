using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionScript : MonoBehaviour
{
    public string sceneName;
    public Color transitionColor;

    public void TransitionTo(){
        Initiate.Fade(sceneName, transitionColor, 3.0f);
    }
}
