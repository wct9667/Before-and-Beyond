using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGrenadeLogic : MonoBehaviour
{
    [SerializeField] private float grenadeRadius = 12f;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private GameObject stunShock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("-----------------------Stun Grenade Hit Ground");
        ExplosionDetection();
    }

    private void ExplosionDetection()
    {
        Collider[] enemiesDetected = Physics.OverlapSphere(transform.position, grenadeRadius, enemyLayerMask);
        foreach (var enemy in enemiesDetected)
        {
            Destroy(enemy.transform.gameObject, .1f);
        }
        StartCoroutine(DrawEffect());
        this.GetComponent<SphereCollider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    private IEnumerator DrawEffect()
    {
        Debug.Log("Drawing Stun");

        GameObject obj = Instantiate(stunShock, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));

        for(float i = 0f; i < grenadeRadius * 2; i = i + 0.5f)
        {
            yield return new WaitForSeconds(.005f);
            obj.transform.localScale = new Vector3(i, i, i);
        }
        yield return new WaitForSeconds(.1f);
        Destroy(obj);
        Destroy(gameObject);
        yield return new WaitForSeconds(.1f);

    }

}
