using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCollision : MonoBehaviour
{

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(DeleteObject(0));
        }
    }

    private IEnumerator DeleteObject(float delay)
    {
        
        yield return new WaitForSeconds(delay);

        GameObject.Destroy(gameObject);
    }
}
