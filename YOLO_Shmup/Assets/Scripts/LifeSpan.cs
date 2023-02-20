using UnityEngine;

public class LifeSpan : MonoBehaviour
{
    public float timeToDestroy = 1f;

    void Update()
    {
        Destroy(gameObject, timeToDestroy);  
    }
}
