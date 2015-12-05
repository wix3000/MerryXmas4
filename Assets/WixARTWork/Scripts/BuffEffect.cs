using System;
using System.Collections;
using UnityEngine;

namespace SSG {
    /// <summary>
    /// Buff系統，
    /// 使用裝飾模式完成BUFF的設定後，再利用ADDIN方法添加進MODLE的BUFF清單裡
    /// </summary>
    public class BuffEffect {

        public string name;
        public float timer = 0f;
        public float duration;

        Action<BuffEffect> onEnter;
        Action<BuffEffect> onEffect;
        Action<BuffEffect> onExit;

        SSGPlayer_Model model;

        public float arg1, arg2;

        string effectName;
        string adhereBone;

        public GameObject particalEffect;

        public float NomralizatTime { get { return timer / duration; } }

        public BuffEffect(string name, float duration) {
            this.name = name;
            this.duration = duration;
        }

        /// <summary>
        /// 設定進入事件
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public virtual BuffEffect SetEnter(Action<BuffEffect> action, bool overRide = true) {
            if (onEnter != null && !overRide) {
                onEnter += action;
            }
            else {
                onEnter = action;
            }
            return this;
        }

        /// <summary>
        /// 設定持續事件
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public virtual BuffEffect SetEffect(Action<BuffEffect> action, bool overRide = true) {
            if (onEffect != null && !overRide) {
                onEffect += action;
            }
            else {
                onEffect = action;
            }
            return this;
        }

        /// <summary>
        /// 設定離開事件
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public virtual BuffEffect SetExit(Action<BuffEffect> action, bool overRide = true) {
            if (onExit != null && !overRide) {
                onExit += action;
            }
            else {
                onExit = action;
            }
            return this;
        }

        /// <summary>
        /// 設定粒子特效的方法
        /// </summary>
        /// <param name="name">檔案名稱</param>
        /// <param name="adhereBone">附著骨骼</param>
        /// <returns></returns>
        public virtual BuffEffect SetParticleEffect(string name, string adhereBone) {
            effectName = name;
            this.adhereBone = adhereBone;
            return this;
        }

        public virtual void Addin(SSGPlayer_Model model) {
            this.model = model;
            for (int i = 0; i < model.buffs.Count; i++) {
                if(model.buffs[i].name == name) {
                    OverWrite(model, i);
                    return;
                }
            }
            model.buffs.Add(this);
            if (onEnter != null) onEnter(this);
            CreateEffect(model);
        }

        void CreateEffect(SSGPlayer_Model model) {
            if (effectName == "") return;   // 沒設定名稱就跳出
            GameObject effect = Resources.Load<GameObject>("ParticleEffects/" + effectName);
            if (!effect) return;            // 找不到檔案也跳出

            //---main process
            particalEffect = UnityEngine.Object.Instantiate(effect);
            particalEffect.transform.SetParent(model.skeletonAnimator.transform);
            BoneFollower bf = particalEffect.GetComponent<BoneFollower>();
            if (!bf) bf = particalEffect.AddComponent<BoneFollower>();
            bf.SkeletonRenderer = model.skeletonAnimator;
            bf.boneName = adhereBone;
            bf.followBoneRotation = false;
        }

        void OverWrite(SSGPlayer_Model model, int i) {
            BuffEffect oldBuff = model.buffs[i];
            model.buffs[i] = this;
            if (effectName != oldBuff.effectName) {
                UnityEngine.Object.Destroy(oldBuff.particalEffect);
                CreateEffect(model);
            }
            else {
                particalEffect = oldBuff.particalEffect;
            } 
        }

        public virtual void OnEffect() {
            if (timer < duration) {
                if (onEffect != null) onEffect(this);
                timer += Time.deltaTime;
                return;
            }
            OnExit();
        }

        public virtual void OnExit() {
            if (onExit != null) onExit(this);
            if (particalEffect) UnityEngine.Object.Destroy(particalEffect);
            model.buffs.Remove(this);
        }
    }
}