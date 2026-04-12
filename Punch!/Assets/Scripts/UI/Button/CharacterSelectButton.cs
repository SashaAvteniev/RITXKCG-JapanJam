using System;
using UnityEngine;

public class CharacterSelectButton : ButtonBase
{
    [SerializeField]
    private ImageWrapper _backColorImage;

    public override bool IsMoving => false;

    public override void Initialize(Action func = null)
    {
        _onPressedFunc = func;

        _backColorImage.Initialize();
        _backColorImage.SetImageAlpha(0.0f);
    }

    public override bool EnActive()
    {
        _backColorImage.SetImageAlpha(1.0f);

        return true;
    }

    public override bool DisActive()
    {
        _backColorImage.SetImageAlpha(0.0f);

        return true;
    }
}
