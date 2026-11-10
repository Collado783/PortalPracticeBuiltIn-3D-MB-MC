
using UnityEngine;

public class BouncingPhysics : MonoBehaviour
{
    private BoxCollider boxCollider;

    public PhysicsMaterial bounceMaterial;
    public PhysicsMaterial originalMaterial;
    public PhysicsMaterial iceMaterial;

    private int bounceContacts = 0;
    private int iceContacts = 0;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();

        if (!CompareTag("Cube") && !CompareTag("Turret"))
        {
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DestroyMaterial"))
        {
            Destroy(gameObject);
            return;
        }
        if (other.CompareTag("BounceMaterial"))
        {
            bounceContacts++;
            UpdatePhysicsMaterial();
        }
        if (other.CompareTag("IceMaterial"))
        {
            iceContacts++;
            UpdatePhysicsMaterial();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BounceMaterial"))
        {
            bounceContacts--;
        }
        if (other.CompareTag("IceMaterial"))
        {
            iceContacts--;
        }

        UpdatePhysicsMaterial();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("DestroyMaterial"))
        {
            Destroy(gameObject);
            return;
        }
        if (collision.collider.CompareTag("BounceMaterial"))
        {
            bounceContacts++;
            UpdatePhysicsMaterial();
        }
        if (collision.collider.CompareTag("IceMaterial"))
        {
            iceContacts++;
            UpdatePhysicsMaterial();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("BounceMaterial"))
        {
            bounceContacts--;
        }
        if (collision.collider.CompareTag("IceMaterial"))
        {
            iceContacts--;
        }

        UpdatePhysicsMaterial();
    }

    private void UpdatePhysicsMaterial()
    {
        bounceContacts = Mathf.Max(0, bounceContacts);
        iceContacts = Mathf.Max(0, iceContacts);

        if (bounceContacts > 0)
        {
            boxCollider.material = bounceMaterial;
        }
        else if (iceContacts > 0)
        {
            boxCollider.material = iceMaterial;
        }
        else
        {
            boxCollider.material = originalMaterial;
        }
    }
}
