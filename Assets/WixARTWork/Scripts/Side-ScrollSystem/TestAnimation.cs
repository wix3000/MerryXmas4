using UnityEngine;
using System.Collections;

public class TestAnimation : MonoBehaviour {

    new Animation animation;
    SkeletonAnimation sa;

	// Use this for initialization
	void Start () {
        animation = GetComponent<Animation>();
        sa = GetComponent<SkeletonAnimation>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A)) {
            int i = 0;
            foreach(AnimationClip ac in animation) {
                if(i == 0) {
                    animation.clip = ac;
                    break;
                }
                i++;
            }
            sa.skeleton.SetToSetupPose();
            animation.Play();
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            int i = 0;
            foreach (AnimationClip ac in animation) {
                if (i == 1) {
                    animation.clip = ac;
                    break;
                }
                i++;
            }
            sa.skeleton.SetToSetupPose();
            animation.Play();
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            int i = 0;
            foreach (AnimationClip ac in animation) {
                if (i == 2) {
                    animation.clip = ac;
                    break;
                }
                i++;
            }
            sa.skeleton.SetToSetupPose();
            animation.Play();
        }
    }
}
