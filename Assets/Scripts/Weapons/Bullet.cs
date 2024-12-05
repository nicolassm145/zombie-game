using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage { get; set; }

    private Enemy _enemy;
    
    void Update()
    {
        transform.Translate(Vector2.up * (10 * Time.deltaTime));
        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Enemy")) return;

        _enemy = other.gameObject.GetComponent<Enemy>();
        
        _enemy.life -= Damage;
        Damage /= 2;
        
        if (_enemy.life <= 0) return;
        _enemy.StartDamageZombie();
    }
}
