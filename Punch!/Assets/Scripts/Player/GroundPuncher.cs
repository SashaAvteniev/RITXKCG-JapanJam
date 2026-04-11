using UnityEngine;

public class GroundPuncher : MonoBehaviour
{
    [SerializeField]
    private LayerMask _groundMask;

    public void OnPunch()
    {
        Vector3 origin = this.transform.position;

        //origin += Vector2.up * _rayProvider.HorizontalRaySpacing * i;

        var hit = Physics.Raycast(
            origin,
            Vector3.down,
            out RaycastHit hitObj,
            1,
            _groundMask.value
        );

        if (hit)
        {
            if (!hitObj.collider.gameObject.TryGetComponent(out StageBlock block))
                return;

            block.OnPunched();
        }
    }
}
