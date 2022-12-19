using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gun : MonoBehaviour
{
    [Header("Statistique de l'arme")]
    public float damage = 10f;
    public float range = 100f;
    public float gunCooldown = 0.2f;
    public float reloadTime = 1.5f;
    public int maxAmmo = 20;
    private int currentAmmo;
    private bool isReloading;
    private bool readyToShoot;

    [Header("Références")]
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

    /// <summary>
    /// Gère les inputs du joueur
    /// </summary>
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

    /// <summary>
    /// Change l'affichage de l'interface utilisateur pour le nombre de munitions
    /// </summary>
    void UiControler()
    {
        ammoCount.text = currentAmmo.ToString();
    }

    /// <summary>
    /// Sert à faire tirer l'arme
    /// </summary>
    void Shoot()
    {
        readyToShoot = false;
        currentAmmo--;
        //Crée un rayon qui part de la caméra et qui va dans la direction où elle regarde
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

        //Dit au script de ne pas tirer pendant le temps d'attente de l'arme
        Invoke("ResetShoot", gunCooldown);
    }

    /// <summary>
    /// Ajoute une ligne qui part de la caméra et qui va jusqu'au point de contact avec un objet
    /// </summary>
    void AddLine()
    {
        GameObject traileffect = Instantiate(lineRenderer.gameObject, hit.point, Quaternion.LookRotation(hit.normal));

        LineRenderer lr = traileffect.GetComponent<LineRenderer>();

        lr.SetPosition(0, muzzleFlash.transform.position);
        lr.SetPosition(1, hit.point);

        Destroy(traileffect, 0.1f);
    }

    /// <summary>
    /// Ajoute une balle au point de contact
    /// </summary>
    void AddWallHit()
    {
        GameObject wallHit = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(wallHit, 1f);
    }

    /// <summary>
    /// Réinitialise le temps d'attente de l'arme
    /// </summary>
    void ResetShoot()
    {
        readyToShoot = true;
    }

    /// <summary>
    /// Sert à recharger l'arme
    /// </summary>
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
