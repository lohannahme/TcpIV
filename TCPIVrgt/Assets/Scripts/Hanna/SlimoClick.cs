using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimoClick : MonoBehaviour
{
    [SerializeField] private float _move;
    [SerializeField] private GameObject _buttons;
    [SerializeField] private Animator _buttonAnimator;

    private bool _sideLeft = true;
    private bool _sideRight = false;

    public void ChangeSide()

    {
        if (_sideLeft)
        {
            transform.LeanMoveLocalX(-110, .3f).setIgnoreTimeScale(true);

            _buttons.transform.LeanMoveLocalX(58, .3f).setIgnoreTimeScale(true);
            _buttonAnimator.SetBool("Buttonfade", true);

            _sideLeft = false;
            _sideRight = true;
        }
        else if (_sideRight)
        {
            transform.LeanMoveLocalX(0, .3f).setIgnoreTimeScale(true);

            _buttons.transform.LeanMoveLocalX(0, .3f).setIgnoreTimeScale(true);
            _buttonAnimator.SetBool("Buttonfade", false);

            _sideLeft = true;
            _sideRight = false;
        }
    }


}
