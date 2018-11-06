using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public enum ECOINTYPE
    {
        TENCENT,
        TWENTYCENT,
        FIFTYCENT,
        ONEDOLLAR,
        TWODOLLAR
    };

    public ECOINTYPE type;
    public Sprite tenSprite;
    public Sprite twentySprite;
    public Sprite fiftySprite;
    public Sprite oneSprite;
    public Sprite twoSprite;

    private Transform particle;
    private PlayerScript playerScript;

    // Use this for initialization
    void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        ResetProperties();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerScript.isStunned() == false)
        {
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        particle.GetComponent<ParticleSystem>().Play();
        GetComponent<SpriteRenderer>().enabled = false;
        //GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1);

        ResetProperties();

        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void ResetProperties()
    {
        if (particle != null)
        {
            particle.gameObject.SetActive(false);
        }

        float x = Random.Range(-8.0f, 8.0f);
        float y = Random.Range(-3.5f, 3.5f);

        transform.position = new Vector3(x, y, 0.0f);

        int rand = Random.Range(0, 15);
        if (rand < 5)
        {
            type = ECOINTYPE.TENCENT;
            tag = "TenCent";
            GetComponent<SpriteRenderer>().sprite = tenSprite;
            particle = transform.GetChild(0);
        }
        else if (rand < 9 && rand >= 5)
        {
            type = ECOINTYPE.TWENTYCENT;
            tag = "TwentyCent";
            GetComponent<SpriteRenderer>().sprite = twentySprite;
            particle = transform.GetChild(1);
        }
        else if (rand < 12 && rand >= 9)
        {
            type = ECOINTYPE.FIFTYCENT;
            tag = "FiftyCent";
            GetComponent<SpriteRenderer>().sprite = fiftySprite;
            particle = transform.GetChild(2);
        }
        else if (rand == 12 || rand == 13)
        {
            type = ECOINTYPE.ONEDOLLAR;
            tag = "OneDollar";
            GetComponent<SpriteRenderer>().sprite = oneSprite;
            particle = transform.GetChild(3);
        }
        else if (rand == 14)
        {
            type = ECOINTYPE.TWODOLLAR;
            tag = "TwoDollar";
            GetComponent<SpriteRenderer>().sprite = twoSprite;
            particle = transform.GetChild(4);
        }

        particle.gameObject.SetActive(true);
    }
}
