using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float gunCooldown = 0.2f;
    public float reloadTime = 1.5f;
    public int maxAmmo = 20;
    public int currentAmmo;
    public bool isReloading;
    public bool readyToShoot;

    public Transform orientation;
    public RaycastHit hit;
    public Camera cam;
    public LayerMask enemie;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public LineRenderer lineRenderer;
    public Animator animator;
    public TextMeshProUGUI ammoCount;


    void Start()
    {
        currentAmmo = maxAmmo;
        readyToShoot = true;
    }

    void Update(){
        GetInput();
        UiControler();
    }


    void GetInput()
    {
        if (Input.GetKey(KeyCode.Mouse0) && currentAmmo > 0 && !isReloading)
        {
            if(readyToShoot){
                Shoot(); 
            }

        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    void UiControler()
    {
        ammoCount.text = currentAmmo.ToString();
    }

    void Shoot()
    {
        readyToShoot = false;
        //shoot
        currentAmmo--;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, enemie))
        {
            if(hit.transform.tag == "Enemy"){
                RobotAI enemie = hit.transform.GetComponent<RobotAI>();
                enemie.TakeDamage(1);
            }
        }

        AddWallHit();
        AddLine();

        muzzleFlash.Play();
        animator.SetTrigger("Shoot");

        Invoke("ResetShoot", gunCooldown);
    }

    void AddLine()
    {
        GameObject traileffect = Instantiate(lineRenderer.gameObject, hit.point, Quaternion.LookRotation(hit.normal));

        LineRenderer lr = traileffect.GetComponent<LineRenderer>();

        lr.SetPosition(0, muzzleFlash.transform.position);
        lr.SetPosition(1, hit.point);

        Destroy(traileffect, 0.1f);
    }

    void AddWallHit()
    {
        GameObject wallHit = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(wallHit, 1f);
    }

    void ResetShoot()
    {
        readyToShoot = true;
    }

    IEnumerator Reload()
    {
        isReloading = true;

        if(currentAmmo == 0){
            animator.SetTrigger("FullReload");
        } else {
            animator.SetTrigger("Reload");
        }

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
