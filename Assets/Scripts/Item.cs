using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem {
    Sprite Icon { get; }
    void Instantiate(Transform transform);
}

public class Shield : IItem {
    public Sprite Icon { get; }
    
    private GameObject _shield;
    
    public Shield() {
        Icon = Resources.Load<Sprite>("shield");
    }
    
    public void Instantiate(Transform transform) {
        if(_shield != null) GameObject.Destroy(_shield);

        var prefab = Resources.Load<GameObject>("shield");
        var wrist = transform.Find("NPC_walk/Root_M/Pelvis_M/PelvisPart1_M/PelvisPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M");
        _shield = GameObject.Instantiate(prefab, wrist);
    }
}

public class Torch : IItem {
    public Sprite Icon { get; }

    private GameObject _torch;

    public Torch() {
        Icon = Resources.Load<Sprite>("torch");
    }
    
    public void Instantiate(Transform transform) {
        if(_torch != null) GameObject.Destroy(_torch);

        var prefab = Resources.Load<GameObject>("torch");
        var wrist = transform.Find("NPC_walk/Root_M/Pelvis_M/PelvisPart1_M/PelvisPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/Elbow_L/ElbowPart1_L/ElbowPart2_L/Wrist_L");
        _torch = GameObject.Instantiate(prefab, wrist);
    }
}