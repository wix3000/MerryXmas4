using UnityEngine;
using System.Collections;

public class girlGM : MonoBehaviour {
	public float speed;
	SkeletonAnimation m_anim;

	bool b;

	// Use this for initialization
	void Start () {
		m_anim = GetComponent<SkeletonAnimation> ();
		m_anim.state.Start += delegate {
			b = true;
			print("start");
		};
		m_anim.state.End += delegate {
			b = false;
			print("End");
		};
		m_anim.state.Complete += delegate {
			print("Complete");
		};
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetAxis("Horizontal") != 0f) {
			float y = (Input.GetAxis("Horizontal") > 0f) ? 180f : 0f;
			Vector3 v = transform.eulerAngles;
			v.y = y;
			transform.eulerAngles = v;
			
			transform.Translate(new Vector3(-speed,0f) * Mathf.Abs(Input.GetAxis("Horizontal")) * Time.deltaTime);

			if (!b) SetAnimation();
		}

	}

	void SetAnimation(){
		StopAllCoroutines();
		m_anim.state.ClearTrack(0);
		m_anim.skeleton.SetToSetupPose();
		var getData = m_anim.state.SetAnimation(0, "go", true);
		StartCoroutine(WaitToIdle(getData.endTime));
	}

	IEnumerator WaitToIdle (float wait)
	{
		yield return new WaitForSeconds(wait+0.05f);
		m_anim.skeleton.SetToSetupPose();
		m_anim.state.SetAnimation(0, "dio", true);
	}
}
