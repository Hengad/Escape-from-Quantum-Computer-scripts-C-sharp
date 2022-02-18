using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameManager gameManager;
    public SpriteManager spriteManager;

    public bool shotByPlayer;
    public enum BulletType
    {
        NONE,
        ElectronBullet,
        NeutronBullet,
        PhotonBullet,
        EntanglementBullet
    }

    private Vector2 shootDir;
    private Rigidbody2D rb;
    private float bulletSpeed;
    private int damage;
    private BulletType bulletType;

    public void Setup(Vector2 shootDir, int damage, float bulletSpeed, bool shotByPlayer, BulletType bulletType)
    {
        spriteManager = GameObject.FindGameObjectWithTag("GameManager").transform.GetChild(0).GetComponent<SpriteManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        this.damage = damage;
        this.shootDir = shootDir;
        this.bulletSpeed = bulletSpeed;
        this.shotByPlayer = shotByPlayer;
        this.bulletType = bulletType;
        if (bulletType == BulletType.NONE)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        }

        else if (bulletType == BulletType.ElectronBullet)
        {
            if(!shotByPlayer)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteManager.neutronBullet;
            }
            else
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteManager.electronBullet;
            }
        }

        else if (bulletType == BulletType.NeutronBullet)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteManager.neutronBullet;
        }
        else if (bulletType == BulletType.EntanglementBullet)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteManager.entanglementBullet;
        }
        else if (bulletType == BulletType.PhotonBullet)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteManager.photonBullet;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.position += new Vector3(shootDir.x, shootDir.y, 0) * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (shotByPlayer)
            {
                if (bulletType == BulletType.EntanglementBullet)
                {
                    if (!collision.gameObject.GetComponent<EnemyController>().entanglementTagged)
                    {
                        collision.gameObject.GetComponent<EnemyController>().entanglementTagged = true;
                    }
                }

                if (collision.gameObject.GetComponent<EnemyController>().enemyType == EnemyController.EnemyType.Graphene && bulletType == BulletType.PhotonBullet)
                {
                    damage += 1;
                }

                else if(collision.gameObject.GetComponent<EnemyController>().enemyType == EnemyController.EnemyType.Metal && bulletType == BulletType.ElectronBullet)
                {
                    damage += 1;
                }

                else if(collision.gameObject.GetComponent<EnemyController>().enemyType == EnemyController.EnemyType.Metal && bulletType == BulletType.PhotonBullet)
                {
                    damage -= 1;
                }

                Destroy(gameObject);

                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                if (collision.gameObject.GetComponent<EnemyController>().entanglementTagged)
                {
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        if (enemies[i].GetComponent<EnemyController>().entanglementTagged == true)
                        {
                            enemies[i].gameObject.GetComponent<EnemyController>().OnHit(damage);
                        }
                    }
                    return;
                }

                collision.gameObject.GetComponent<EnemyController>().OnHit(damage);
            }
        }

        else if (collision.gameObject.tag == "Player")
        {
            if (!shotByPlayer)
            {
                collision.gameObject.GetComponent<PlayerController>().OnHit();
                Destroy(gameObject);
            }
        }

        else if (collision.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }

        else if (collision.gameObject.tag == "Gate")
        {
            Destroy(gameObject);
        }
    }
}
