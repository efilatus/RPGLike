using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int _maxHealth = 100;
    int _currentHealth;

    public GameObject _deadBody;
    private Animator _eAnimator;

    void Awake()
    {
        _currentHealth = _maxHealth;
        _eAnimator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        StartCoroutine(HitAnimation());
        if (_currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Instantiate(_deadBody, transform.position, Quaternion.Euler(transform.rotation.eulerAngles));
        Destroy(this.gameObject);
    }

    //void Update()
    //{

    //}

    IEnumerator HitAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        _eAnimator.SetTrigger("IsHit");
    }
}
