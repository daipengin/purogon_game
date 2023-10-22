using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_move : MonoBehaviour
{
    [SerializeField]
    float x_limit = 1.5f;

    [SerializeField]
    float speed = 1f;

    [SerializeField]
    GameObject Item;

    [SerializeField]
    int ball_count;

    [SerializeField]
    float Cool_down = 0.5f;

    [SerializeField]
    Game_Manager manager;

    [SerializeField]
    float Wide;

    float timer;

    GameObject holdingobj;

    Vector3 startpos;
    Vector3 currentpos;

    // Start is called before the first frame update
    void Start()
    {
        ball_count = 0;
        timer = 0;
        Genarate();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        Movinig(x);

        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space)&& timer*Time.timeScale > Cool_down)
        {
            Doropping();
            timer = 0;
        }
        if (Time.timeScale != 0)
        {
            TapMove();
        }
        

    }

    void TapMove()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startpos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            currentpos = Input.mousePosition;
            float diffx = (currentpos.x - startpos.x) / Screen.width*Wide*speed;
            float newX = Mathf.Clamp(transform.localPosition.x + diffx, -x_limit, x_limit);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            startpos = currentpos;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(timer * Time.timeScale > Cool_down)
            {
                Doropping();
                timer = 0;
            }
            
        }
    }



    void Movinig(float x)
    {
        Vector3 pos = transform.position;
        pos += new Vector3(x*speed*Time.deltaTime, 0, 0);
        
        pos.x = Mathf.Clamp(pos.x, -x_limit, x_limit);
        transform.position = pos;
    }

    void Genarate()
    {
        GameObject obj = Instantiate(Item, transform.position, Quaternion.identity,transform);
        Dorop_Item script = obj.GetComponent<Dorop_Item>();
        script.Priority = ball_count++;
        script.mane = manager;
        script.My_num = Random.Range(0, Mathf.Max(2, manager.Max_size - 3));
        ball_count++;

        obj.GetComponent<CircleCollider2D>().enabled = false;
        obj.GetComponent<Rigidbody2D>().gravityScale = 0;
        holdingobj = obj;
    }


    void Doropping()
    {
        holdingobj.transform.parent = null;
        holdingobj.GetComponent<CircleCollider2D>().enabled = true;
        holdingobj.GetComponent<Rigidbody2D>().gravityScale = 1 ;
        Genarate();

    }
}
