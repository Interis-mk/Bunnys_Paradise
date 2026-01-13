using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    public void PlayMusic1()
    {
        SoundManager.instance.SwitchMusic("creepy-buildup");
        
    }public void PlayMusic2()
    {
        SoundManager.instance.SwitchMusic("creepy-even");
    }public void PlaySfx1()
    {
        SoundManager.instance.PlaySfx("blip");
    }public void PlaySfx2()
    {
        SoundManager.instance.PlaySfx("door-open");
    }
}
