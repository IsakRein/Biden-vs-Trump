using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LocalSoundManager : MonoBehaviour
{
    public AudioManager audioManager;

    public UnityEvent start;
    public UnityEvent onDestroy;


    private void Start() {
        audioManager = FindObjectOfType<AudioManager>();
        start.Invoke();
        Debug.Log("Playing");
        Debug.Log(audioManager);
    }

    private void OnDestroy() {
        onDestroy.Invoke();
    }

    public void Play(string sound) {
        audioManager.Play(sound);
    }
    public void Stop(string sound) {
        audioManager.Stop(sound);
    }
    public void Pause(string sound) {
        audioManager.Pause(sound);
    }
    public void UnPause(string sound) {
        audioManager.UnPause(sound);
    }
    public void IfNotPlayingPlay(string name) {
        audioManager.IfNotPlayingPlay(name);
    }
}
