using UnityEngine;

public class String : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public Transform topPoint;
    public Transform middlePoint;
    public Transform bottomPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3;
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, topPoint.position);
        lineRenderer.SetPosition(1, middlePoint.position);
        lineRenderer.SetPosition(2, bottomPoint.position);
    }
}
