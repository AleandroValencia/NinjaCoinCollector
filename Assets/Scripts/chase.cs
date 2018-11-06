using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chase : MonoBehaviour
{
    private float timer = 0.0f;
    private PlayerScript player;
    public bool chasing = false;
    private float speed = 0.0f;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        transform.localScale = new Vector3(0.1f, 0.1f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        speed += timer / 1000.0f;
        
        transform.position += (player.GetComponent<Transform>().position - transform.position) * Time.deltaTime * speed;
        
    }
}
