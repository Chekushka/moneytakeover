using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScaling : MonoBehaviour
{
    private Animator _animator;
    private static readonly int IsDisable = Animator.StringToHash("isDisable");
    private static readonly int IsEnable = Animator.StringToHash("isEnable");

    private void Awake() => _animator = GetComponent<Animator>();
    private void OnDisable() => _animator.SetTrigger(IsDisable);
    private void OnEnable() => _animator.SetTrigger(IsEnable);
}
