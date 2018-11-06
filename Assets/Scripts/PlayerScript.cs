using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    enum EWHICHWALL
    {
        TOP_WALL = 0,
        RIGHT_WALL = 1,
        BOTTOM_WALL = 2,
        LEFT_WALL = 3
    };

    public float speed = 6.0f;
    public float slowSpeed = 6.0f;

    private bool moving = false;
    private int clickCount = 0;
    private Vector2 mousePos;
    private Vector2 direction;

    // Coin count
    public int tenCents = 0;
    public int twentyCents = 0;
    public int fiftyCents = 0;
    public int oneDollars = 0;
    public int twoDollars = 0;

    private int coinCount = 0;
    private bool grow = true;
    private float totalCoins = 0;
    private bool dash = true;
    private bool stunned = false;

    private float timer = 0.0f;
    public bool mainMenu = false;

    private EWHICHWALL wall = EWHICHWALL.TOP_WALL;

    // Use this for initialization
    void Start()
    {
        if (!mainMenu)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            moving = true;
            dash = false;
            speed = 1.0f;
            direction = new Vector2(0.0f, -5.0f);
            clickCount = 1;
        }
        else
        {
            moving = false;
            dash = true;
            speed = 6.0f;
            clickCount = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 1.75f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (GetComponent<CircleCollider2D>().enabled == false)
            {
                GetComponent<CircleCollider2D>().enabled = true;
                speed = 6.0f;
            }
        }

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);

        // grow coin sack
        totalCoins = tenCents + twentyCents + fiftyCents + oneDollars + twoDollars;
        transform.GetChild(0).GetComponent<Transform>().localScale = new Vector3(Map(0, 800, 1, 3, totalCoins), Map(0, 800, 1, 3, totalCoins));

        //if (!moving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (clickCount < 1)
                {
                    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    bool outOfBounds = false;
                    if (!moving)
                    {
                        switch (wall)
                        {
                            case EWHICHWALL.TOP_WALL:
                                if (mousePos.y > pos.y)
                                {
                                    outOfBounds = true;
                                }
                                break;
                            case EWHICHWALL.RIGHT_WALL:
                                if (mousePos.x > pos.x)
                                {
                                    outOfBounds = true;
                                }
                                break;
                            case EWHICHWALL.BOTTOM_WALL:
                                if (mousePos.y < pos.y)
                                {
                                    outOfBounds = true;
                                }
                                break;
                            case EWHICHWALL.LEFT_WALL:
                                if (mousePos.x < pos.x)
                                {
                                    outOfBounds = true;
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    if (!outOfBounds)
                    {
                        moving = true;
                        clickCount++;
                        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        direction = (mousePos - pos).normalized;

                        if (mousePos.x > pos.x)
                        {
                            GetComponent<SpriteRenderer>().flipX = true;
                        }
                        else
                        {
                            GetComponent<SpriteRenderer>().flipX = false;
                        }

                        // dash if moving horizontally
                        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                        {
                            dash = true;
                        }
                        else
                        {
                            // roll if moving vertically
                            dash = false;
                        }
                    }
                    GetComponent<TrailRenderer>().startColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
            }
        }

        if (moving)
        {
            GetComponent<Animator>().SetBool("top", false);
            GetComponent<Animator>().SetBool("side", false);
            GetComponent<Animator>().SetBool("bottom", false);

            if (!stunned)
            {
                if (dash == true)
                {
                    GetComponent<Animator>().SetBool("dash", true);
                    GetComponent<Animator>().SetBool("roll", false);
                    GetComponent<Transform>().forward = new Vector3(0.0f, 0.0f, 1.0f);
                }
                else
                {
                    if (!GetComponent<SpriteRenderer>().flipX)
                    {
                        transform.Rotate(Vector3.forward * Time.deltaTime * 360.0f * speed);
                    }
                    else
                    {
                        transform.Rotate(Vector3.forward * Time.deltaTime * -360.0f * speed);
                    }
                    GetComponent<Animator>().SetBool("roll", true);
                    GetComponent<Animator>().SetBool("dash", false);
                }
                Vector2 position = pos + (direction * speed * Time.deltaTime);
                transform.position = position;
            }
            else
            {
                if (!GetComponent<SpriteRenderer>().flipX)
                {
                    transform.Rotate(Vector3.forward * Time.deltaTime * 360.0f);
                }
                else
                {
                    transform.Rotate(Vector3.forward * Time.deltaTime * -360.0f);
                }
                Vector2 position = pos + (direction * slowSpeed * Time.deltaTime);
                transform.position = position;
            }
        }
        else
        {
            GetComponent<Transform>().forward = new Vector3(0.0f, 0.0f, 1.0f);
            GetComponent<Animator>().SetBool("dash", false);
            GetComponent<Animator>().SetBool("roll", false);
        }

        Breathe();
        //GetComponent<Animator>().SetBool("moving", moving);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!stunned)
        {
            if (collision.tag == "TenCent")
            {
                tenCents++;
                coinCount++;
            }
            else if (collision.tag == "TwentyCent")
            {
                twentyCents++;
                coinCount++;
            }
            else if (collision.tag == "FiftyCent")
            {
                fiftyCents++;
                coinCount++;
            }
            else if (collision.tag == "OneDollar")
            {
                oneDollars++;
                coinCount++;
            }
            else if (collision.tag == "TwoDollar")
            {
                twoDollars++;
                coinCount++;
            }

            if (collision.tag == "Boot")
            {
                speed++;
                GetComponent<TrailRenderer>().time += 0.1f;
                GetComponent<TrailRenderer>().startColor = new Color(0.0f, 0.792f, 1.0f, 1.0f);
            }

            if (collision.tag == "Lightning")
            {
                clickCount--;
                GetComponent<TrailRenderer>().startColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            }
        }

        if (collision.tag == "Obstacle")
        {
            stunned = true;
            GetComponent<TrailRenderer>().startColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }

        if (collision.tag == "TopWall" || collision.tag == "LeftWall" || collision.tag == "RightWall" || collision.tag == "BottomWall")
        {
            GetComponent<TrailRenderer>().startColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            moving = false;
            clickCount = 0;
            stunned = false;
        }

        GetComponent<Animator>().SetBool("stunned", stunned);

        if (collision.tag == "TopWall")
        {
            wall = EWHICHWALL.TOP_WALL;
            GetComponent<Animator>().SetBool("top", true);
            GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, 3.3f, GetComponent<Transform>().position.z);
        }
        else if (collision.tag == "LeftWall")
        {
            wall = EWHICHWALL.LEFT_WALL;
            GetComponent<Animator>().SetBool("side", true);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (collision.tag == "RightWall")
        {
            wall = EWHICHWALL.RIGHT_WALL;
            GetComponent<Animator>().SetBool("side", true);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (collision.tag == "BottomWall")
        {
            wall = EWHICHWALL.BOTTOM_WALL;
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            GetComponent<Animator>().SetBool("bottom", true);
            GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, -4.15f, GetComponent<Transform>().position.z);
        }
    }

    public int GetTotalCoins()
    {
        return (coinCount);
    }

    public void SetCoinCount(int _count)
    {
        coinCount = _count;
    }

    public void SetMoving(bool _move)
    {
        moving = _move;
    }

    private void Breathe()
    {
        if (grow)
        {
            transform.localScale *= 1.0005f;
            if (transform.localScale.x > 0.51f)
            {
                grow = false;
            }
        }
        else
        {
            transform.localScale *= 0.9995f;
            if (transform.localScale.x < 0.49f)
            {
                grow = true;
            }
        }
    }

    private float Map(float _oldMin, float _oldMax, float _newMin, float _newMax, float _value)
    {
        float oldRange = _oldMax - _oldMin;
        float newRange = _newMax - _newMin;
        float result = (((_value - _oldMin) * newRange) / oldRange) + _newMin;

        return result;
    }

    public bool isStunned()
    {
        return stunned;
    }
}
