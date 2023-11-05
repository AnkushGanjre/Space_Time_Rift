using UnityEngine;
using UnityEngine.AI;

public class NPCControllerScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    public GameObject PATH;
    private Transform[] PathPoints;

    public float minDistance = 1;
    public int index = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        PathPoints = new Transform[PATH.transform.childCount];
        for (int i = 0; i < PathPoints.Length; i++)
        {
            PathPoints[i] = PATH.transform.GetChild(i);
        }
    }

    private void Update()
    {
        Roam();
    }

    private void Roam()
    {
        
        if (Vector3.Distance(transform.position , PathPoints[index].position) < minDistance)
        {
            if (index >= 0 && index < PathPoints.Length)
            {
                index += 1;
            }
            else
            {
                index = 0;
            }
        }

        agent.SetDestination(PathPoints[index].position);
        animator.SetFloat("vertical", !agent.isStopped ? 1 : 0);
    }
}
