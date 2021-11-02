using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private const string GroundTag = "Ground";
    private bool isInGround;
    public bool IsInGround
    {
        get
        {
            return isInGround;
        }
        set
        {
            isInGround = value;
        }
    }

    private bool isEnterGround, isStayGround, isExitGround;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isEnterGround || isStayGround)
        {
            IsInGround = true;
        }
        else if (isExitGround)
        {
            IsInGround = false;
        }

        isEnterGround = false;
        isStayGround = false;
        isExitGround = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GroundTag)
        {
            isEnterGround = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == GroundTag)
        {
            isStayGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == GroundTag)
        {
            isExitGround = true;
        }
    }
}
