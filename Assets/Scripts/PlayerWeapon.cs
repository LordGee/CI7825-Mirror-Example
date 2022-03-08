using Mirror;
using UnityEngine;

public class PlayerWeapon : NetworkBehaviour {
    private int selectedWeaponLocal = 1;
    private Weapon activeWeapon;
    private float weaponCooldownTime;

    [SerializeField] private GameObject[] weaponArray;

    [SyncVar(hook = nameof(OnWeaponChange))]
    public int activeWeaponSynced = 1;

    private void OnWeaponChange(int oldWeapon, int newWeapon) {
        if (oldWeapon >= 0 && oldWeapon < weaponArray.Length && weaponArray[oldWeapon] != null) {
            weaponArray[oldWeapon].SetActive(false);
        }
        if (newWeapon >= 0 && newWeapon < weaponArray.Length && weaponArray[newWeapon] != null) {
            weaponArray[newWeapon].SetActive(true);
            weaponArray[activeWeaponSynced].TryGetComponent<Weapon>(out activeWeapon);
        }
    }

    [Command]
    public void CmdChangeActiveWeapon(int newIndex) {
        activeWeaponSynced = newIndex;
    }

    [Command]
    private void CmdShootRay() {
        RpcFireWeapon();
    }

    [ClientRpc]
    private void RpcFireWeapon() {
        GameObject bullet = Instantiate(activeWeapon.weaponBullet, 
            activeWeapon.weaponFirePosition.position, activeWeapon.weaponFirePosition.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * activeWeapon.weaponSpeed;
        if (bullet) {
            Destroy(bullet, activeWeapon.weaponLife);
        }
    }

    void Awake() {
        foreach (var item in weaponArray) {
            if (item != null) {
                item.SetActive(false);
            }
        }
        if (selectedWeaponLocal < weaponArray.Length && weaponArray[selectedWeaponLocal] != null) {
            weaponArray[selectedWeaponLocal].TryGetComponent<Weapon>(out activeWeapon);
        }
    }

    void Update() {
        if (!isLocalPlayer) { return; }
        if (Input.GetButtonDown("Fire2")) {
            selectedWeaponLocal += 1;
            if (selectedWeaponLocal >= weaponArray.Length) {
                selectedWeaponLocal = 0;
            }
            CmdChangeActiveWeapon(selectedWeaponLocal);
        }
        if (Input.GetButtonDown("Fire1")) {
            if (activeWeapon && Time.time > weaponCooldownTime && activeWeapon.weaponAmmo > 0) {
                weaponCooldownTime = Time.time + activeWeapon.weaponCooldown;
                activeWeapon.weaponAmmo -= 1;
                CmdShootRay();
            }
        }
    }
}
