using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] float range = 15f;
    [SerializeField] ParticleSystem projectile;
    Transform target;


    // Update is called once per frame
    void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }
    void AimWeapon()
    {   
        if(target == null)
        {
            Attack(false);
            return;
        }
        
        float targetDistance = Vector3.Distance(transform.position, target.position);

        weapon.LookAt(target);
        if (targetDistance > range)
        {
            Attack(false);
        }
        else
        {
            Attack(true);
        }
      


    }
    void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach(Enemy enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if(targetDistance < maxDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }
        
        target = closestTarget;

    }
    void Attack(bool isActive)
    {
        var emissionsModule = projectile.emission;
        emissionsModule.enabled = isActive;
    }
}
