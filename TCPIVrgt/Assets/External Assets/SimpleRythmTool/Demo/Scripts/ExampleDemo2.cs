using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleDemo2 : MonoBehaviour
{
    [SerializeField] AudioClip tum;
    [SerializeField] AudioClip tah;
    [SerializeField] SimpleRythmTool rythmTool;
    [SerializeField] Animator move, blink;

    AudioSource source;

    [SerializeField] float tempoDeInput = 0.3f;
    [SerializeField, Range(-1f, 1f)] float offset = 0.5f;
    float tempoAteInput
    {
        get { return rythmTool.timePerBeat() - (tempoDeInput - tempoDeInput * offset) * 0.5f; }
    }

    float avisoCountdown;
    bool aviso = false;
    bool hit = false;
    KeyCode key;
    
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        avisoCountdown = tempoAteInput;

        Debug.Log("tempoDeImput: " + tempoDeInput + "; offset: " + ((tempoDeInput - tempoDeInput * offset) * 0.5f));
    }
    
    // Update is called once per frame
    void Update()
    {
        if (aviso)
        {
            avisoCountdown -= Time.deltaTime;
            if (avisoCountdown <= 0 && Input.GetKeyDown(key) && !hit)
            {
                hit = true;
                move.SetTrigger((key == KeyCode.Space) ? "jump" : "crouch");
            }
            if (avisoCountdown <= -tempoDeInput)
            {
                aviso = false;
            }
        }
    }

    public void Tum()
    {
        source.clip = tum;
        source.Play();
    }
    
    public void Aviso_pula()
    {
        aviso = true;
        hit = false;
        avisoCountdown = tempoAteInput;
        key = KeyCode.Space;

        Tum();
    }

    public void Aviso_agacha()
    {
        aviso = true;
        hit = false;
        avisoCountdown = tempoAteInput;
        key = KeyCode.LeftControl;

        Tah();
    }

    public void Resultado()
    {
        aviso = false;
        if (hit)
        {
            Debug.Log("Acertô miseravi!");
        }
        else
        {
            Debug.Log("Erouuuu!");
        }
        if (key == KeyCode.Space)
            Tah();
        else
            Tum();
    }

    public void Tah()
    {
        source.clip = tah;
        source.Play();
    }

    public void Blink()
    {
        blink.SetTrigger("blink");
    }
}
