using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] bool startingMomentum = true;
    [SerializeField] float maxInitialAngularVelocity;
    [SerializeField] float maxInitialLinearVelocity;

    // Start is called before the first frame update
    IEnumerator Start()
    {

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
            GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-maxInitialAngularVelocity, maxInitialAngularVelocity), Random.Range(-maxInitialAngularVelocity, maxInitialAngularVelocity), Random.Range(-maxInitialAngularVelocity, maxInitialAngularVelocity));
            GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-maxInitialLinearVelocity, maxInitialLinearVelocity), Random.Range(-maxInitialLinearVelocity, maxInitialLinearVelocity), Random.Range(-maxInitialLinearVelocity, maxInitialLinearVelocity));
        }
    }

    // Update is called once per frame
    void Update()
    {

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
            GetComponent<Rigidbody>().isKinematic = false;
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
