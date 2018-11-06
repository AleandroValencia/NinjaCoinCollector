using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootScript : MonoBehaviour
{

    private float maxSize = 0.0f;
    public float minSize = 1.0f;
    public bool grow = true;
    private float timer = 0.0f;
    public float lifetime = 5.0f;
    private float rotation = 1.0f;

    // Use this for initialization
    void Start()
    {
        float x = Random.Range(-8.0f, 8.0f);
        float y = Random.Range(-3.5f, 3.5f);

        transform.position = new Vector3(x, y, -1.0f);
        maxSize = transform.localScale.x + 0.2f;
        minSize = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < 0.6f || timer > 4.4f)
        {
            if (GetComponent<SpriteRenderer>().enabled)
            {
                transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), rotation++ * 25);
                if (timer > 4.4f)
                {
                    grow = false;
                    transform.localScale *= 0.9f;
                }
                else if (transform.localScale.x < maxSize)
                {
                    transform.localScale *= 1.05f;
                }
            }
        }
        else
        {
            transform.forward = new Vector3(0.0f, 0.0f, 1.0f);
        }

        // Fade out
        if (tag != "Obstacle")
        {
            if (timer >= 4.0f)
            {
                Color temp = GetComponent<SpriteRenderer>().color;
                temp.a = 0.6f;
                GetComponent<SpriteRenderer>().color = temp;
            }
            else if (timer >= 3.0f)
            {
                Color temp = GetComponent<SpriteRenderer>().color;
                temp.a = 0.7f;
                GetComponent<SpriteRenderer>().color = temp;
            }
            else if (timer >= 2.0f)
            {
                Color temp = GetComponent<SpriteRenderer>().color;
                temp.a = 0.8f;
                GetComponent<SpriteRenderer>().color = temp;
            }
            else if (timer >= 1.0f)
            {
                Color temp = GetComponent<SpriteRenderer>().color;
                temp.a = 0.9f;
                GetComponent<SpriteRenderer>().color = temp;
            }
        }

        if (timer >= lifetime)
        {
            if (tag == "Boot")
            {
                GameObject.Find("GameManager").GetComponent<GameManagerScript>().bootCount--;
            }
            else if (tag == "Obstacle")
            {
                GameObject.Find("GameManager").GetComponent<GameManagerScript>().obstacleCount--;
            }
            Destroy(this.gameObject);
        }

        if (grow)
        {
            transform.localScale *= 1.005f;
            if (transform.localScale.x >= maxSize)
            {
                grow = false;
            }
        }
        else
        {
            transform.localScale *= 0.995f;
            if (transform.localScale.x < minSize)
            {
                grow = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<AudioSource>().Play();
        StartCoroutine(Death());
        if (tag == "Boot")
        {
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().bootCount--;
        }
        else if (tag == "Obstacle")
        {
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().obstacleCount--;
        }
    }

    IEnumerator Death()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
