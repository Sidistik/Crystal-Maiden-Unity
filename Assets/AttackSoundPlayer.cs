using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip attackSoundClip;
    [SerializeField] private float volume = 1f;

    public void PlayAttackSound()
    {
        if (attackSoundClip != null && SoundFXManager.instance != null)
        {
            SoundFXManager.instance.PlaySoundFXClip(attackSoundClip, transform, volume);
        }
        else
        {
            Debug.LogWarning("Attack sound clip or SoundFXManager instance is missing!", this);
        }
    }
}

