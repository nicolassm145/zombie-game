using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
}
