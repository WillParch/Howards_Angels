using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float interval = 5f;
    private bool isVisible = true;

    private void Start()
    {
        StartCoroutine(PlatformCycle());
    }

    IEnumerator PlatformCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            ToggleVisibility();
        }
    }

    void ToggleVisibility()
    {
        isVisible = !isVisible;
        GetComponent<Renderer>().enabled = isVisible;
        GetComponent<Collider>().enabled = isVisible;
    }
}
