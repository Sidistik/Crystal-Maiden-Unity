using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MovementSoundPlayer : MonoBehaviour
{
    [SerializeField] private float movementThreshold = 0.1f; 
    [SerializeField] private AudioClip movementClip;
    [SerializeField] private float volume = 30f;

    private AudioSource audioSource;
    private Vector3 lastPosition;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = movementClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        float movementSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;

        if (movementSpeed > movementThreshold)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Pause();
        }

        lastPosition = transform.position;
    }
}

