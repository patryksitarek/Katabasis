using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCollision : MonoBehaviour
{

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("[test] Deleting object");
            StartCoroutine(DeleteObject(0.1f));
        }
    }

    private IEnumerator DeleteObject(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("[test] " + transform.parent.gameObject.name);
        GameObject.Destroy(transform.parent.gameObject);
    }
}
