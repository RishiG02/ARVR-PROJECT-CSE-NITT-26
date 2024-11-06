using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.XR.CoreUtils;

public class FurniturePlacementManager : MonoBehaviour
{
    public GameObject SpawnableFurniture;

    public XROrigin sessionOrigin;

    public ARRaycastManager raycastManager;

    public ARPlaneManager planeManager;

    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                bool collision = raycastManager.Raycast(Input.GetTouch(0).position, raycastHits, TrackableType.PlaneWithinPolygon);

                if (collision && isButtonPressed() == false)
                {
                    if (SpawnableFurniture != null)
                    {
                        Debug.Log("Instantiating furniture...");
                        GameObject _object = Instantiate(SpawnableFurniture);

                        // Temporarily place object at origin to confirm it's visible
                        _object.transform.position = new Vector3(0, 0, 0);
                        _object.transform.rotation = Quaternion.identity; // Optional: set a default rotation

                        Debug.Log("Furniture placed at origin.");
                    }
                    else
                    {
                        Debug.LogWarning("SpawnableFurniture is null! Assign a prefab in the Inspector.");
                    }
                }

                // Disable planes once furniture is placed
                foreach (var plane in planeManager.trackables)
                {
                    plane.gameObject.SetActive(false);
                }
                planeManager.enabled = false;
            }
        }
    }

    public bool isButtonPressed()
    {
        if (EventSystem.current.currentSelectedGameObject?.GetComponent<Button>() == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SwitchFurniture(GameObject furniture)
    {
        SpawnableFurniture = furniture;
    }
}
