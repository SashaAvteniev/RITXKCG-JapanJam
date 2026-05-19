using UnityEngine;
using UnityEngine.InputSystem;

public class TitleBacker : MonoBehaviour
{
    void OnRetuenTitle(InputValue value)
    {
        GameManager.EndGame();
    }
}
