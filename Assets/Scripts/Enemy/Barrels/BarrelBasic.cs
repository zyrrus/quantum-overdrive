using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelBasic : AbstractBarrel
{
    // Bullet management
    private static List<GameObject> bullets;
    private static Transform bulletParent;
    public static bool bulletsSetup { get; set; }
    private int bulletIndex;

    private void SetupBasicBulletPool() {
        if (bulletsSetup) return;

        bullets = new List<GameObject>();

        GameObject newBullet;
        for (int i = 0; i < initialBulletCount; i++) {
            newBullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity, bulletParent);
            bullets.Add(newBullet);
            newBullet.SetActive(false);
            Debug.Log(newBullet.name);
        }
        bulletsSetup = true;
    }

    // Shooting management
    public override void Shoot() {
        if (!shotTimer.isOver) return;

        bulletIndex = 0;
        while (shotTimer.isOver) {
            if (bulletIndex >= bullets.Count) {
                GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity, bulletParent);
                bullets.Add(newBullet);
                newBullet.transform.rotation = transform.rotation;
                shotTimer.Reset();
            }
            // else if (bullets[bulletIndex] == null) {
            //     GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity, bulletParent);
            //     bullets[bulletIndex] = newBullet;
            //     newBullet.transform.rotation = transform.rotation;
            //     shotTimer.Reset();
            // }
            else if (!bullets[bulletIndex].activeInHierarchy) {
                bullets[bulletIndex].SetActive(true);
                bullets[bulletIndex].transform.position = bulletSpawnPoint.transform.position;
                bullets[bulletIndex].transform.rotation = transform.rotation;
                shotTimer.Reset();
            }
            else
                bulletIndex++;
        }
        // Debug.Log("Fired");
    }

    // Commented because it calls nonfunctional BulletBasic.setDirection
    // private void AimBullet(GameObject bullet) {
    //     BulletBasic bulletScript = bullet.GetComponent<BulletBasic>();
    //     bulletScript.setDirection(transform.forward);
    //     // Debug.DrawRay(bulletSpawnPoint.transform.position, transform.forward, Color.green, 1f);
    // }

    private void Update() {
        if (!shotTimer.isOver) shotTimer.Tick();
    }

    // Instantiation
    private void Awake() {
        bulletParent = Instantiate(bulletParentPrefab).transform;
        SetupBasicBulletPool();
        shotTimer = new Timer(shotCooldown);
    }
}
