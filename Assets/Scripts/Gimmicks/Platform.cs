using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] PolygonCollider2D cameraArea;

    private Vector2 localPosition;

    // Start is called before the first frame update
    void Start()
    {
        localPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (transform.localPosition.y < cameraArea.points.ElementAt(2).y)
        {
            transform.localPosition = new Vector2 (localPosition.x, 4.5f);
        }
        else
        {
            transform.Translate(-transform.up * 3f * Time.deltaTime);
        }
    }
}
