using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LoadingController : BaseController
{
    // Start is called before the first frame update
    void Start()
    {
        AdmobUtility.Instance.InitializeAdmob();
        StartCoroutine(LoadTitle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadTitle()
    {
        yield return new WaitForSeconds(5f);
        GameManager.Instance.LoadSceneTo("Title");
    }
}
