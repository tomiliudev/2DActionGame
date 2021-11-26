using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    [Header("プレイヤーの跳ねる高さ")] [SerializeField] public float boundHight;
    [HideInInspector] public bool isPlayerStepOn;
}
