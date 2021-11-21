using System.Linq;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] e_MoveType moveType;
    [SerializeField] float moveSpeed = 3f;

    private enum e_MoveType
    {
        freeze,
        up,
        down,
        left,
        right
    }

    private Vector2 localPosition;
    
    private float cameraColliderUpY;
    private float cameraColliderDownY;

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
        Move();
    }

    private void Move()
    {
        Vector2 direction = Vector2.up;
        switch (moveType)
        {
            case e_MoveType.freeze:
                return;
            case e_MoveType.up:
                direction = transform.up;
                break;
            case e_MoveType.down:
                direction = -transform.up;
                break;
            case e_MoveType.left:
                break;
            case e_MoveType.right:
                break;
        }

        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "CameraCollider")
        {
            var cameraCollider = collision.GetComponent<PolygonCollider2D>();
            cameraColliderUpY = cameraCollider.points.ElementAt(1).y;
            cameraColliderDownY = cameraCollider.points.ElementAt(2).y;
            switch (moveType)
            {
                case e_MoveType.up:
                    transform.localPosition = new Vector2(localPosition.x, cameraColliderDownY);
                    break;
                case e_MoveType.down:
                    transform.localPosition = new Vector2(localPosition.x, cameraColliderUpY);
                    break;
                case e_MoveType.left:
                    break;
                case e_MoveType.right:
                    break;
            }
        }
    }
}
