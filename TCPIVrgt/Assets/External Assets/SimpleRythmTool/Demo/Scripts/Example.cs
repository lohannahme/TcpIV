using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    [SerializeField] AudioClip tum;
    [SerializeField] AudioClip tah;

    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
    }

    public void Tum()
    {
        Debug.Log(", Tum");
        source.clip = tum;
        source.Play();
    }

    public void Tah()
    {
        Debug.Log(", Tah!");
        Debug.Log("");
        source.clip = tah;
        source.Play();
    }
}
