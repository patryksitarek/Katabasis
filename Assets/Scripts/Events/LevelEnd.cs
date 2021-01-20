using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private float throttle = 1f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(CheckIfEnemiesRemaining());
        }
    }

    public IEnumerator CheckIfEnemiesRemaining()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length > 0)
        {
            // Debug.Log("[test] Enemies remaining");
            yield return new WaitForSeconds(throttle);
            StartCoroutine(CheckIfEnemiesRemaining());
        }
        else
        {
            // Debug.Log("[test] No enemies remaining");
            StartCoroutine(EndLevel());
        }
    }

    private IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(1);

        GameFlowController gfc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameFlowController>();

        gfc.OnLevelComplete();
    }
}
