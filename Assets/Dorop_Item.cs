using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Dorop_Item : MonoBehaviour
{
    [SerializeField]
    List<Sprite> images;

    [SerializeField]
    float[] r;//”¼Œa

    public int Priority;
    public int My_num;

    int Max_mynum;

    SpriteRenderer SR;
    public Game_Manager mane;

    [SerializeField]
    bool Auto = false;
    [SerializeField]
    int Auto_size_num;
    [SerializeField]
    float Auto_size_range,Auto_size_min;



    [SerializeField]
    float game_over_limit = 2f;
    bool game_Over = false;
    float game_Over_time = 0;

    SpriteMask mask;

    [SerializeField]
    AudioClip clip;

    AudioSource AS;



    private void Awake()
    {
        if (Auto)
        {
            r = new float[Auto_size_num];
            for(int i = 0;i < Auto_size_num; i++)
            {
                r[i] = Auto_size_min + Auto_size_range * i;
            }

            if (Auto_size_num > images.Count)
            {
                int num  = images.Count;
                for(int i = 0; i < Auto_size_num - num; i++)
                {
                    images.Add(images[i]);
                }
            }

        }   
    }
    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        AS = GetComponent<AudioSource>();
        transform.localScale = new Vector3(r[My_num], r[My_num], 1);
        SR.sprite = images[My_num];
        Max_mynum = r.Length;
        
        mask = transform.GetChild(0).GetComponent<SpriteMask>();
        mask.frontSortingOrder = Priority;
        mask.backSortingOrder = Priority-1;
        SR.sortingOrder = Priority;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(r[My_num], r[My_num],1);
        SR.sprite = images[My_num];

        if (game_Over)
        {
            game_Over_time += Time.deltaTime;
        }
        if(game_Over_time > game_over_limit)
        {
            mane.Game_Over();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Dorop_Item sc = collision.gameObject.GetComponent<Dorop_Item>();
        if (sc != null)
        {
            if(sc.Priority < Priority && sc.My_num == My_num)
            {
                Destroy(collision.gameObject);
                My_num++;
                mane.Point += My_num * My_num;
                mane.Max_size = math.max(My_num, mane.Max_size);
                AS.PlayOneShot(clip);
                if (My_num >= Max_mynum)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        game_Over = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        game_Over = false;
        game_Over_time = 0;
    }


}
