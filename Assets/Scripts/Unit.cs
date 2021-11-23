using System;
using Buildings;
using UnityEngine;

public class Unit : MonoBehaviour
{
   [SerializeField] private float unitSpeed = 3f;
   [SerializeField] private float unitRotationSpeed = 10f;
   [SerializeField] private Team team;

   public Building startBuilding;
   
   private Vector3 _targetPos;
   private Collider _collider;

   public Team GetTeam() => team;
   public void SetTargetPos(Vector3 position) => _targetPos = position;

   private void Start() => _collider = GetComponent<Collider>();

   private void Update()
   {
      transform.position = Vector3.MoveTowards(transform.position, _targetPos, 
         unitSpeed * Time.deltaTime);
      
      var lookAtPos = _targetPos - transform.position;
      var newRotation = Quaternion.LookRotation(lookAtPos);
      transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * unitRotationSpeed);
   }

   private void OnCollisionEnter(Collision other)
   {
      if (other.gameObject.CompareTag(gameObject.tag))
         Physics.IgnoreCollision(other.collider, _collider);
   }
}
