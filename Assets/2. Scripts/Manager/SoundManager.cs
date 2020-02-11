using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip levelWinAudio;
    public AudioClip levelLoseAudio;
    public AudioClip bossWaveAudio;

    private AudioSource audioSourceComponent;

    void Start() {
        audioSourceComponent = GetComponent<AudioSource>();
    }

    private IEnumerator playUntilEnd(AudioClip audio){
        PlayAudio(audio);
        float waitFor = audioSourceComponent.clip.length;
        yield return new WaitForSeconds(waitFor);
    }

    private void PlayAudio(AudioClip audio){
        audioSourceComponent.clip = audio;
        audioSourceComponent.Play();
    }

    public void playLevelWin(){
        PlayAudio(levelWinAudio);
    }

    public void playLevelLoss(){
        PlayAudio(levelLoseAudio);
    }

    public void playBossWave(){
        PlayAudio(bossWaveAudio);
    }
    
    public IEnumerator playLevelWinUntilEnd(){
        yield return playUntilEnd(levelWinAudio);
    }

    public IEnumerator playLevelLossUntilEnd(){
        yield return playUntilEnd(levelLoseAudio);
    }

    public IEnumerator playBossWaveUntilEnd(){
        yield return playUntilEnd(bossWaveAudio);
    }
}
