using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public int ATK = 10;

    void Update()
    {
        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0), Space.Self);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.isTrigger) return;
        
        switch(collision.tag)
        {
            case "Wall":
                Destroy(gameObject);
                break;
            case "Enemy":
                collision.gameObject.GetComponent<EnemyController>().Attack(ATK);
                Destroy(gameObject);
                break;
        }
    }
}
