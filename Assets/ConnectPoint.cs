﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectPoint : MonoBehaviour, PowerSource
{

    public GameObject ghostCylinderPrefab;

    public PowerCore connectedCore;

    bool ghostShowing;

    AudioSource audioSource;
    public AudioClip powerUpAudio;
    public AudioClip powerDownAudio;
    Rigidbody coreRb;
   

    // Start is called before the first frame update
    void Start()
    {
        if (connectedCore != null)
        {
            connectedCore.ConnectToPoint(this);
        }
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PowerCore core = other.gameObject.GetComponentInParent<PowerCore>();
        if (core != null)
        {
            // show ghost mode
            Debug.Log("Ghost Mode On");
            ghostCylinderPrefab.SetActive(true);
           core.SetPointHovering(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PowerCore core = other.gameObject.GetComponentInParent<PowerCore>();
        if (core != null)
        {
            Debug.Log("Ghost Mode Off");
            ghostCylinderPrefab.SetActive(false);
            core.SetPointHovering(null);
        }
    }

    public void ConnectCore(PowerCore core)
    {
        connectedCore = core;
        Transform attachpointOffsetTransform = connectedCore.transform.Find("ConnectAttachPoint");

        Debug.Log($"Core {core.name} connected to {name}");

        connectedCore.transform.position = transform.position;
        connectedCore.transform.rotation = transform.rotation;
        connectedCore.transform.Translate(-attachpointOffsetTransform.localPosition,Space.Self);
        connectedCore.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        connectedCore.GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        ghostCylinderPrefab.SetActive(false);

        if (audioSource == null)
        {
            Debug.LogWarning($"No audio source on Connect Point {name}");
        }

        if (core.powered && audioSource != null)
        {
            audioSource.PlayOneShot(powerUpAudio);
        }
    }

    public void DisconnectCore()
    {
        connectedCore.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;


        GetComponent<CapsuleCollider>().enabled = true;
        Debug.Log($"Core {connectedCore.name} disconnected from {name}");

        if (audioSource == null)
        {
            Debug.LogWarning($"No audio source on Connect Point {name}");
        }
        if (connectedCore.powered && audioSource != null)
        {
            audioSource.PlayOneShot(powerDownAudio);
        }
        connectedCore = null;
    }

    public bool IsPowered()
    {
        if (connectedCore == null)
        {
            return false;
        }
        return connectedCore.powered;
    }
}

public interface PowerSource
{
    bool IsPowered();
}