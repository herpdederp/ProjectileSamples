using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerProjectileController : MonoBehaviour
{
    public int frame;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }
    private void FixedUpdate()
    {

        transform.Translate(Vector3.forward * Time.fixedDeltaTime * 10f);
    }
}
