using UnityEngine;

public class RandomStartVelocity : MonoBehaviour
{
    public bool useForwardFromOther;
    public string other;
    public float forwardIntensity = 1;
    public Vector3 spawnVelocity = Vector3.zero;
    public float spawnTorque2D = 0;
    public Vector3 spawnTorque3D = Vector3.zero;
    public bool randomVelocity = false;
    public bool randomTorque = false;

    public void Start()
    {
        if (this.transform.GetComponent<Rigidbody2D>())
        {
            if (randomVelocity)
            {
                this.transform.GetComponent<Rigidbody2D>().AddForce(
                    new Vector2(
                        Random.Range(-spawnVelocity.x, spawnVelocity.x),
                        Random.Range(-spawnVelocity.y, spawnVelocity.y)
                    )
                );
            }
            else
            {
                this.transform.GetComponent<Rigidbody2D>().AddForce(spawnVelocity);
            }

            if (randomTorque)
            {
                this.transform.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-spawnTorque2D, spawnTorque2D));
            }
            else
            {
                this.transform.GetComponent<Rigidbody2D>().AddTorque(spawnTorque2D);
            }
        }

        if (this.transform.GetComponent<Rigidbody>())
        {
            if (randomVelocity)
            {
                this.transform.GetComponent<Rigidbody>().AddForce(
                    new Vector3(
                        Random.Range(-spawnVelocity.x, spawnVelocity.x),
                        Random.Range(-spawnVelocity.y, spawnVelocity.y),
                        Random.Range(-spawnVelocity.z, spawnVelocity.z)
                    )
                );
            }
            else
            {
                this.transform.GetComponent<Rigidbody>().AddForce(spawnVelocity);
            }

            if (randomTorque)
            {
                this.transform.GetComponent<Rigidbody>().AddTorque(
                    new Vector3(
                        Random.Range(-spawnTorque3D.x, spawnTorque3D.x),
                        Random.Range(-spawnTorque3D.y, spawnTorque3D.y),
                        Random.Range(-spawnTorque3D.z, spawnTorque3D.z)
                    )
                );
            }
            else
            {
                this.transform.GetComponent<Rigidbody>().AddTorque(spawnTorque3D);
            }

            if (useForwardFromOther)
            {
                GameObject otherGameObject = GameObject.FindGameObjectWithTag(other);

                if (otherGameObject)
                {
                    this.transform.GetComponent<Rigidbody>().AddForce(
                        otherGameObject.transform.forward * forwardIntensity,
                        ForceMode.Impulse
                    );
                }
            }
        }
    }
}