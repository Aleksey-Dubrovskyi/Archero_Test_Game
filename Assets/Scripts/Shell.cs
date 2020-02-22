using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Shell : MonoBehaviour
{
    private Transform playerPos;
    private float speedValue = 5f;
    private GameObject[] enemysInRange;
    private Animator shellAnim;
    public string casterType;

    private void Start()
    {
        shellAnim = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        if (casterType.Equals("Player"))
        {
            enemysInRange = new GameObject[GameObject.FindGameObjectsWithTag("Enemy").Length];
            enemysInRange = GameObject.FindGameObjectsWithTag("Enemy");
            ChoseEnemyPos();
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            transform.right = playerPos.transform.position - transform.position;
            transform.DOMove(playerPos.transform.position, speedValue).SetSpeedBased(true).SetEase(Ease.Linear).OnComplete(() => StartCoroutine(EnemyHit()));
        }

    }

    private void ChoseEnemyPos()
    {
        float distanceToClosesEnemy = Mathf.Infinity;
        GameObject closestEnemy = null;
        foreach (var enemy in enemysInRange)
        {
            float distanceToEnemy = (enemy.transform.position - playerPos.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosesEnemy)
            {
                distanceToClosesEnemy = distanceToEnemy;
                closestEnemy = enemy;
            }
        }
        transform.right = closestEnemy.transform.position - transform.position;
        transform.DOMove(closestEnemy.transform.position, speedValue).SetSpeedBased(true).SetEase(Ease.Linear).OnComplete(() => StartCoroutine(EnemyHit()));
    }

    private IEnumerator EnemyHit()
    {
        shellAnim.SetTrigger("isHited");
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}
