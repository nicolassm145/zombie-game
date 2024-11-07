using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject player;
    public GameObject particle;
    private int life;
    void Start()
    {
        life = 30;
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        transform.position=Vector2.MoveTowards(transform.position,player.transform.position,Time.deltaTime*2f);
        if (life <= 0)
        {
            Instantiate(particle, transform.position, Quaternion.identity );
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet"))
        {
            life = life - 10;
            Destroy(collision.gameObject);
        }
    }
}
