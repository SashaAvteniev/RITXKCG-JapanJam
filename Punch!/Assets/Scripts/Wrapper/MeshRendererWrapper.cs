using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MeshRendererWrapper : MonoBehaviour
{
    /// <summary>
    /// ラップ対象
    /// </summary>
    private MeshRenderer _renderer;

    /// <summary>
    /// カメラに映っているか
    /// </summary>
    public bool IsInCamera
        => _renderer.isVisible;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        if (_renderer != null)
            return;

        _renderer = this.GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// 現在の色を取得する関数
    /// </summary>
    public Color GetCurrentColor()
        => _renderer.material.color;

    /// <summary>
    /// 現在の色を更新する関数
    /// </summary>
    /// <param name="color"></param>
    public void SetSpriteColor(Color color)
        => _renderer.material.color = color;

    /// <summary>
    /// 文字UIの透明度を取得する関数
    /// </summary>
    public float GetSpriteAlpha()
        => _renderer.material.color.a;

    /// <summary>
    /// 文字UIの透明度を取得する関数
    /// </summary>
    public void SetSpriteAlpha(float value)
        => _renderer.material.color = new(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, value);
}
