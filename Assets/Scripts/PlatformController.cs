using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float disappearInterval = 10f;
    public float reappearInterval = 3f;

    private bool isOnCooldown = false;
    private bool isPlayerOnPlatform = false;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOnCooldown && !isPlayerOnPlatform)
        {
            isPlayerOnPlatform = true;
            StartCoroutine(DisappearAndReappear());
        }
    }

    public IEnumerator DisappearAndReappear()
    {
        if (!isOnCooldown)
        {
            isOnCooldown = true;

            gameObject.SetActive(false);
            yield return new WaitForSeconds(disappearInterval);
            gameObject.SetActive(true);

            yield return new WaitForSeconds(reappearInterval);
            isOnCooldown = false;
        }
    }
}
