using UnityEngine;

public class PlayerStock : MonoBehaviour
{
    [SerializeField]
    private ImageWrapper _playerImage;

    [SerializeField]
    private TextWrapper _text;

    [SerializeField]
    private string _imageEventName;

    [SerializeField]
    private string _textEventName;

    [SerializeField]
    private Sprite[] _sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerImage.Initialize();
        _text.Initialize();

        EventDispatcher.Instance.Subscribe(_imageEventName, SetPlayerImage);
        EventDispatcher.Instance.Subscribe(_textEventName, OnLifeChanged);
    }

    public void SetPlayerImage(object data)
    {
        if (data is not int id)
            return;

        _playerImage.SetSprite(_sprite[id]);
    }

    public void OnLifeChanged(object data)
    {
        if (data is not int life)
            return;

        _text.SetText($"×{life}");
    }
}
