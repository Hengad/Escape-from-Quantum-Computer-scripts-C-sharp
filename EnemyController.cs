using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public event EventHandler<WeaponController.OnShootEventArgs> OnShoot;

    public Transform player;
    public int health;
    public bool entanglementTagged;

    Rigidbody2D rb;
    SpriteRenderer sprite;
    GameManager gameManager;
    SpriteManager spriteManager;
    SoundController soundController;

    GameObject entanglementTagSprite;

    public enum EnemyType
    {
        Electron,
        Metal,
        Graphene
    }

    public EnemyType enemyType;
    public WeaponController.WeaponType weaponDrop;

    private float range;
    private float speed;
    private float minDistance = 4.0f;
    private float maxDistance = 12.0f;
    private Color originalColor;

    public void OnHit(int damage)
    {
        soundController.enemyHitSound.Play();
        StartCoroutine(OnHitTimer());

        if (entanglementTagged)
        {
            entanglementTagSprite.GetComponent<SpriteRenderer>().sprite = spriteManager.entanglementTagged;
        }

        this.health -= damage;
        if (health <= 0)
        {
            if(weaponDrop != WeaponController.WeaponType.NONE)
            {
                Transform droppedItem = Instantiate(gameManager.droppedWeapon, transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0.0f)));
                droppedItem.GetComponent<DropController>().dropType = weaponDrop;

                if(weaponDrop == WeaponController.WeaponType.EntanglementGun)
                {
                    droppedItem.GetComponent<SpriteRenderer>().sprite = spriteManager.toolbarEntanglementGun;
                }

                else if (weaponDrop == WeaponController.WeaponType.PhotonGun)
                {
                    droppedItem.GetComponent<SpriteRenderer>().sprite = spriteManager.toolbarPhotonGun;
                }
            }

            else // if no weapon drop, has a chance to drop health
            {
                System.Random _random = new System.Random();
                int randomNum = _random.Next(1, 6);
                Debug.Log(randomNum);
                if(randomNum == 1) // 1/6 chance
                {
                    Transform droppedItem = Instantiate(gameManager.droppedWeapon, transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0.0f)));
                    droppedItem.GetComponent<SpriteRenderer>().sprite = spriteManager.heart;
                    droppedItem.GetComponent<DropController>().dropType = WeaponController.WeaponType.Heart;
                }
            }
            
            Destroy(gameObject);
        }
    }

    private IEnumerator OnHitTimer()
    {
        sprite.color = Color.green;
        yield return new WaitForSeconds(0.2f);
        sprite.color = originalColor;
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        spriteManager = GameObject.FindGameObjectWithTag("GameManager").transform.GetChild(0).GetComponent<SpriteManager>();
        soundController = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundController>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalColor = sprite.color;
        entanglementTagged = false;

        entanglementTagSprite = Instantiate(new GameObject(), transform);
        entanglementTagSprite.AddComponent<SpriteRenderer>();
        entanglementTagSprite.GetComponent<SpriteRenderer>().sortingLayerName = "Enemies";

        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = transform.GetChild(0).GetComponent<WeaponController>().WeaponSprites[transform.GetChild(0).GetComponent<WeaponController>().selectedWeapon];

        if (this.enemyType == EnemyType.Electron)
        {
            speed = 0.6f;
            health = 10;
        }

        else if (this.enemyType == EnemyType.Graphene)
        {
            speed = 1.0f;
            health = 12;
        }

        else if (this.enemyType == EnemyType.Metal)
        {
            speed = 1.3f;
            health = 12;
        }
    }

    private void UpdateShooting()
    {
        OnShoot?.Invoke(this, new WeaponController.OnShootEventArgs
        {
            shootStartPos = transform.position,
            shootEndPos = player.transform.position,
            shotByPlayer = false
        });
    }

    private void FixedUpdate()
    {
        range = Vector2.Distance(transform.position, player.position);

        if(range < maxDistance)
        {
            lookTowardsPlayer();
            UpdateShooting();
        }

        if (range > minDistance && range < maxDistance)
        {
            rb.velocity = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y).normalized * speed;
        }
        else
        {
            rb.velocity = new Vector2(0.0f, 0.0f);
        }

        if(entanglementTagSprite != null)
        {
            entanglementTagSprite.transform.position = transform.position;
        }
    }

    private void lookTowardsPlayer()
    {
        Vector2 enemyPosition = transform.position;
        Vector2 playerPosition = player.position;
        float angle = Mathf.Atan2(enemyPosition.y - playerPosition.y, enemyPosition.x - playerPosition.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90.0f));
    }
}
