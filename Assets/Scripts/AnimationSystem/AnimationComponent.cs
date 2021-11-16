using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

public class AnimationComponent : MonoBehaviour
{
    //在检查器窗口赋值
    public AnimancerComponent animancerComponent;
    public List<AnimationClip> saveClips;

    public StackFsmState stackFsmState;

    public Dictionary<string, AnimationClip> clips
        = new Dictionary<string, AnimationClip>();

    public Dictionary<string, string> runtimeClips
        = new Dictionary<string, string>()
        {
            {HeroFsmStateTypes.Idle.GetMappedString(),"Anim_Idle"},
            {HeroFsmStateTypes.Run.GetMappedString(),"Anim_Run" },
            {HeroFsmStateTypes.Attack.GetMappedString(),"Anim_Attack" },
        };
    private void Awake()
    {
        clips.Add("Anim_Idle", saveClips[0]);
        clips.Add("Anim_Run", saveClips[1]);

    }
    public AnimancerState PlayAnim(HeroFsmStateTypes types,float speed = 1.0f,float fadeDuration = 0.3f)
    {
        return PlayAnim(types.GetMappedString(), speed, fadeDuration);
    }
    public AnimancerState PlayAnim(string types,float speed = 1.0f,float fadeDuration = 0.3f)
    {
        AnimancerState animancerState = animancerComponent.CrossFade(
            this.clips[runtimeClips[types]], fadeDuration);
        animancerState.Speed = speed;
        return animancerState;
    }
    public AnimancerState PlayAnimFromStart(HeroFsmStateTypes types,float speed = 1.0f,float fadeDuration = 1.0f)
    {
        AnimancerState animancerState =
            animancerComponent.CrossFadeFromStart(
                this.clips[runtimeClips[types.GetMappedString()]], fadeDuration);
        animancerState.Speed = speed;
        return animancerState;
    }
    public void PlayAnimAndReturnIdleFromStart(HeroFsmStateTypes types,float speed = 1.0f,float fadeDuration = 1.0f)
    {
        PlayAnimFromStart(types, speed, fadeDuration).OnEnd = PlayIdleFromStart;
    }
    public void PlayIdleFromStart()
    {
        animancerComponent.CrossFadeFromStart(this.clips[runtimeClips[HeroFsmStateTypes.Idle.GetMappedString()]]);
    }
    public void PlayerAnimByStack(float speed = 1.0f,float fadeDuration = 0.3f)
    {
        LinkedListNode<HeroFsmStateBase> currentStateNode = this.stackFsmState.GetCurrentState();
        if(currentStateNode == null)
        {
            PlayIdleFromStart();
            return;
        }
        HeroFsmStateBase currentState = currentStateNode.Value;

        if (this.runtimeClips.ContainsKey(currentState.stateTypes.GetMappedString()))
        {
            PlayAnim(currentState.stateTypes, speed, fadeDuration);
        }else if(this.runtimeClips.ContainsKey(currentState.stateName))
        {
            PlayAnim(currentState.stateName, speed, fadeDuration);
        }else
        {
            PlayIdleFromStart();
        }
    }
    public void ClearReference()
    {
        this.animancerComponent = null;
        this.clips.Clear();
        this.clips = null;
        this.runtimeClips.Clear();
        this.runtimeClips = null;
        this.stackFsmState = null;
    }
}
