using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelfDestroying : MonoBehaviour
{
    [SerializeField] private float delay = 1;
    private void Start() => StartCoroutine(DestroyWithDelay(delay));
    private IEnumerator DestroyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
