using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAI;

public class TeamAssignment : MonoBehaviour
{
   [SerializeField] private Team playerTeam;
   [SerializeField] private List<EnemyDecisions> enemies;
   
   private static TeamAssignment _instance;
   private void Awake()
   {
      if (_instance == null) 
         _instance = this; 
      else if(_instance == this)
         Destroy(gameObject);
   }
    
   public static TeamAssignment GetInstance() => _instance;

   public Team GetPlayerTeam() => playerTeam;
   public EnemyDecisions GetEnemyByTeam(Team team) => enemies[(int)team];
}
