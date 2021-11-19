using Buildings;
using UnityEngine;

public class Unit : MonoBehaviour
{
   [SerializeField] private Team team;

   public Team GetTeam() => team;

}
