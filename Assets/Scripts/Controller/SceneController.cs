using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{


    [SerializeField] Treasure treasure;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (treasure.IsGetTreasure)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
