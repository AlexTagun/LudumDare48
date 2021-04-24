using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem {
    Sprite Icon { get; }
}

public class Item : MonoBehaviour, IItem {
    public Sprite Icon { get; }
}

public class Shield : IItem {
    public Sprite Icon { get; }
}