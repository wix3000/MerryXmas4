using UnityEngine;
using System.Collections;

public class SetToSetupPos : MonoBehaviour {

    public Animator anim;
    public SkeletonAnimator SA;
    AnimatorStateInfo ASI;

    int lastHash;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        ASI = anim.GetCurrentAnimatorStateInfo(0);

        if (ASI.fullPathHash != lastHash) {
            SA.skeleton.SetToSetupPose();
            lastHash = ASI.fullPathHash;
        }
	}
}
