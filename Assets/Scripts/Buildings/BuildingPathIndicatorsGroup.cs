using System;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class BuildingPathIndicatorsGroup : MonoBehaviour
    {
        [SerializeField] [Range(1, 3)] private int indicatorCount;
        [SerializeField] private List<SpriteRenderer> circles;

        public void SetCircleColor(int circleIndex, Team team) => circles[circleIndex].color =
            TeamColors.GetInstance().GetBuildingPathsCountIndicatorColor(team);

        public void SetColorForCircles(int circleIndex, Team team)
        {
            for(var i = 0; i < circleIndex; i++)
                circles[i].color =
                    TeamColors.GetInstance().GetBuildingPathsCountIndicatorColor(team);
        }

        public void ClearCircleColor(int circleIndex) => circles[circleIndex].color = Color.white;

        public void ClearAllCircles()
        {
            foreach (var circle in circles)
                circle.color = Color.white;
        }
    }
}
