using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponController : MonoBehaviour {
    public float aimRotationSpeed = 5f;
    public float roundsPerMinute = 60f;
    public float shootDistance = 500f;
    public LayerMask hitLayer;

    [NonSerialized] public Vector2 aimAtPosition;
    private Coroutine shootingCoroutine;

    private float timeBetweenShots;
    private float timeSinceLastShot;
    private ParticleSystem weaponParticleSystem;
    private Transform weaponSocket;

    public float Speed { get; set; }

    public bool IsShooting { get; private set; }

    public bool IsAimable => !Mathf.Approximately(aimRotationSpeed, 0);

    private void Awake() {
        weaponSocket = transform.Find("Graphics/weaponSocket");
        Assert.IsNotNull(weaponSocket, "weaponSocket == null");

        weaponParticleSystem = GetComponentInChildren<ParticleSystem>();

        timeBetweenShots = 60f / roundsPerMinute;
    }

    private void LateUpdate() {
        if (IsAimable) AimWeapon();
    }

    private void AimWeapon() {
        var direction = aimAtPosition - (Vector2) weaponSocket.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        weaponSocket.rotation = Quaternion.Lerp(weaponSocket.rotation, rotation, aimRotationSpeed * Time.deltaTime);
    }

    public void StartShooting() {
        if (IsShooting) {
            return;
        }
        IsShooting = true;
        shootingCoroutine = StartCoroutine(Shooting(Mathf.Max(0f, timeSinceLastShot + timeBetweenShots - Time.time)));
    }

    private IEnumerator Shooting(float startDelay) {
        yield return new WaitForSeconds(startDelay);
        while (true) {
            weaponParticleSystem.Play();
            FireShot();
            timeSinceLastShot = Time.time;
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
    
    public void StopShooting() {
        IsShooting = false;
        StopCoroutine(shootingCoroutine);
    }

    private void FireShot() {
        Debug.DrawRay(weaponSocket.position, weaponSocket.right * shootDistance, Color.green, 0.5f);
        //todo fix fire rate bug
    }
}