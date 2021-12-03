using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDontDestroy : MonoBehaviour
{
    private void Awake() => DontDestroyOnLoad(gameObject);
}
