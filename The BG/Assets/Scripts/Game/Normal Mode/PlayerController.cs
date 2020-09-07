using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Arsenal[] arsenals;
    public Transform playerWeaponHand;
    public GameObject impactEffect;
    public AudioSource fireSound;

    private Animator animator;
    private Actions actions;
    private bool firstTimeShoot = true;

    private float fireRate = 0.5f;
    private int damage = 1;
    private float timer;

    private int layerToRay;
    private bool beingShooting;

    private ParticleSystem muzzleParticle;
    public GameObject cardridge;
    private GameObject spawnPositionCar;
    private GameController gameController;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        actions = GetComponentInChildren<Actions>();
        fireSound = GetComponent<AudioSource>();

        int enemy = 1 << LayerMask.NameToLayer("Enemy");
        int env = 1 << LayerMask.NameToLayer("Enviroment");
        layerToRay = enemy | env;

        InstantiateWeapon(arsenals[0].name);

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void InstantiateWeapon(string name)
    {
        foreach (Arsenal arsenal in arsenals)
        {
            if (arsenal.name == name)
            {
                if (playerWeaponHand.childCount > 0)
                    Destroy(playerWeaponHand.GetChild(0).gameObject);

                if (arsenal.weapon != null)
                {
                    GameObject newGun = (GameObject)Instantiate(arsenal.weapon);
                    muzzleParticle = newGun.GetComponentInChildren<ParticleSystem>();
                    spawnPositionCar = newGun.transform.GetChild(1).gameObject;
                    newGun.transform.parent = playerWeaponHand;
                    newGun.layer = playerWeaponHand.gameObject.layer;
                    newGun.transform.localPosition = new Vector3(-0.01f, 0.23f, -0.005f);
                    newGun.transform.localRotation = Quaternion.Euler(-80f, 0, 90f);
                }
                animator.runtimeAnimatorController = arsenal.controller;
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.gameOverFlag) return;

        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            if (!ApplicationUtil.GamePaused)
            {
                if (Input.GetButton("Fire1") && !beingShooting)
                {
                    timer = 0f;
                    actions.Attack();
                    StartCoroutine(Fire());
                }
            }
        }

        if (!gameController.instruction.enabled && firstTimeShoot)
        {
            if (ApplicationUtil.FirstLevel && ApplicationUtil.CurrentSceneIndex == 1)
            {
                gameController.instruction.text = "Left Click to shoot him. Shoot him to die!";
                gameController.instruction.enabled = true;
                firstTimeShoot = true;
            }
        }
    }

    IEnumerator Fire()
    {
        if (firstTimeShoot)
        {
            gameController.instruction.enabled = !gameController.instruction.enabled;
            firstTimeShoot = !firstTimeShoot;
        }

        beingShooting = true;
        fireSound.Play();
        GameObject localCardridge;
        if (spawnPositionCar) localCardridge = (GameObject)Instantiate(this.cardridge, spawnPositionCar.transform);

        muzzleParticle.Stop();
        muzzleParticle.Play();

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit, 100, layerToRay))
        {
            var health = raycastHit.collider.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(damage);

            GameObject impactObject = Instantiate(impactEffect, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
            Destroy(impactObject, 2f);

        }

        yield return new WaitForSeconds(fireRate);
        beingShooting = false;
    }

    [Serializable]
    public struct Arsenal
    {
        public string name;
        public GameObject weapon;
        public RuntimeAnimatorController controller;
    }
}
