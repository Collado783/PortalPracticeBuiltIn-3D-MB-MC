using UnityEngine;

public class BouncingPhysics : MonoBehaviour
{
    private BoxCollider boxCollider;

    public PhysicsMaterial bounceMaterial;
    public PhysicsMaterial originalMaterial;

    private int bounceContacts = 0;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
         if (other.tag == "BounceMaterial")
        {
            bounceContacts++;
            boxCollider.material = bounceMaterial;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "BounceMaterial")
        {
            bounceContacts--;

            if (bounceContacts <= 0)
            {
                bounceContacts = 0;
                boxCollider.material = originalMaterial;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "BounceMaterial")
        {
            bounceContacts--;

            if (bounceContacts <= 0)
            {
                bounceContacts = 0;
                boxCollider.material = originalMaterial;
            }
        }
    }
}