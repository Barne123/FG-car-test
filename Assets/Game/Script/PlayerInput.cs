using UnityEngine;

namespace FG {
    [RequireComponent(typeof(MovementController), typeof(WeaponController))]
    public class PlayerInput : MonoBehaviour {
        private const string fireID = "Fire1";
        private MovementController movement;
        private Camera playerCamera;
        private WeaponController weapon;

        private void Awake() {
            movement = GetComponent<MovementController>();
            playerCamera = Camera.main;
            weapon = GetComponent<WeaponController>();
        }

        private void Update() {
            movement.movementInput.Set(Input.GetAxis("Turn"), Input.GetAxis("Drive"));
            movement.IsBraking = Input.GetButton("Brake");
            weapon.aimAtPosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetButtonDown(fireID)) {
                weapon.StartShooting();
            }
            else if (Input.GetButtonUp(fireID)) {
                weapon.StopShooting();
            }
        }
    }
}