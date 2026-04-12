using System;
using UnityEngine;

public class SelectedImage : MonoBehaviour
{
    [SerializeField]
    private string _eventName;

    [SerializeField]
    private Sprite[] _sprites;

    [SerializeField]
    private ImageWrapper _imageUI;

    private static int _playerOrder = 0;

    void Start()
    {
        _imageUI.Initialize();
        EventDispatcher.Instance.Subscribe(_eventName, OnSelected);
    }

    public void OnSelected(object data)
    {
        if (data is not int number)
            return;

        _playerOrder++;

        GameObject textObject = new GameObject("PlayerText");
        textObject.transform.SetParent(_imageUI.transform, false);
        TMPro.TextMeshProUGUI text = textObject.AddComponent<TMPro.TextMeshProUGUI>();
        RectTransform rect = textObject.GetComponent<RectTransform>();

        text.text = "Player " + _playerOrder.ToString();
        text.alignment = TMPro.TextAlignmentOptions.Center;
        text.color = Color.black;
        rect.localScale = new Vector3(4f, 4f, 4f);
        rect.anchoredPosition = new Vector2(0f, 350f);

        _imageUI.SetSprite(_sprites[number]);
    }
}