using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class lockcanvastoidle : MonoBehaviour
{
    public GameObject canvastolock;

    public string scenetolockto;
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == scenetolockto)
        {
            canvastolock.SetActive(true);
        }
        else
        {
            canvastolock.SetActive(false);
        }
    }
}
