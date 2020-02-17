using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PowerCore : MonoBehaviour
{

    private ConnectPoint hoverPoint;
    private ConnectPoint connectedPoint;

    public bool powered;

    public Material unpoweredMaterial;
    public Material poweredMaterial;

    public GameObject sparkPrefab;
    public GameObject smokePrefab;

    private Light pointLight;
    private Rigidbody rb;

    [SerializeField] bool startingMomentum = true;
    [SerializeField] float maxInitialAngularVelocity;
    [SerializeField] float maxInitialLinearVelocity;
    [SerializeField] bool alwaysKinematic = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        rb = GetComponent<Rigidbody>();
        pointLight = transform.Find("Point Light").gameObject.GetComponent<Light>();
        if (powered)
        {
            PowerUp();
        }
        else
        {
            PowerDown();
            BurnOut(false);
        }
        yield return null;
        if (startingMomentum && connectedPoint == null)
        {
            rb.angularVelocity = new Vector3(Random.Range(-maxInitialAngularVelocity, maxInitialAngularVelocity), Random.Range(-maxInitialAngularVelocity, maxInitialAngularVelocity), Random.Range(-maxInitialAngularVelocity, maxInitialAngularVelocity));
            rb.velocity = new Vector3(Random.Range(-maxInitialLinearVelocity, maxInitialLinearVelocity), Random.Range(-maxInitialLinearVelocity, maxInitialLinearVelocity), Random.Range(-maxInitialLinearVelocity, maxInitialLinearVelocity));
        }
    }

    public void SetPointHovering(ConnectPoint point)
    {
        hoverPoint = point;
    }

    public void ConnectToPoint(ConnectPoint point)
    {

        connectedPoint = point;
        hoverPoint = null;

        connectedPoint.ConnectCore(this);

    }

    public void OnDetachedFromHand()
    {
        Debug.Log("Detached from hand");
        if (hoverPoint != null)
        {
            ConnectToPoint(hoverPoint);
        }
        else
        {
            if (alwaysKinematic)
            {
                rb.isKinematic = true;                
            }
            else
            {
                rb.isKinematic = false;
            }
        }
    }

    public void OnAttachedToHand()
    {
        if (connectedPoint != null)
        {
            Debug.Log($"Attaching {name} to hand, disconnecting from {connectedPoint.name}");
            connectedPoint.DisconnectCore();
            connectedPoint = null;
        }
    }

    public void BurnOut(bool withSpark = true)
    {
        PowerDown();
        Transform sparkTransform = transform.Find("SparkPoint");
        if (withSpark)
        {
            GetComponent<AudioSource>().Play();
            Instantiate(sparkPrefab, sparkTransform.position, sparkTransform.rotation, transform);
        }
        Instantiate(smokePrefab, sparkTransform.position, sparkTransform.rotation, transform);
        // particle systems and sounds
    }

    public void PowerDown()
    {
        powered = false;
        GameObject cylinder = GetComponentInChildren<MeshCollider>().gameObject;
        MeshRenderer meshRenderer = cylinder.GetComponent<MeshRenderer>();

        Material[] mats = meshRenderer.materials;
        mats[1] = unpoweredMaterial;
        meshRenderer.materials = mats;
        pointLight.gameObject.SetActive(false);
    }

    public void PowerUp()
    {
        powered = true;
        GameObject cylinder = GetComponentInChildren<MeshCollider>().gameObject;
        MeshRenderer meshRenderer = cylinder.GetComponent<MeshRenderer>();
        Material[] mats = meshRenderer.materials;
        mats[1] = poweredMaterial;
        meshRenderer.materials = mats;
        pointLight.gameObject.SetActive(true);

    }

    public string GetPoweredPoint()
    {
        if (connectedPoint == null)
        {
            return null;
        }
        return connectedPoint.name;
    }
}
