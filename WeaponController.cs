using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public class OnShootEventArgs : EventArgs
    {
        public Vector2 shootStartPos;
        public Vector2 shootEndPos;
        public bool shotByPlayer;
    }

    public GameObject owner;
    public GameManager gameManager;
    public SpriteManager spriteManager;
    public SoundController soundController;

    // adding new weapon, remember: spritemanager, WeaponType, WeaponSprites, SetToolbarSprite from InventoryManager

    public enum WeaponType
    {
        NONE,
        ElectronPistol,
        EntanglementGun,
        PhotonGun,
        Heart // to make things easier. If I had more items that are not weapons then I'd implement this differently
    }

    public Dictionary<WeaponType, Sprite> WeaponSprites;
    public WeaponType selectedWeapon;
    private float nextFire;

    private BulletController.BulletType bulletType;
    private float bulletSpeed;
    public float fireRate;
    public int damage;
    public int energyChange;
    public int errorRate; // not implemented

    private void Awake()
    {
        spriteManager = GameObject.FindGameObjectWithTag("GameManager").transform.GetChild(0).GetComponent<SpriteManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        soundController = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundController>();
        WeaponSprites = new Dictionary<WeaponType, Sprite>();
        // add new weapons here
        WeaponSprites.Add(WeaponType.NONE, null);
        WeaponSprites.Add(WeaponType.ElectronPistol, spriteManager.electronPistol);
        WeaponSprites.Add(WeaponType.EntanglementGun, spriteManager.entanglementGun);
        WeaponSprites.Add(WeaponType.PhotonGun, spriteManager.photonGun);

        if (owner.tag == "Player")
        {
            owner.GetComponent<PlayerController>().OnShoot += ShootBullet_OnShoot;
            owner.transform.GetChild(0).GetComponent<WeaponController>().selectedWeapon = WeaponType.NONE;
        }

        else if (owner.tag == "Enemy")
        {
            owner.GetComponent<EnemyController>().OnShoot += ShootBullet_OnShoot;
        }

        nextFire = 0.0f;
        energyChange = 0;
        bulletType = BulletController.BulletType.NeutronBullet;
        errorRate = 0;
    }

    private void Update()
    {
        transform.position = owner.transform.position;
        transform.rotation = owner.transform.rotation;
    }

    private void UpdateWeaponType()
    {
        if (selectedWeapon == WeaponType.NONE)
        {
            this.damage = 0;
            this.bulletSpeed = 0.0f;
            this.fireRate = 0.0f;
            this.bulletType = BulletController.BulletType.NONE;
            this.errorRate = 0;
            this.energyChange = 0;
        }

        else if (selectedWeapon == WeaponType.ElectronPistol)
        {
            this.damage = 2;
            this.bulletSpeed = 4.0f;
            this.fireRate = 0.45f;
            this.bulletType = BulletController.BulletType.ElectronBullet;
            this.errorRate = 0;
            this.energyChange = 2;
        }

        else if (selectedWeapon == WeaponType.EntanglementGun)
        {
            this.damage = 0;
            this.bulletSpeed = 15.0f;
            this.fireRate = 1.0f;
            this.bulletType = BulletController.BulletType.EntanglementBullet;
            this.energyChange = -30;
        }

        else if (selectedWeapon == WeaponType.PhotonGun)
        {
            this.damage = 2;
            this.bulletSpeed = 8.0f;
            this.fireRate = 0.25f;
            this.bulletType = BulletController.BulletType.PhotonBullet;
            this.energyChange = 1;
        }
    }

    private void ShootBullet_OnShoot(object sender, OnShootEventArgs e)
    {
        if (this.selectedWeapon != WeaponType.NONE)
        {
            if (Time.time > nextFire)
            {
                UpdateWeaponType();
                nextFire = Time.time + fireRate;

                if (owner.tag == "Player")
                {
                    if (owner.gameObject.GetComponent<PlayerController>().energy + energyChange < 0)
                    {
                        return;
                    }

                    else if(owner.gameObject.GetComponent<PlayerController>().energy + energyChange > owner.gameObject.GetComponent<PlayerController>().maxEnergy)
                    {
                        // if control here, no else check
                    }

                    else
                    {
                        owner.gameObject.GetComponent<PlayerController>().energy += energyChange;
                    }

                    soundController.shootSound.Play();
                }

                float angle = Mathf.Atan2(e.shootStartPos.y - e.shootEndPos.y, e.shootStartPos.x - e.shootEndPos.x) * Mathf.Rad2Deg;
                Vector2 shootDirection = (e.shootEndPos - e.shootStartPos).normalized;
                e.shootStartPos += ((e.shootEndPos - e.shootStartPos).normalized) * 0.5f;

                Transform bulletTransform = Instantiate(gameManager.pfBullet, e.shootStartPos, Quaternion.Euler(new Vector3(0f, 0f, angle - 90.0f)));
                bulletTransform.GetComponent<BulletController>().Setup(shootDirection, damage, this.bulletSpeed, e.shotByPlayer, bulletType);
            }
        }   
    }
}
