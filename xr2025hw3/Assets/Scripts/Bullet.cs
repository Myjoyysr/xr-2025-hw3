using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 4f;

    // Update is called once per frame
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
