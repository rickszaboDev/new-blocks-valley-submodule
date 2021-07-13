using UnityEngine;

public class CustomAnimation
{

    private static AnimationClip setCustomKeyFramesToClip(AnimationClip clip, float pos1, float pos2, float pos3, string coords)
    {
        Keyframe[] keys;
        keys = new Keyframe[3];
        keys[0] = new Keyframe(0.0f, pos1);
        keys[1] = new Keyframe(0.3f, pos2);
        keys[2] = new Keyframe(0.6f, pos3);
        var curve = new AnimationCurve(keys);
        AnimationClip _clip = clip;
        _clip.SetCurve("", typeof(Transform), coords, curve);
        return _clip;
    }

    public static AnimationClip setAnimationTo(GameObject objectToAnim, Vector3 myNextPos, Direction upOrDown)
    {
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;

        var oldPos = objectToAnim.transform.position;
        if (upOrDown == Direction.Up)
        {
            clip = setCustomKeyFramesToClip(clip, oldPos.x, oldPos.x, myNextPos.x, "localPosition.x");
            clip = setCustomKeyFramesToClip(clip, oldPos.z, oldPos.z, myNextPos.z, "localPosition.z");
            clip = setCustomKeyFramesToClip(clip, oldPos.y, myNextPos.y, myNextPos.y, "localPosition.y");
        }
        else
        {
            clip = setCustomKeyFramesToClip(clip, oldPos.x, myNextPos.x, myNextPos.x, "localPosition.x");
            clip = setCustomKeyFramesToClip(clip, oldPos.z, myNextPos.z, myNextPos.z, "localPosition.z");
            clip = setCustomKeyFramesToClip(clip, oldPos.y, oldPos.y, myNextPos.y, "localPosition.y");
        }

        AnimationEvent animEv = new AnimationEvent();
        animEv.time = 2.0f;
        animEv.functionName = "OnAnimationEnded";
        clip.events = new AnimationEvent[] { animEv };

        return clip;
    }
}
