using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RythmEvents : MonoBehaviour
{
    public static RythmEvents get;
    [SerializeField] Text resposta;
    [SerializeField] Text hitText;

    [SerializeField] AudioClip tum;
    [SerializeField] AudioClip tah;
    [SerializeField] AudioClip vraw;
    [SerializeField] AudioClip swish;
    [SerializeField] AudioClip drone;
    [SerializeField] AudioClip rede;
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
        playerController.hitEvent.AddListener(HitHandler);

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

        /*Tum();*/
    }

    public void Aviso_agacha()
    {
        aviso = true;
        hit = false;
        avisoCountdown = tempoAteInput;
        needCrouch = true;

        //Tah();
    }

    public void Aviso_tiro()
    {
        aviso = true;
        hit = false;
        avisoCountdown = tempoAteInput;
        needCrouch = needJump = true;

        //Vraw();
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
            resposta.text = "Acertou!";
            Debug.Log("Acertô miseravi!");
        }
        else
        {
            resposta.text = "Errou!";
            Debug.Log("Erouuuu!");
        }
        /*if (needJump && needCrouch)
            Tah();//outro som?
        else if (needJump)
            Tah();
        else
            Tum();*/

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
        playerController.jumpEvent.RemoveListener(JumpHandler);
        playerController.diveEvent.RemoveListener(DiveHandler);
        playerController.crouchEvent.RemoveListener(CrouchHandler);
        playerController.hitEvent.RemoveListener(HitHandler);
    }

    public void SetSpeed(float mult)
    {
        for (int i = 0; i < rythmTool.sheets.Length; i++)
            rythmTool.sheets[i].BPM = (int)(originalBPM[i] * mult);
    }

    private void PlayClip(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    private void HitHandler()
    {
        hitText.text = "Atingido!";
    }

    public void ResetHit()
    {
        hitText.text = "-";
    }
}
