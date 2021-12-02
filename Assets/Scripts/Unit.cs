using System.Collections;
using Buildings;
using UnityEngine;

public class Unit : MonoBehaviour
{
   [SerializeField] private float unitSpeed = 3f;
   [SerializeField] private float unitRotationSpeed = 10f;
   [SerializeField] private Team team;
   [SerializeField] private BuildingType type;
   [SerializeField] private float unitDestroyDelay = 2f;
   [SerializeField] private GameObject deathParticles;

   public Building startBuilding;

   private Animator _unitAnimator;
   private Vector3 _targetPos;
   private Collider _collider;
   private bool _isDead;
   private static readonly int IsDead = Animator.StringToHash("IsDead");

   public Team GetTeam() => team;
   public BuildingType GetUnitType() => type;
   public void SetTargetPos(Vector3 position) => _targetPos = position;

   private void Start()
   {
      _collider = GetComponent<Collider>();
      _unitAnimator = GetComponent<Animator>();
      type = startBuilding.GetBuildingType();
   }

   private void Update()
   {
      if(_isDead) return;
      
      transform.position = Vector3.MoveTowards(transform.position, _targetPos, 
         unitSpeed * Time.deltaTime);
      
      var lookAtPos = _targetPos - transform.position;
      var newRotation = Quaternion.identity;
      if (lookAtPos == Vector3.zero)
         Destroy(gameObject);
      else
         newRotation = Quaternion.LookRotation(lookAtPos);
      transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * unitRotationSpeed);
   }

   private void OnCollisionEnter(Collision other)
   {
      if (other.gameObject.CompareTag(gameObject.tag))
         Physics.IgnoreCollision(other.collider, _collider);
      else
      {
         _isDead = true;
         _collider.enabled = false;
         transform.rotation = Quaternion.Euler(0, Random.Range(-90,90),0);
         _unitAnimator.SetTrigger(IsDead);
         StartCoroutine(DestroyUnitObject(unitDestroyDelay));
      }
   }

   private IEnumerator DestroyUnitObject(float delay)
   {
      yield return new WaitForSeconds(delay);
      Instantiate(deathParticles, transform.position, Quaternion.identity);
      Destroy(gameObject);
   }
}
