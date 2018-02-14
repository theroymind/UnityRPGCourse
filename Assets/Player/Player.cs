using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class Player : MonoBehaviour, IDamageable {
    [SerializeField] int enemyLayer = 9;
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float dmaagePerHit = 10f;
    [SerializeField] float minTimeBetweenHits = 1f;
    [SerializeField] float maxAttackRange = 2f;
    
    GameObject currentTarget;
    CameraRaycaster cameraRaycaster = null;
    float currentHealthPoints;
    float lastHitTime;

    public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

    void Start() {
        currentHealthPoints = maxHealthPoints;
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
    }

    void OnMouseClick(RaycastHit raycastHit, int layerHit) {
        if(layerHit == enemyLayer) {
            var enemy = raycastHit.collider.gameObject;
            // Check enemy is in range
            if((enemy.transform.position - transform.position).magnitude > maxAttackRange) {
                return;
            }

            currentTarget = enemy;

            if (Time.time - lastHitTime > minTimeBetweenHits) {
                enemy.GetComponent<Enemy>().TakeDamage(10f);
                lastHitTime = Time.time;
            }
        }
    }

    public void TakeDamage(float damage) {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);

        if(currentHealthPoints <= 0) {
            // Destroy(gameObject);
        }
    }
}
