using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrappleCrosshairFeedback : MonoBehaviour
{
    private RaycastHit hit;
    private int maxDist;
    [SerializeField] private LayerMask layerMask;

    private Transform camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        layerMask = LayerMask.GetMask("whatIsGround");
        maxDist = 45;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(camera.transform.position, camera.TransformDirection(Vector3.forward), out hit, maxDist, layerMask))
        {
            this.GetComponent<Image>().color = new Color32(0, 100, 255, 255);
        }
        else
        {
            this.GetComponent<Image>().color = new Color32(255, 100, 0, 255);
        }
    }
}
