using UnityEngine;

public class Door : MonoBehaviour
{
    Vector3 StartPoint;
    Vector3 EndPoint;

    public Transform endPoint;
    public float Speed = 2f;

    bool open = false;

    void Start()
    {
        StartPoint = transform.position;
        EndPoint = endPoint.position;
    }
    void Update()
    {
        if (open)
        {
            transform.position = Vector3.MoveTowards(transform.position, EndPoint, Speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, StartPoint, Speed * Time.deltaTime);
        }
    }

    public void OpenDoor()
    {
        open = true;
    }

    public void CloseDoor()
    {
        open = false;
    }
}