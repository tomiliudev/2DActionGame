using System.Collections;
using UnityEngine;

public sealed class Fly : Enemy
{
    private Vector3 originPos;
    private Vector3 movePos;
    const float MaxChangeValue = 1f;

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        StartCoroutine(UpdateMovePos());
    }

    private Vector3 prePos = Vector3.zero;
    private void FixedUpdate()
    {
        rb2D.MovePosition(Vector2.MoveTowards(transform.position, movePos, moveSpeed * Time.fixedDeltaTime));
    }


    IEnumerator UpdateMovePos()
    {
        while (true)
        {
            float x = UnityEngine.Random.Range(0.5f, MaxChangeValue);
            float y = UnityEngine.Random.Range(0.5f, MaxChangeValue);
            x = ConvertPos(x);
            y = ConvertPos(y);

            movePos = new Vector3(originPos.x + x, originPos.y + y, originPos.z);


            var sec = UnityEngine.Random.Range(0.1f, 2f);
            yield return new WaitForSeconds(sec);
        }
    }

    private float ConvertPos(float pos)
    {
        if (UnityEngine.Random.Range(0, 2) > 0)
        {
            return +pos;
        }
        else
        {
            return -pos;
        }
    }
}
