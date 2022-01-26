using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPannel;
    [SerializeField] private GameObject _creditsPannel;



    void Start()
    {
        StartScaleToZero();
    }

    public void ChangeScene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenPannel(GameObject pannel)
    {
        pannel.transform.LeanScale(Vector3.one, .2f).setIgnoreTimeScale(true);
    }

    public void ClosePannel(GameObject activePannel)
    {
        activePannel.transform.LeanScale(Vector3.zero, .2f).setIgnoreTimeScale(true);
    }

    public void StartScaleToZero()
    {
        _settingsPannel.transform.localScale = Vector3.zero;
        _creditsPannel.transform.localScale = Vector3.zero;
    }
}
