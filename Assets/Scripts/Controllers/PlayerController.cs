using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float fireCd = 0.1f;
    public int maxHealth = 9999999;
    public int health;
    private float timeAfterPreFire = 1f;
    private Rigidbody2D rigidbody2d;
    private float horizontal; 
    private float vertical;
    private UIManager ui_manager;

    private int using_tool_index;
    private List<GameObject> tools;
    
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        ui_manager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        ChangeHealth(maxHealth);
        InitTools();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        timeAfterPreFire += Time.deltaTime;
        if(Input.GetAxis("Fire") == 1 && timeAfterPreFire > fireCd){
            GameObject.Instantiate(tools[using_tool_index], transform.position, transform.rotation);
            timeAfterPreFire = 0;
        }

        if(Input.GetButtonDown("Switch Tool")){
            float switch_tool = Input.GetAxis("Switch Tool");
            using_tool_index += (int)switch_tool;

            if(using_tool_index < 0) using_tool_index += tools.Count;
            else if(using_tool_index >= tools.Count) using_tool_index = 0;

            ui_manager.change_tool(using_tool_index);
        }
    }
    void FixedUpdate(){
        Vector3 direction = new Vector3(horizontal, vertical, 0);
        if(direction.magnitude > 0){
            Vector3 position = transform.position + direction * speed * Time.deltaTime;
            rigidbody2d.MovePosition(position);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    private void ChangeHealth(int value)
    {
        if(value < 0) health = 0;
        else health = value;
        ui_manager.change_health(health, maxHealth);
    }
    private void InitTools()
    {
        tools = new List<GameObject>();
        tools.Add((GameObject)Resources.Load("Prefabs/Bullet"));
        tools.Add((GameObject)Resources.Load("Prefabs/Wall"));
        using_tool_index = 0;
    }
    public void Attack(int ATK)
    {
        ChangeHealth(health - ATK);
        if(health <= 0)Death();
    }
    public void Death()
    {
        GameObject.Instantiate(Resources.Load("Prefabs/Effects/Explode"), transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
