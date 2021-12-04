using UnityEngine;

public class Fire : MonoBehaviour
{
    GameManager gm;
    private void Start()
    {
        gm = GameManager.Instance;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
        {
            gm.player.OnDamage();
        }
    }
}