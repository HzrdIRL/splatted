using UnityEngine;

public class Bullet : MonoBehaviour {

    private Transform target;

    public int dmg;
    public int ownedBy;
    public float explosionRadius = 0f;
    public float speed = 70f;
    public GameObject impactEffect;

	
	// Update is called once per frame
	void Update () {
	    if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
	}

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);

        if (explosionRadius > 0f)
        {
            Explode();
        }else
        {
            Damage(target);
        }

        Destroy(gameObject);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy" || (collider.tag == "Turret" && collider.GetComponent<Turret>().ownedBy != ownedBy))
            {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        Enemy e;
        Node n;
        Turret t;

        if ((n = enemy.GetComponent<Node>()) != null)
        {
            n.ChangeOwner(ownedBy);
        }

        if ((e = enemy.GetComponent<Enemy>()) != null)
        {
            e.TakeDamage(dmg);
        }

        if ((t = enemy.GetComponent<Turret>()) != null)
        {
            t.TakeDamage(dmg);
        }


    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
