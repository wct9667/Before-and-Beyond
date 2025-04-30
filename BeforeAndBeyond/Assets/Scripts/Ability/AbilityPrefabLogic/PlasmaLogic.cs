using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

public class PlasmaLogic : MonoBehaviour
{
    [SerializeField] private float plasmaRadius = 12f;
    private LayerMask enemyMask;
    [SerializeField] private GameObject plasmaShockwave;

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("-----------------------Stun Grenade Hit Ground");
        if (col.transform.gameObject.tag != "hammer")
        {
            ExplosionDetection();
        }
    }

    private void ExplosionDetection()
    {
        enemyMask = LayerMask.GetMask("whatIsEnemy");
        Collider[] enemiesDetected = Physics.OverlapSphere(transform.position, plasmaRadius, enemyMask);
        foreach (var enemy in enemiesDetected)
        {
            Health health = enemy.transform.gameObject.GetComponent<Health>();
            health.SubtractHealth(100);
            //Destroy(enemy.transform.gameObject, .1f);
        }
        StartCoroutine(DrawEffect());
        this.GetComponent<SphereCollider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    private IEnumerator DrawEffect()
    {
        Debug.Log("Drawing Stun");

        GameObject obj = Instantiate(plasmaShockwave, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));

        for (float i = 0f; i < plasmaRadius * 2; i = i + 0.5f)
        {
            yield return new WaitForSeconds(.0025f);
            obj.transform.localScale = new Vector3(i, i, i);
        }
        yield return new WaitForSeconds(.075f);
        Destroy(obj);
        Destroy(gameObject);
        yield return new WaitForSeconds(.1f);

    }

}
