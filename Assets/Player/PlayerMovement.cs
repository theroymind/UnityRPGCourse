using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;
    AICharacterControl aiCharacterControl = null;
    GameObject walkTarget = null;

    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;

    bool isInDirectMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        aiCharacterControl = GetComponent<AICharacterControl>();
        walkTarget = new GameObject("walkTarget");
        currentDestination = transform.position;

        cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
    }

    void ProcessMouseClick(RaycastHit raycastHit, int layerHit) {
        switch (layerHit) {
            case walkableLayerNumber:
                walkTarget.transform.position = raycastHit.point;
                aiCharacterControl.SetTarget(walkTarget.transform);
                break;
            case enemyLayerNumber:
                print("enemyLayer");
                GameObject enemy = raycastHit.collider.gameObject;
                aiCharacterControl.SetTarget(enemy.transform);
                break;
            default:
                Debug.LogWarning("Unable to process click");
                return;
        }
    }

    private void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        // calculate camera relative direction to move:
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

        thirdPersonCharacter.Move(movement, false, false);
    }
}

