using UnityEngine;

public sealed class TouchManager : SingletonMonoBehaviour<TouchManager>
{
    RaycastHit2D[] hits;
    private void Update()
    {
        //hits = null;
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    hits = Physics2D.RaycastAll(worldPoint, Vector2.zero);

        //    foreach (var hit in hits)
        //    {
        //        if (hit)
        //        {
        //            // ドアのアニメーション
        //            DoDoorAnimation(hit);
        //        }
        //    }
        //}
    }

    private void DoDoorAnimation(RaycastHit2D hit)
    {
        if (hit.collider.tag != GameConfig.DoorTag) return;
        hit.collider.GetComponent<Door>().OpenDoorAnimation();
    }
}
