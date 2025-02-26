using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;

    // Update is called once per frame
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
