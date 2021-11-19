using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private float yOffset = 2;
    [SerializeField] private bool isCustomCamera;
    [SerializeField] private Transform customPoint;
    private void Update()
    {   
        transform.LookAt(isCustomCamera ? customPoint.transform.position : 
            Camera.main.transform.position, Vector3.up);
        transform.position = transform.parent.position + new Vector3(0, yOffset, 0);
    }

    public void ChangeOffset(float value) => yOffset = value;
}
