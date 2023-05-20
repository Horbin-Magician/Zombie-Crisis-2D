using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 1f;
    private int health = 100;
    public float agentRadios = 0.5f;
    public int ATK = 10;
    private float navCD = 1f;
    private float lastNav;
    private List<NavNode> path;
    private Transform playerTsf;
    private Rigidbody2D rigidbody2d;
    void Start()
    {
        lastNav = navCD;
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Vector2 startPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 endPos = new Vector2(players[0].transform.position.x, players[0].transform.position.y);
        
        lastNav -= Time.deltaTime;
        if(lastNav <= 0){
            lastNav = navCD;
            path = NavMapPool.Instance.FindPath(startPos, endPos, agentRadios);
        }
    }
    void FixedUpdate(){
        if(path != null)
        {
            Vector3 direction = new Vector3(path[1].x-path[0].x, path[1].y-path[0].y);
            direction.Normalize();
            if(direction.magnitude > 0){
                Vector3 position = transform.position + direction * speed * Time.deltaTime;
                rigidbody2d.MovePosition(position);
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
        
    }
    public void Attack(int ATK)
    {
        health = health - ATK;
        if(health <= 0) Death();
    }
    public void Death()
    {
        GameObject.Instantiate(Resources.Load("Prefabs/Effects/Explode"), transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Player":
                collision.gameObject.GetComponent<PlayerController>().Attack(ATK);
                break;
        }
    }
}