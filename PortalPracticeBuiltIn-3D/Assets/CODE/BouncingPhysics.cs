using UnityEngine;

public class BouncingPhysics : MonoBehaviour
{
    private BoxCollider boxCollider; 
    private void Awake()
    {
       boxCollider = this.GetComponent<BoxCollider>();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (boxCollider != null)
        //{
        //    boxCollider.material = new PhysicMaterial()
        //    {
        //        bounciness = 1,
        //    };
        //}
    }
}
