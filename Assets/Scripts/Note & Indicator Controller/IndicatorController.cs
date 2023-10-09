using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour
{
    // Start is called before the first frame update
    public double timeElapse = 3f;
    public double time;
    public GRADE grade;

    void Start()
    {
    
    }

    public void ResetTime() => time = 0;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= timeElapse)
        {
            Destroy(gameObject);
        }
    }
}
