using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchDescriptor : MonoBehaviour
{
    public InventoryUIItem item;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           item.ShowInfo();           
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            item.ShowInfo();
        }
    }
}
