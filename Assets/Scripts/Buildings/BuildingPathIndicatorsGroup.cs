using System;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class BuildingPathIndicatorsGroup : MonoBehaviour
    {
        [SerializeField] [Range(1, 3)] private int indicatorCount;
        [SerializeField] private List<SpriteRenderer> circles;

        public void ColorCircles(int circlesCount, Team team)
        {
            ClearAllCircles();
            for (var i = 0; i < circlesCount && i < circles.Count; i++)
                circles[i].color = TeamColors.GetInstance().GetBuildingPathsCountIndicatorColor(team);
        }

        public void ClearAllCircles()
        {
            foreach (var circle in circles)
                circle.color = Color.white;
        }
    }
}
