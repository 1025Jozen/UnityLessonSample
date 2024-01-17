using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartticlePlay : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            Play();
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            Stop();
        }
    }




    // 1. 再生
    private void Play() {
        particle.Play();
    }

    // 2. 一時停止
    private void Pause() {
        particle.Pause();
    }

    // 3. 停止
    private void Stop() {
        particle.Stop();
    }
}
