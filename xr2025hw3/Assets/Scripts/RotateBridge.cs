using UnityEngine;

public class RotateBridge : MonoBehaviour
{

    public Transform target1;
    public Transform target2;

    public float rotationSpeed = 3f;
    private bool isRotating = false;
    private bool finishedRotation = false;

    private Quaternion targetRotation;


    public GameObject wallBlock;


    // Update is called once per frame
    void Update()
    {
        if (!target1 && !target2 && !isRotating && !finishedRotation){
            isRotating = true;
            targetRotation = Quaternion.Euler(transform.eulerAngles.x -90f, transform.eulerAngles.y,transform.eulerAngles.z);
        }

        if (isRotating){
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f){
                transform.rotation = targetRotation;
                isRotating = false;
                finishedRotation = true;

                if (wallBlock != null){
                    Destroy(wallBlock);
                }
            }
        }
    }
}
