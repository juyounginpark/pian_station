using UnityEngine;

public class interactPoint : MonoBehaviour
{
    public GameObject pointPrefab;


    public float yOffset = 1.5f; // 포인트 y 간격
    private void Start()
    {
        GameObject[] interactableObjects = GameObject.FindGameObjectsWithTag("interact");

        foreach (GameObject obj in interactableObjects)
        {
            Vector3 pointPosition = new Vector3(
                obj.transform.position.x,
                obj.transform.position.y + yOffset,
                obj.transform.position.z
            );

            GameObject point = Instantiate(pointPrefab, pointPosition, Quaternion.identity);

            point.transform.SetParent(obj.transform);
        }


    }
    
   
}