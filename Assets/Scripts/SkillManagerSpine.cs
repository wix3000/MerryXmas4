using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SkillManagerSpine : MonoBehaviour {

    [Tooltip("技能數量")]
    public DataSpine[] m_data;
    [Tooltip("按鍵間隔時間")]
    public float interval = 0.5f;
    [Tooltip("施放技能後的動作"), SpineAnimation]
    public string endAnimation;

    List<KeyCode> keys = new List<KeyCode>();
    List<KeyCode> checkKeys = new List<KeyCode>();

    KeyCode getKey;
    int frame = -1;
    float timer;
    bool isShow;
    SkeletonAnimation m_anim;

    void Start ()
    {
        m_anim = GetComponent<SkeletonAnimation>();
    }
	void Update ()
    {
        HandleSkill();
    }

    void HandleSkill ()
    {
        if (Input.anyKeyDown && timer <= interval)
        {
            if (isShow)
            {
                isShow = false;
            }
            frame++;
            timer = 0f;
            getKey = GetPressKey();
            keys.Add(getKey);
            CheckKey(frame);
        }
        if (frame > -1)
        {
            timer += Time.deltaTime;

            if (timer > interval)
            {
                DelayClear();
            }
        }
    }

    void CheckKey (int getFrame)
    {
        for (int i = 0; i < m_data.Length; i++)
        {
            if (getFrame < m_data[i].keyName.Count)
            {
                if (getKey == m_data[i].keyName[getFrame])
                {
                    if (getFrame + 1 == m_data[i].keyName.Count)
                    {
                        for (int j = 0; j < m_data[i].keyName.Count; j++)
                        {
                            if (keys[j] != m_data[i].keyName[j])
                            {
                                break;
                            }
                            else if (j+1 == m_data[i].keyName.Count)
                            {
                                ShowSkill(i);
                            }
                        }
                    }
                }
            }
        }

        checkKeys.Clear();
        for (int i = 0; i < m_data.Length; i++)
        {
            if (getFrame < m_data[i].keyName.Count)
            {
                for (int j = getFrame; j >= 0; j--)
                {
                    if (keys[getFrame - j] == m_data[i].keyName[getFrame - j])
                    {
                        checkKeys.Add(m_data[i].keyName[getFrame]);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        if (!checkKeys.Contains(getKey))
        {
            DelayClear();
        }
    }

    void ShowSkill (int getIndex)
    {
        StopAllCoroutines();
        m_anim.state.ClearTrack(0);
        m_anim.skeleton.SetToSetupPose();
        var getData = m_anim.state.SetAnimation(0, m_data[getIndex].skillName, false);
        StartCoroutine(WaitToIdle(getData.endTime));
        if (!m_data[getIndex].isCombo)
        {
            isShow = true;
            Invoke("DelayClear", 0.01f);
        }
    }

    void DelayClear ()
    {
        frame = -1;
        keys.Clear();
        checkKeys.Clear();
        timer = 0f;
    }

    IEnumerator WaitToIdle (float wait)
    {
        yield return new WaitForSeconds(wait+0.05f);
        m_anim.skeleton.SetToSetupPose();
        m_anim.state.SetAnimation(0, endAnimation, true);
    }

    KeyCode GetPressKey ()
    {
        int length = System.Enum.GetNames(typeof(KeyCode)).Length;

        for (int i = 0; i < length; i++)
        {
            if (Input.GetKey((KeyCode)i))
            {
                return (KeyCode)i;
            }
        }

        return KeyCode.None;
    }
}
