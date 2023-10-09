using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersController : MonoBehaviour
{
    // Start is called before the first frame update

    private static ManagersController instance;

    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
}
