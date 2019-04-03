using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : Bolt.EntityEventListener<IProjectileState>
{
    public PlayerController sourcePlayer;

    float maxFrames = 50;

    float smoothFrames;

    // int frame;
    bool init = false;

    ProjectileFakeController projectileFakeController;

    public override void Attached()
    {
        state.SetTransforms(state.transform, transform);

    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        PlayerController PC = other.GetComponent<PlayerController>();
        if (PC != sourcePlayer && PC != null)
        {
            Debug.Log("HIT");

            if (projectileFakeController)
            {
                Destroy(projectileFakeController.graphic);
                Destroy(projectileFakeController);
            }

            BoltNetwork.Destroy(gameObject);

        }
    }

    public override void Detached()
    {
        if (projectileFakeController)
        {
            Destroy(projectileFakeController.graphic);
            Destroy(projectileFakeController);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("wew2");
    }


    // Use this for initialization
    void Start()
    {
        if (BoltNetwork.isServer)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }

        smoothFrames = maxFrames;
    }

    // Update is called once per frame
    void Update()
    {
        if (projectileFakeController != null)
        {
            if (smoothFrames > 0)
                smoothFrames--;
            projectileFakeController.graphic.transform.position = Vector3.Lerp(transform.position, projectileFakeController.transform.position, (smoothFrames / maxFrames));
            projectileFakeController.graphic.transform.rotation = Quaternion.Lerp(transform.rotation, projectileFakeController.transform.rotation, (smoothFrames / maxFrames));
        }
    }

    private void FixedUpdate()
    {
        if (BoltNetwork.isServer)
            transform.Translate(Vector3.forward * Time.fixedDeltaTime * 10f);
        else
        {
            if (init == false)
            {
                if (state.frame != 0)
                {
                    var ArrayPFC = GameObject.FindObjectsOfType<ProjectileFakeController>();

                    foreach (ProjectileFakeController PFC in ArrayPFC)
                    {
                        if (PFC.frame == state.frame)
                        {
                            init = true;

                            projectileFakeController = PFC;
                            projectileFakeController.found = true;
                            break;
                        }
                    }

                }

            }
        }
    }
}
