using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class OrbitalObject : MonoBehaviour
{
    public static List<OrbitalObjectInfo> orbitalObjects = new List<OrbitalObjectInfo>();

    OrbitalObjectInfo orbitalObjectInfo;
    float orbitalSpeed;

    bool setup = false;

    public void SetupOrbitalObject (PlanetaryObject planetaryObject)
    {
        orbitalObjectInfo = new OrbitalObjectInfo(GetComponent<Rigidbody>(), planetaryObject);
        orbitalObjects.Add(orbitalObjectInfo);

        orbitalSpeed = planetaryObject.orbitalSpeed;

        orbitalObjectInfo.rb.position = Vector3.right * orbitalObjectInfo.planetaryObject.radiusToCentre;

        setup = true;
    }

    void Attract ()
    {
        foreach (OrbitalObjectInfo orbitalObject in orbitalObjects)
        {
            if (orbitalObject != orbitalObjectInfo)
            {
                Vector3 forceDirection = (orbitalObjectInfo.rb.position - orbitalObject.rb.position);
                float r = forceDirection.magnitude;
                if (r == 0)
                    r = Mathf.Epsilon;

                forceDirection = forceDirection.normalized * SolarSystem.G * (orbitalObjectInfo.rb.mass * orbitalObject.rb.mass) / (r * r);
                orbitalObject.rb.AddForce(forceDirection, ForceMode.Force);
            }
        }
    }

    void Move ()
    {
        if (orbitalSpeed > 0)
            orbitalObjectInfo.rb.MovePosition(orbitalObjectInfo.rb.position + Vector3.forward * orbitalSpeed * Time.fixedDeltaTime);
    }

    void FixedUpdate()
    {
        if (setup)
        {
            Attract();
            Move();
        }
    }

    [System.Serializable]
    public class OrbitalObjectInfo
    {
        public Rigidbody rb;
        public PlanetaryObject planetaryObject;

        public OrbitalObjectInfo(Rigidbody rb, PlanetaryObject planetaryObject)
        {
            this.rb = rb;
            this.planetaryObject = planetaryObject;

            this.rb.useGravity = false;
            this.rb.drag = 0f;
            this.rb.angularDrag = 0f;
            this.rb.mass = planetaryObject.mass;
        }
    }
}
