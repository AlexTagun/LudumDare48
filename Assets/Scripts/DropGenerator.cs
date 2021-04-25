using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DropGenerator : MonoBehaviour
{
    [SerializeField] private List<DropInfo> _positiveDrops;
    [SerializeField] private List<DropInfo> _negativeDrops;

    private GeneratorConfig _generatorConfig;

    private void Awake()
    {
        _generatorConfig = DataManager.GeneratorConfig;
    }

    public DropInfo GetDropInfo(int index)
    {
        var drops = GetDrops(index);

        if (drops == null)
        {
            return null;
        }
        
        var allWeight = 0;

        foreach (var dropInfo in drops)
        {
            allWeight += dropInfo.Weight;
        }

        var randWeight = Random.Range(1, allWeight + 1);

        var controlWeight = 0;

        foreach (var dropInfo in drops)
        {
            if (CheckControlWeight(ref controlWeight, randWeight, dropInfo.Weight))
            {
                return dropInfo;
            }
        }
            
        return null;
    }

    private List<DropInfo> GetDrops(int index)
    {
        index = Math.Min(index, _generatorConfig.GeneratorDatas.Count - 1);

        var currentGenerationData = _generatorConfig.GeneratorDatas[index];
        
        var allWeight = 0f;

        allWeight += currentGenerationData.PositiveDropWeight;
        allWeight += currentGenerationData.NegativeDropWeight;
        allWeight += currentGenerationData.NoneDropWeight;

        var randWeight = Random.Range(1, allWeight + 1);

        var controlWeight = 0;
        
        if (CheckControlWeight(ref controlWeight, randWeight, currentGenerationData.PositiveDropWeight))
        {
            return _positiveDrops;
        }
        
        if (CheckControlWeight(ref controlWeight, randWeight, currentGenerationData.NegativeDropWeight))
        {
            return _negativeDrops;
        }

        return null;
    }

    private bool CheckControlWeight(ref int controlWeight, float randWeight, int targetWeight)
    {
        var leftEdge = controlWeight;
        controlWeight += targetWeight;
        var rightEdge = controlWeight;

        return IsInRange(leftEdge, rightEdge, randWeight);
    }

    private bool IsInRange(int leftEdge, int rightEdge, float randWeight)
    {
        return leftEdge < randWeight && randWeight <= rightEdge;
    }
}
