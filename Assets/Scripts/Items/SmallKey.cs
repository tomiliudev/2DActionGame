using System.Collections;
using UnityEngine;

public class SmallKey : MonoBehaviour
{
    GameObject magnet;
    bool isHit;
    Hashtable hash;
    
    Vector3 magnetPos = Vector3.zero;
    float distance = 0f;
    float speed = 1f;

    private void Start()
    {
        magnet = GameObject.FindWithTag("Magnet");
        hash = new Hashtable();
        hash.Add("easeType", iTween.EaseType.linear);
    }

    
    private void Update()
    {
        if (isHit)
        {
            magnetPos = magnet.transform.position;
            distance = Vector2.Distance(transform.position, magnetPos);

            // hash.Addだとキーすでにあるからエラーになるけど、上書きしたい場合はこのように指定
            hash["position"] = magnetPos;
            hash["time"] = distance / speed;
            iTween.MoveUpdate(gameObject, hash);
        }
    }

    private void FixedUpdate()
    {
        if (magnet != null)
        {
            var hit = Physics2D.Raycast(transform.position, magnet.transform.position - transform.position, 2f, 1 << LayerMask.NameToLayer("Magnet"));
            if (hit.collider != null)
            {
                isHit = true;
            }
            else
            {
                isHit = false;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            float yPos = transform.position.y;

            Hashtable hash = new Hashtable();
            hash.Add("y", yPos + 1.5f);
            hash.Add("time", 0.5f);
            hash.Add("oncomplete", "OnComplete");
            iTween.MoveTo(gameObject, hash);
        }
    }

    private void OnComplete()
    {
        Destroy(gameObject);
    }
}
