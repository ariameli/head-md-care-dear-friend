using UnityEngine;
using Yarn.Unity;

public class SylviaActionsNew : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    [YarnCommand("SylviaSittingIdle")]
    public void SittingIdle()
    {
        //animator.SetBool("IsSitting", true);
        animator.Play("SittingIdle", 0, 0f);
    }


    [YarnCommand("SylviaStandingIdle")]
    public void StandingIdle()
    {
        //animator.SetBool("IsSitting", false);
        animator.Play("Idle", 0, 0f);
    }
}