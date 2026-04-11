using UnityEngine;

public class SelectedImage : MonoBehaviour
{
    [SerializeField]
    private string _eventName;

    [SerializeField]
    private Sprite[] _sprites;

    [SerializeField]
    private ImageWrapper _imageUI;

    void Start()
    {
        _imageUI.Initialize();
        EventDispatcher.Instance.Subscribe(_eventName, OnSelected);
    }

    public void OnSelected(object data)
    {
        if (data is not int number)
            return;

        _imageUI.SetSprite(_sprites[number]);
    }
}
