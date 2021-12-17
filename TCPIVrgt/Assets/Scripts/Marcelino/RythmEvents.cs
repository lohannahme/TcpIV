using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmEvents : MonoBehaviour
{
    [SerializeField] AudioClip tum;
    [SerializeField] AudioClip tah;
    [SerializeField] SimpleRythmTool rythmTool;
    [SerializeField] PlayerController playerController;
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

    bool jump, dive, crouch;
    bool needJump, needCrouch;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        avisoCountdown = tempoAteInput;

        playerController.jumpEvent.AddListener(JumpHandler);
        playerController.diveEvent.AddListener(DiveHandler);
        playerController.crouchEvent.AddListener(CrouchHandler);
    }

    // Update is called once per frame
    void Update()
    {
        if (aviso)
        {
            avisoCountdown -= Time.deltaTime;
            if (avisoCountdown <= 0 && !hit)
            {
                if ((jump && needJump) || (crouch && needCrouch))
                {
                    hit = true;
                }
            }
            if (avisoCountdown <= -tempoDeInput)
            {
                aviso = false;
            }
        }

        jump = dive = crouch = false;
    }

    public void Tum()
    {
        source.clip = tum;
        source.Play();
    }

    public void Tah()
    {
        source.clip = tah;
        source.Play();
    }

    public void Aviso_pula()
    {
        aviso = true;
        hit = false;
        avisoCountdown = tempoAteInput;
        needJump = true;

        Tum();
    }

    public void Aviso_agacha()
    {
        aviso = true;
        hit = false;
        avisoCountdown = tempoAteInput;
        needCrouch = true;

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
        if (needJump)
            Tah();
        else
            Tum();

        needJump = needCrouch = false;
    }

    private void JumpHandler()
    {
        jump = true;
    }
    private void DiveHandler()
    {
        dive = true;
    }
    private void CrouchHandler()
    {
        crouch = true;
    }

    private void OnDisable()
    {
        playerController.jumpEvent.RemoveAllListeners();
        playerController.diveEvent.RemoveAllListeners();
        playerController.crouchEvent.RemoveAllListeners();
    }
}
