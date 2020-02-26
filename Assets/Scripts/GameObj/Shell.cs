using System.Collections;
using UnityEngine;

public class Shell : MonoBehaviour
{
    private GameObject player;
    public float speedValue = 10f;
    private GameObject[] enemysInRange;
    private Animator shellAnim;
    public string casterType;
    private float lookAngle;
    private Vector2 lookDirection;
    private Rigidbody2D shellRB;
    public float offsetRotation;
    private GameObject closestEnemy;
    public bool bossStateThree;
    Collider2D shellCollider;

    private void Start()
    {
        shellAnim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        shellRB = GetComponent<Rigidbody2D>();
        shellCollider = GetComponent<Collider2D>();
        if (casterType.Equals("Player"))
        {
            enemysInRange = new GameObject[GameObject.FindGameObjectsWithTag("Enemy").Length];
            enemysInRange = GameObject.FindGameObjectsWithTag("Enemy");
            ChoseEnemyPos();
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            ShellRotation(player);
        }
    }

    private void FixedUpdate()
    {
        if (gameObject.activeInHierarchy)
        {
            shellRB.AddForce(gameObject.transform.up * speedValue);
        }

    }

    private void ChoseEnemyPos()
    {
        float distanceToClosesEnemy = Mathf.Infinity;
        closestEnemy = null;
        foreach (var enemy in enemysInRange)
        {
            float distanceToEnemy = (enemy.transform.position - player.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosesEnemy)
            {
                distanceToClosesEnemy = distanceToEnemy;
                closestEnemy = enemy;
            }
        }
        ShellRotation(closestEnemy);
    }

    private IEnumerator Hit()
    {
        shellCollider.enabled = false;
        shellRB.velocity = Vector2.zero;
        shellRB.angularVelocity = 0f;
        shellAnim.SetTrigger("isHited");
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Hit();
    }

    private void ShellRotation(GameObject target)
    {
        if (GameManager.instance.gameState != GameState.Lose)
        {
            if (bossStateThree == true)
            {
                lookDirection = target.transform.position - gameObject.transform.position;
                lookDirection.Normalize();
                lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, (lookAngle - 90f) + offsetRotation);
            }
            else
            {
                lookDirection = target.transform.position - gameObject.transform.position;
                lookDirection.Normalize();
                lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && casterType == "Enemy")
        {
            if (collision.isTrigger)
            {
                Player player = collision.GetComponent<Player>();
                DamageManager.instance.TakeShellDamage(player.playerHP);
                player.playerHP = DamageManager.instance.UpdateHpInfo();
                player.healthBar.SetHealth(player.playerHP);
                StartCoroutine("Hit");
            }
        }
        if (collision.tag == "Enemy" && casterType == "Player")
        {
            Enemy currentEnemy = collision.GetComponent<Enemy>();
            DamageManager.instance.TakeShellDamage(currentEnemy.enemyHP);
            currentEnemy.enemyHP = DamageManager.instance.UpdateHpInfo();
            currentEnemy.healthBar.SetHealth(currentEnemy.enemyHP);
            StartCoroutine("Hit");
        }
    }
}
