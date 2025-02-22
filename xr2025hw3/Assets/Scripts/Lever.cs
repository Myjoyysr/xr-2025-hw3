using UnityEngine;

public class Lever : MonoBehaviour
{
    private HingeJoint hinge;
    public float angle = 45f;
    public bool goingDown = false;
    public bool goingUp =false;

    public float currentAngle;

    public Transform bridgeDown;
    public Transform bridgeUp;

    public Transform wallDown;
    public Transform wallUp;

    public GameObject bridge;
    public GameObject wall;

    public float bridgeSpeed = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hinge = GetComponent<HingeJoint>();

        JointLimits limits = hinge.limits;
        limits.min = angle+1;
        limits.max = -angle+1;

        hinge.limits = limits;

    }

    // Update is called once per frame
    void Update()
    {
        currentAngle = hinge.angle;

        if (currentAngle >= angle){
            goingUp = true;
        }
        else if (currentAngle <= -angle){
            goingDown = true;
        }else{
            goingUp = false;
            goingDown = false;
        }

        if (goingUp){
            bridge.transform.position = Vector3.MoveTowards(bridge.transform.position, bridgeUp.position, bridgeSpeed * Time.deltaTime);
            wall.transform.position = Vector3.MoveTowards(wall.transform.position, wallDown.position, bridgeSpeed * Time.deltaTime);
        }
        if(goingDown){
            bridge.transform.position = Vector3.MoveTowards(bridge.transform.position, bridgeDown.position, bridgeSpeed * Time.deltaTime);
            wall.transform.position = Vector3.MoveTowards(bridge.transform.position, wallUp.position, bridgeSpeed * Time.deltaTime);
        }
    }
}
