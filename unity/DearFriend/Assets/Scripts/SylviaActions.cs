using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class AnnaActions : MonoBehaviour
{
    public Transform position1;
    public Transform position2;
    public Transform position3;
    public Transform position4;

    private NavMeshAgent agent;
    private Animator animator;
    private Coroutine moveRoutine;
    private bool sitAfterArrival;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    [YarnCommand("AnnaMoves")]
    public void Moves(string positionName)
    {
        Transform target = positionName switch
        {
            "Kitchen" => position1,
            "Window" => position2,
            "Couch" => position3,
            "Computer" => position4,
            _ => null
        };

        if (target != null)
        {
            if (moveRoutine != null)
                StopCoroutine(moveRoutine);

            sitAfterArrival = false;
            moveRoutine = StartCoroutine(MoveToPosition(target));
        }
    }

    private IEnumerator MoveToPosition(Transform targetPosition)
    {
        if (agent == null || targetPosition == null)
            yield break;

        animator.SetBool("IsSitting", false);
        animator.SetBool("IsWalking", true);
        agent.isStopped = false;
        agent.SetDestination(targetPosition.position);

        //rotate to face the target position while moving
        transform.rotation = targetPosition.rotation;
    
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            yield return null;

        agent.ResetPath();
        transform.rotation = targetPosition.rotation;
        animator.SetBool("IsWalking", false);

        
        if (sitAfterArrival && targetPosition == position3)
        {
            sitAfterArrival = false;
            yield return SnapAndSit();
        }



        moveRoutine = null;
    }

    [YarnCommand("AnnaSits")]
    public void Sits()
    {
    
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }

        sitAfterArrival = true;
        moveRoutine = StartCoroutine(MoveToPosition(position3));
    }

    private IEnumerator SnapAndSit()
    {
        if (agent == null || position3 == null)
            yield break;

        agent.isStopped = true;
        agent.ResetPath();

        //transform.position = position3.position;
        transform.rotation = position3.rotation;

        animator.SetBool("IsWalking", false);
        animator.SetBool("IsSitting", true);
        animator.SetTrigger("Sits");

        yield return null;
    }
}