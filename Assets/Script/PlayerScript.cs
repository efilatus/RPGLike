using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    public int _maxHealth = 100;
    int _currentHealth;
    public float _attackRate;
    public float _attackDistance;
    public int _rotationSpeed;
    private float _nextAttack;
    public float _rangePoint = 0.5f;
    public int _attackDamage;

    private Animator _pAnimator;
    private NavMeshAgent _pNavMeshAgent;
    public LayerMask _enemyLayers;
    private Transform _targetEnemy;
    public Transform _attackPoint;
    
    private bool _enemyClicked;
    private bool _walking;

    public GameObject _Mark;

    void Awake()
    {
        _currentHealth = _maxHealth;
        _pAnimator = GetComponent<Animator>();
        _pNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    void LateUpdate()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(camRay, out hit, 1000))
            {
                if (hit.collider.tag == "Enemy")
                {
                    _pNavMeshAgent.stoppingDistance = _attackDistance;
                    _targetEnemy = hit.transform;
                    _enemyClicked = true;
                    _pNavMeshAgent.destination = _targetEnemy.position;
                }
                else
                {
                    _pNavMeshAgent.stoppingDistance = 0.5f;
                    StartCoroutine(MarkIndicator(hit.point));
                    _enemyClicked = false;
                    _pNavMeshAgent.destination = hit.point;
                }
            }
        }

        this.Move();
    }

    private void Move()
    {
        if (_pNavMeshAgent.remainingDistance <= _pNavMeshAgent.stoppingDistance)
            _walking = false;
        else
            _walking = true;

        _pAnimator.SetBool("IsWalking", _walking);

        if (_enemyClicked)
            this.Rotate();
    }

    void Rotate()
    {
        if (_targetEnemy == null)
            return;

        if (!_pNavMeshAgent.pathPending && _pNavMeshAgent.remainingDistance <= _pNavMeshAgent.stoppingDistance)
        {
            var lookPos = _targetEnemy.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);  
            //if (transform.rotation.y < rotation.y)
            //{
            //    _pAnimator.SetTrigger("TurnRight");
            //}
            //else if (transform.rotation.y > rotation.y)
            //{
            //    _pAnimator.SetTrigger("TurnLeft");
            //}
            if (transform.rotation == rotation)
                Attack();
        }
    }

    void Attack()
    {
        if (Time.time > _nextAttack)
        {
            _nextAttack = Time.time + _attackRate;
            _pAnimator.SetTrigger("IsAttack");
            Collider[] hitEnemies = Physics.OverlapSphere(_attackPoint.position, _rangePoint, _enemyLayers);
            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyScript>().TakeDamage(_attackDamage);
            }
            _targetEnemy = null;
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    if (_attackPoint == null)
    //        return;
    //    Gizmos.DrawSphere(_attackPoint.position, _rangePoint);
    //}

    private IEnumerator MarkIndicator(Vector3 pos)
    {
        Vector3 position = new Vector3(pos.x, pos.y + 0.01f, pos.z);
        GameObject mark = Instantiate(_Mark, position, Quaternion.identity);
        yield return new WaitForSeconds(1);
        Destroy(mark);
    }
}
