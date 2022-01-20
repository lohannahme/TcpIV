using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public enum ETutorialState { none, rede, drone, tiro, coletavel }

    [SerializeField] GameObject rede, drone, tiro, coletavel;
    ETutorialState state = ETutorialState.none;

    bool pause = false;

    // Start is called before the first frame update
    void Start()
    {
        rede.SetActive(false);
        drone.SetActive(false);
        tiro.SetActive(false);
        coletavel.SetActive(false);

        PlayerController.get.jumpEvent.AddListener(OnRedeHandler);
        PlayerController.get.jumpEvent.AddListener(OnTiroHandler);
        PlayerController.get.jumpEvent.AddListener(OnColetavelHandler);

        PlayerController.get.crouchEvent.AddListener(OnDroneHandler);
        PlayerController.get.crouchEvent.AddListener(OnTiroHandler);
        PlayerController.get.crouchEvent.AddListener(OnColetavelHandler);

        PlayerController.get.diveEvent.AddListener(OnTiroHandler);
        PlayerController.get.diveEvent.AddListener(OnColetavelHandler);
    }

    private void TutorialOn(GameObject panel, ETutorialState _state)
    {
        rede.SetActive(false);
        drone.SetActive(false);
        tiro.SetActive(false);
        coletavel.SetActive(false);

        pause = true;
        Time.timeScale = 0.5f;

        panel.SetActive(true);
        state = _state;
    }
    
    public void OnRedeTutorial()
    {
        TutorialOn(rede, ETutorialState.rede);
    }

    public void OnDroneTutorial()
    {
        TutorialOn(drone, ETutorialState.drone);
    }

    public void OnTiroTutorial()
    {
        TutorialOn(tiro, ETutorialState.tiro);
    }

    public void OnColetavelTutorial()
    {
        TutorialOn(coletavel, ETutorialState.coletavel);
    }

    private void TutorialOff(GameObject panel, ETutorialState _state)
    {
        pause = false;
        Time.timeScale = 1f;
        panel.SetActive(false);
        state = ETutorialState.none;
    }

    private void OnRedeHandler()
    {
        TutorialOff(rede, ETutorialState.rede);
    }
    private void OnDroneHandler()
    {
        TutorialOff(drone, ETutorialState.drone);
    }
    private void OnTiroHandler()
    {
        TutorialOff(tiro, ETutorialState.tiro);
    }
    private void OnColetavelHandler()
    {
        TutorialOff(coletavel, ETutorialState.coletavel);
    }

    public void EndTutorial()
    {
        state = ETutorialState.none;
        coletavel.SetActive(false);
    }
}
