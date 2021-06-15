using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source of Explosion is NULL");
        }
        _audioSource.PlayOneShot(_explosionSoundClip);
        Destroy(this.gameObject, 3.0f);
    }
}
