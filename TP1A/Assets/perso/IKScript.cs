using UnityEngine;
using System;
using System.Collections;
// Pour IK Unity
// https://www.youtube.com/watch?v=EggUxC5_lGE
[RequireComponent(typeof(Animator))]
public class IKScript : MonoBehaviour
{

    protected Animator animator;

    public bool ikActive = false;

    Vector3 lfPos, rfPos;
    Quaternion lfRot, rfRot;

    // public GameObject leftf,rightf;
    Transform left_foot, right_foot;

    float lf_param, rf_param;

    public Vector3 offsetY = new Vector3(0f, 0f, 0f);
    public float raycastDepth = 1f;

    // http://gyanendushekhar.com/2017/06/29/understanding-layer-mask-unity-5-tutorial/
    int LayerMaskCharacters;

    public GameObject target= null;
    void Start()
    {
        animator = GetComponent<Animator>();
        left_foot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        right_foot = animator.GetBoneTransform(HumanBodyBones.RightFoot);

        lfRot = left_foot.rotation;
        rfRot = right_foot.rotation;

        // merci virgile
        // DisableRagdoll();

        LayerMaskCharacters = 1 << LayerMask.NameToLayer("Characters");
        LayerMaskCharacters = ~LayerMaskCharacters;

    }
    void Update()
    {
        RaycastHit leftHit, rightHit;

        Vector3 lpos = left_foot.TransformPoint(Vector3.zero);
        Vector3 rpos = right_foot.TransformPoint(Vector3.zero);

        if (Physics.Raycast(lpos, -Vector3.up, out leftHit, raycastDepth, LayerMaskCharacters))
        {

            Debug.Log("HIT");

            if (leftHit.collider != null)
            {
                Debug.Log(leftHit.collider.name);

                lfPos = leftHit.point;
                lfRot = Quaternion.FromToRotation(transform.up, leftHit.normal) * transform.rotation;
            }


        }

        if (Physics.Raycast(rpos, -Vector3.up, out rightHit, raycastDepth, LayerMaskCharacters))
        {
            // Debug.Log("HIT");

            if (rightHit.collider != null)
            {
                Debug.Log(rightHit.collider.name);

                rfPos = rightHit.point;
                rfRot = Quaternion.FromToRotation(transform.up, rightHit.normal) * transform.rotation;
            }
        }
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {

        if (animator)
        {
            lf_param = animator.GetFloat("lf_param");
            rf_param = animator.GetFloat("rf_param");

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {

                // Set the look target position, if one has been assigned
                // if (lookObj != null)
                // {
                //     animator.SetLookAtWeight(1);
                //     animator.SetLookAtPosition(lookObj.position);
                // }

                // Set the right hand target position and rotation, if one has been assigned

                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, lf_param);
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rf_param);

                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, lf_param);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rf_param);

                animator.SetIKPosition(AvatarIKGoal.LeftFoot, lfPos + offsetY);
                animator.SetIKPosition(AvatarIKGoal.RightFoot, rfPos + offsetY);

                animator.SetIKRotation(AvatarIKGoal.LeftFoot, lfRot);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, rfRot);


                // animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                // animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
                // animator.SetIKPosition(AvatarIKGoal.LeftFoot, target.transform.position);
                // animator.SetIKRotation(AvatarIKGoal.LeftFoot, target.transform.rotation);
            }
        }
    }
    private void DisableRagdoll()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.detectCollisions = false;
        }
    }
}