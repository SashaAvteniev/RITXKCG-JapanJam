using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    private TextWrapper _text;
    [SerializeField]
    public ResultInfo _resultInfo;
    [SerializeField]
    public int _winnerId;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _text.Initialize();

    }
    private void Update()
    {
        SetText();
    }
    private void SetText()
    {
        switch (_winnerId)
        {
            case 0:
                _text.SetText("Player1Win!!");
                break;
            case 1:
                _text.SetText("Player2Win!!");
                break;
            case 2:
                _text.SetText("Player3Win!!");
                break;
            case 3:
                _text.SetText("Player4Win!!");
                break;
        }
    }
}
