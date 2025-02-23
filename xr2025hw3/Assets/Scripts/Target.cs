using UnityEngine;

public class Target : MonoBehaviour
{

    public float drop = 10f;
    public float dropSpeed = 3f;
    public float lifeTime = 3f;
    private bool goingDown = false;

    private Vector3 startPosition;
    private Vector3 endPosition;

    public float floatingSpeed = 1f;
    public float floatingHeight = 0.3f;

    void Start() {
        startPosition = transform.position;
        endPosition = startPosition - new Vector3(0,drop,0);
    }

    void Update(){
        if (goingDown){
            transform.position = Vector3.MoveTowards(transform.position, endPosition, dropSpeed * Time.deltaTime);
        }else{
            float offset = Mathf.Cos(Time.time * floatingSpeed) * floatingHeight;
            transform.position = startPosition + new Vector3(0, offset, 0);
        }
    }


    private void OnTriggerEnter(Collider other){
        Debug.Log("Collision");
        if (goingDown){
            return;
        }


        if (other.CompareTag("Bullet")){
            Debug.Log("Collision WITH BULLET");
            goingDown = true;
            Destroy(gameObject, lifeTime);
        }

    }
 
}
