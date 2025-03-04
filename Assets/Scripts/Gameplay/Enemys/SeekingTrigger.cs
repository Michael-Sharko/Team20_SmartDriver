using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SeekingTrigger : MonoBehaviour
{
    public bool onSeeking { get; private set; } = false;

    [SerializeField]
    private float seekingRadius = 80f;

    private SphereCollider seekingTrigger => gameObject.GetComponent<SphereCollider>();
    private void Start()
    {
        seekingTrigger.radius = seekingRadius;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
            onSeeking = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            onSeeking = false;
    }
}
