using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmEvents : MonoBehaviour
{
    public static RythmEvents get;

    [SerializeField] AudioClip tum;
    [SerializeField] AudioClip tah;
    [SerializeField] AudioClip vraw;
    [SerializeField] AudioClip swish;
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

    int[] originalBPM;

    // Start is called before the first frame update
    void Start()
    {
        get = this;

        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        avisoCountdown = tempoAteInput;

        playerController.jumpEvent.AddListener(JumpHandler);
        playerController.diveEvent.AddListener(DiveHandler);
        playerController.crouchEvent.AddListener(CrouchHandler);

        originalBPM = new int[rythmTool.sheets.Length];
        for (int i = 0; i < rythmTool.sheets.Length; i++)
            originalBPM[i] = rythmTool.sheets[i].BPM;
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

    public void Vraw()
    {
        source.clip = vraw;
        source.Play();
    }

    public void Swish()
    {
        source.clip = swish;
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

    public void Aviso_tiro()
    {
        aviso = true;
        hit = false;
        avisoCountdown = tempoAteInput;
        needCrouch = needJump = true;

        Vraw();
    }

    public void Drone()
    {
        ObstacleManager.get.SpawnDrone();
    }

    public void Tronco()
    {
        ObstacleManager.get.SpawnTronco();
    }

    public void Tiro()
    {
        ObstacleManager.get.SpawnTiro();
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
        if (needJump && needCrouch)
            Tah();//outro som?
        else if (needJump)
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

    public void SetSpeed(float mult)
    {
        for (int i = 0; i < rythmTool.sheets.Length; i++)
            rythmTool.sheets[i].BPM = (int)(originalBPM[i] * mult);
    }
}
