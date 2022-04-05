using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public ColorType colorType;
    public bool isStacked;
    private ParticleSystem particle;

    private void OnCollisionEnter(Collision collision)
    {
        if (isStacked && collision.gameObject.CompareTag("CollectableCube"))
        {
            var cube = collision.gameObject.GetComponent<Cube>();
            if (cube.colorType == colorType)
            {
                Player.Instance.AddCube(cube);
            }
            else
            {
                collision.gameObject.tag = "Untagged";
                Player.Instance.RemoveCube();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            Player.Instance.ChangeColorType(other.gameObject);
            particle = other.GetComponent<Gate>().particle;
            particle.Play();
        }
        else if (other.CompareTag("Finish") && !Player.Instance.end)
        {
            Player.Instance.GameEnding(true);
        }
    }
}
