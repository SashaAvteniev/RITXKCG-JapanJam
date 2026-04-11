using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSelectData", menuName = "Scriptable Objects/CharacterSelectData")]
public class CharacterSelectData : ScriptableObject
{
    public int Player1Character;
    public int Player2Character;
    //public int Player3Character;
    //public int Player4Character;

    public void Initialize()
    {
        Player1Character = 0;
        Player2Character = 0;
    }
}
