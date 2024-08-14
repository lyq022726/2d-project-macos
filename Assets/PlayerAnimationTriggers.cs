using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    // Start is called before the first frame update
    private Player player => GetComponentInParent<Player>();
    
    private void AnimationTriggers(){
        player.AnimationTrigger();
    }
}
