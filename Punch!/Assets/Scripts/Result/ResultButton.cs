using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
