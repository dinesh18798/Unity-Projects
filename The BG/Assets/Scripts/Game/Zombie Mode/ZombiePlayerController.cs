using System.Collections;
using UnityEngine;

public class ZombiePlayerController : MonoBehaviour
{ 
    public Camera fpsCam;
    public AudioSource fireSound;
    public AudioSource reloadSound;
    public Animator weaponAnimator;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public float impactForce = 30f;
    public int maxAmmo = 10;

    private float damage = 1f;
    private float range = 100f;
    private float fireRate = 10f; 
    private int currentAmmo;
    private float reloadTime = 1f;

    private float nextFire = 0f;
    private bool isReloading = false;
    private ZombieGameController gameController;

    private void Start()
    {
        currentAmmo = maxAmmo;

        GameObject gameControllerObject = GameObject.FindWithTag("ZombieGameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<ZombieGameController>();
            gameController.UpdateAmmo(currentAmmo);
        }
        
    }

    void Update()
    {
        if (ApplicationUtil.GamePaused) return;
        if (gameController.gameOverFlag) return;

        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextFire)
        {
            nextFire = Time.time + 1f / fireRate;
            Fire();
        }
    }

    private IEnumerator Reload()
    {
        reloadSound.Play();
        isReloading = true;
        weaponAnimator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - 0.25f);
        weaponAnimator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);

        currentAmmo = maxAmmo;
        gameController.UpdateAmmo(currentAmmo);
        reloadSound.Stop();
        isReloading = false;
    }

    private void Fire()
    {
        muzzleFlash.Play();
        fireSound.Play();
        currentAmmo--;
        gameController.UpdateAmmo(currentAmmo);

        RaycastHit raycastHitInfo;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out raycastHitInfo, range))
        {           
            ZombieHealth enemyHealth = raycastHitInfo.transform.gameObject.GetComponent<ZombieHealth>();

            if (enemyHealth != null && enemyHealth.isAlive) enemyHealth.TakeDamage(damage);

            GameObject impactObject = Instantiate(impactEffect, raycastHitInfo.point, Quaternion.LookRotation(raycastHitInfo.normal));
            Destroy(impactObject, 2f);
        }
    }
}
