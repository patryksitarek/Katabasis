using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettingHandler : MonoBehaviour
{
    public Transform center;
    public float aggroRange;
    public GameObject target = null;

    public GameObject overrideTarget;
    public float? overrideDistance;

    [SerializeField]
    private CombatController combatController;
    private CombatController targetCC;
    private CombatController.Faction faction;

    void Start()
    {
        if (overrideDistance == null) overrideDistance = aggroRange;
        faction = combatController.faction;
    }

    private void Update()
    {
        if (combatController.isDead) return;

        if (!target || targetCC.isDead)
        {
            GetTarget();
        }
    }

    private GameObject FindTarget()
    {
        if (!overrideTarget)
        {
            // Find all objects in range
            Collider[] potentialTargets = Physics.OverlapSphere(center.position, aggroRange);

            // Return any of the attackable, non-friendly targets
            foreach (Collider target in potentialTargets)
            {
                if (target.TryGetComponent<CombatController>(out CombatController cc)
                    && cc.faction != faction
                    && !cc.isDead)
                {
                    return target.gameObject;
                }
            }
        // Only return the overrideTarget if it's in range
        } else if (Vector3.Distance(transform.position, overrideTarget.transform.position) <= overrideDistance)
        {
            if (!overrideTarget.GetComponent<CombatController>().isDead)
            {
                return overrideTarget;
            }
        }

        return null;
    }

    public void GetTarget()
    {
        target = FindTarget();

        if (target) targetCC = target.GetComponent<CombatController>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(center.position, aggroRange);
    }
}
