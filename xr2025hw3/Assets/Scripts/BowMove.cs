using UnityEngine;

public class BowMove : MonoBehaviour
{

    public Transform rightHand;
    public Transform leftHand;

    public float distanceToHand;
    public float rotationSpeed;
    public float floatingSpeed;
    public float floatingHeight = 0.2f;
    public float treshold = 0.5f;

    private Vector3 startPosition;

    void Start(){
        startPosition = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        float distanceToRight = Vector3.Distance(transform.position, rightHand.position);
        float distanceToLeft = Vector3.Distance(transform.position, leftHand.position);

        if (distanceToRight > treshold && distanceToLeft > treshold){
            RotateObject();
            FloatObject();
        }else{
            startPosition = transform.position;
        }

    }

    private void RotateObject(){
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void FloatObject(){
        float offset = Mathf.Cos(Time.time * floatingSpeed) * floatingHeight;
        transform.position = startPosition + new Vector3(0, offset,0);
    }
}
