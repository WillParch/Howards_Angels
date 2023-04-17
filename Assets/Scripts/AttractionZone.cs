using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionZone : MonoBehaviour
{
   public float attractionForce = 100.0f;
    public float attractionRadius = 5.0f;

    private void Update() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attractionRadius);
        foreach (Collider collider in colliders) {
            if (collider.CompareTag("Player")) {
                CharacterController characterController = collider.GetComponent<CharacterController>();
                if (characterController != null) {
                    Vector3 direction = transform.position - collider.transform.position;
                    float distance = direction.magnitude;
                    if (distance <= attractionRadius) {
                        float strength = (1.0f - distance / attractionRadius) * attractionForce;
                        characterController.Move(direction.normalized * strength * Time.deltaTime);
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
}
