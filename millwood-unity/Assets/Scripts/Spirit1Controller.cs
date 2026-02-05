using System;
using UnityEngine;

public class Spirit1Controller : MonoBehaviour
{
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject leg;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(body,  transform.position, transform.rotation);
        }
    }
}
