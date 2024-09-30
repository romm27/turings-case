using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AudioSource source;

    [Header("Settings")]
    [SerializeField] AudioClip[] clicks;


    //Methods
    public void PlayClickSound() {
        int r = Random.Range(0, clicks.Length);
        source.PlayOneShot(clicks[r]);
    }
}
