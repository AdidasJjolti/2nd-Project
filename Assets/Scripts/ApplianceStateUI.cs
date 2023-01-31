using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplianceStateUI : MonoBehaviour
{
    [SerializeField] private Transform[] appliancePoints;
    [SerializeField] private GameObject[] applianceStates;
    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;

        applianceStates[0].transform.position = camera.WorldToScreenPoint(appliancePoints[0].position + new Vector3(2, 1, 6));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
