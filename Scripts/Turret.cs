using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : MonoBehaviour {

    private Transform target;
    private Enemy targetEnemy;
    private float health;
    private List<GameObject> targetNodes = new List<GameObject>();
    public int ownedBy;

    [Header("General")]
    public float range = 15f;
    public float startHealth = 100;
    public int reward = 50;
    public GameObject deathEffect;

    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public int bulletDmg;

    [Header("Use Lazer")]
    public bool useLazer = false;
    public float damageOverTime = 40;
    public float slowAmount = 0.5f;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    [Header("Use Paint")]
    public bool usePaint = false;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public string nodeTag = "Node";
    public string turretTag = "Turret";
    public Transform partToRotate;
    public float turnSpeed = 10f;

    public Transform firePoint;

	// Use this for initialization
	void Start () {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        health = startHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (target == null)
        {
            if(useLazer)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }
            else
            {
                if (fireCountdown > 0)
                    fireCountdown -= Time.deltaTime;
            }

            return;
        }

        LockOnTarget();

        if (useLazer)
        {
            Lazer();
        }
        else
        {
            if (fireCountdown <= 0)
            {
                if (!usePaint)
                {
                    Shoot();
                }
                else
                {
                    Paint();
                }
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
	}

    void LockOnTarget()
    {
        //Target Lock-on
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Lazer()//doesn't hurt turrets yet
    {
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowAmount);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }
            
        //Draw Lazer Beam
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        //Impact Effect Particles
        Vector3 dir = firePoint.position - target.position;
        impactEffect.transform.position = target.position + dir.normalized;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
        
    }

    void Paint()
    {
        foreach(GameObject node in targetNodes)
        {
            target = node.transform;
            Shoot();
        }

        target = null;
        targetNodes.Clear();
    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.dmg = bulletDmg;
        bullet.ownedBy = ownedBy;

        if (bullet != null)
            bullet.Seek(target);
    }

    //Get nearest target
    void UpdateTarget()
    {
        if (!usePaint)
        {
            /* GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
             GameObject[] turrets = GameObject.FindGameObjectsWithTag(turretTag);

             float shortestDistance = Mathf.Infinity;
             GameObject nearestEnemy = null;

             foreach (GameObject enemy in enemies)
             {
                 float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                 if (distanceToEnemy < shortestDistance)
                 {
                     shortestDistance = distanceToEnemy;
                     nearestEnemy = enemy;
                 }
             }*/

            Collider[] enemies = Physics.OverlapSphere(transform.position, range);
            GameObject[] turrets = GameObject.FindGameObjectsWithTag(turretTag);

            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (Collider enemy in enemies)
            {
                GameObject _target = enemy.transform.gameObject;
                Debug.Log(_target);
                if (string.Compare(_target.tag, turretTag) == 0 && _target.GetComponent<Turret>().ownedBy != ownedBy)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = _target;
                    }
                }

                if (string.Compare(_target.tag, enemyTag) == 0 && _target.GetComponent<Enemy>().ownedBy != ownedBy)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = _target;
                    }
                }

            }

            if (nearestEnemy != null && shortestDistance <= range)
            {
                target = nearestEnemy.transform;
                targetEnemy = target.GetComponent<Enemy>();
            }
            else 
            {
                target = null;
            }
        }
        else
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, range);

            foreach (Collider hit in hits)
            {
                GameObject _target = hit.transform.gameObject;

                if (string.Compare(_target.tag, nodeTag) != 0)
                {
                    continue;
                }

                if (_target.GetComponent<Node>().ownedBy == ownedBy)
                {
                    targetNodes.Remove(_target);
                    continue;
                }

                if (targetNodes.Find(x => x.GetInstanceID() == _target.GetInstanceID()) == null)
                {
                    targetNodes.Add(_target);
                }
            }

            if(targetNodes.Count >= 1)
            {
                target = targetNodes[0].transform;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        //healthBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PlayerStats.Money += reward;

        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        //WaveSpawner.EnemiesAlive--;

        Destroy(gameObject);
    }


    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
