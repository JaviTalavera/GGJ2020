using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesInteraction : MonoBehaviour
{
    public ParticleSystem _particles;
    public PassAudioToNextScene passAudio;

    void OnMouseDown()
    {
        if (_particles)
        {
            _particles.Play();
            passAudio.Play(5);
        }
    }
}
