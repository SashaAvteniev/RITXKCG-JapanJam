using UnityEngine;

public class StageBlock : MonoBehaviour
{
    private Durator _durator;

    [SerializeField]
    private MeshRendererWrapper _holeSprite;

    [SerializeField]
    private float _holeTime;

    private void Start()
    {
        _durator = new();
        _holeSprite.Initialize();
        _holeSprite.SetSpriteAlpha(0.0f);
    }

    private void FixedUpdate()
    {
        _durator.Update();
    }

    public void OnPunched()
    {
        PutSprite();
        Debug.Log("Punched");
        // TODO:Write the code here to make this a block that the player can fall through
    }

    // Process for restoring the floor to its original condition
    private void Restor()
    {
        _holeSprite.SetSpriteAlpha(0.0f);
        // TODO:Write the code here to allow the player to pass through this area again
    }

    private void PutSprite()
    {
        _holeSprite.SetSpriteAlpha(1.0f);

        _durator.CreateTask(SpriteTransparent, Restor, _holeTime);
    }

    private void SpriteTransparent(float _elapsedTime, float _endTime)
    {
        _holeSprite.SetSpriteAlpha(1 - (_elapsedTime / _endTime));
    }
}
