﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    private RectTransform crosshairPosition;
    private Camera cam;

    [SerializeField]
    private float rateOfFire;

    [SerializeField]
    private float gunCooldown;

    [SerializeField]
    private float damage;

    Animator anim;

    [SerializeField]
    private GameObject gunEffect;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        crosshairPosition = GetComponent<RectTransform>();
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCursorPosition();
        if(Input.GetKeyDown(KeyCode.Mouse0)) { Shoot(); }
        if(gunCooldown > 0.0f) { gunCooldown -= Time.deltaTime; }
    }

    void UpdateCursorPosition()
    {
        crosshairPosition.position = Input.mousePosition;
    }

    void Shoot()
    {
        //Firing Logic
        int layerMask = LayerMask.GetMask("Enemies");
        if (gunCooldown <= 0.0f)
        {
            //Animation
            anim.SetBool("GunFiring", true);
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f, layerMask))
            {
                Transform objectHit = hit.transform;

                Instantiate(gunEffect, hit.point, Quaternion.identity);

                Health h = objectHit.GetComponent<Health>();

                if(h != null)
                {
                    h.TakeDamage(damage);     
                }
            }
            else { Debug.Log("MISS"); }

            gunCooldown = rateOfFire;
        }
    }
}
