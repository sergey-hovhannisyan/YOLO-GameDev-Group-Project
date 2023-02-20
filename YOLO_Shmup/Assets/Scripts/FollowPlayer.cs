using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerPosition;
    public Vector3 offset;
    void Update()
    {
        transform.position = playerPosition.position + offset;
    }
}
