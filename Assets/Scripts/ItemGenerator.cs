using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private List<DropInfo> _dropInfos;
    
    public DropInfo GetDropInfo()
    {
        var allWeight = 0;

        foreach (var dropInfo in _dropInfos)
        {
            allWeight += dropInfo.Weight;
        }

        var randWeight = Random.Range(1, allWeight + 1);

        var controlWeight = 0;

        foreach (var dropInfo in _dropInfos)
        {
            var leftEdge = controlWeight;
            controlWeight += dropInfo.Weight;
            var rightEdge = controlWeight;

            if (leftEdge < randWeight && randWeight <= rightEdge)
            {
                return dropInfo;
            }
        }
            
        return null;
    }
}
