using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPong3D : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if(transform.position.z < -11)
        {
            Destroy(gameObject);
        }
    }
}
