using System.Net;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    Door door;
    Vector3 StartPoint;
    Vector3 EndPoint;
    public Transform endPoint;
    public float Speed = 2f;
    public bool Pushed = false; 
    void Start()
    {
        door = FindAnyObjectByType<Door>();
        StartPoint = transform.position;
        EndPoint = endPoint.position;
    }

    void Update()
    {
        if (!Pushed)
        {
            Debug.Log("nO");
            transform.position = Vector3.MoveTowards(transform.position, StartPoint, Speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) 
        Debug.Log("Si");
        Pushed = true;
        transform.position = Vector3.MoveTowards(transform.position, EndPoint, Speed * Time.deltaTime);
        door.OpenDoor();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            Debug.Log("nO");
        transform.position = Vector3.MoveTowards(transform.position, StartPoint, Speed * Time.deltaTime);
        Pushed = false;
        door.CloseDoor();
    }
    void OnCollisionStay(Collision other)
    {
        

    }
    private void OnCollisionExit(Collision collision)
    {
        

    }
}
