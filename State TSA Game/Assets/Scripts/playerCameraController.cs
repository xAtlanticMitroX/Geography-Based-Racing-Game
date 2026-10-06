using UnityEngine;

public class player1Camera : MonoBehaviour
{

    public GameObject follow;
    public GameObject car;
    [Range(0,20)]public float smoothTime = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, follow.transform.position, ref velocity, smoothTime * Time.deltaTime);
        transform.LookAt(car.transform);        
    }
}
