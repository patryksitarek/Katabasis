using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LootDropper : MonoBehaviour
{
    public GameObject loot;
    private Random rand = new Random();

    [SerializeField][Range(0f, 1f)]
    private float dropChance = 0.3f;

    public void DropLoot()
    {
        if (Random.value <= dropChance)
        {
            loot = GameObject.Instantiate<GameObject>(loot, transform.position, transform.rotation);
            loot.transform.parent = null;
        }
    }

}
