using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, ILazyMapManager
{
    private void Awake() {
        initCaches();
        initLazyState();
    }

    private void FixedUpdate() {
        if (!oldTestBounds.Equals(testBounds)) {
            updateFocus(testBounds);
            oldTestBounds = testBounds;
        }
    }

    private void initCaches() {
        _cache._anglePerStep = 360f / _stepsInCircle;
        _cache._zAngleRadians = _zAngle * Mathf.Deg2Rad;
        _cache._stepSize = _radius * Mathf.Sin(_cache._anglePerStep / 2f) * 2;
        _cache._stepDeltaZ = _cache._stepSize * Mathf.Sin(_cache._zAngleRadians / 2f);
        _cache._stepRotator = Quaternion.Euler(0f, _cache._anglePerStep, 0f);
        _cache._stageZHeight = _stepsInCircle * _cache._stepDeltaZ;
    }

    private void initLazyState() {
        _lazyMapState._minRampIndex = 0;
        _lazyMapState._activeRamps = new LinkedList<Ramp>();
        _lazyMapState._freeRamps = new Stack<Ramp>();

        //NB: We need this hack because indices "from-to" means
        // "including both min and max index", so initial state with zero bounds
        // should have one step. It is good idea to make this one step element later
        // instead of this workaround.
        _lazyMapState._maxRampIndex = -1;
    }

    private static void swap<T>(ref T a, ref T b) {
        T tmp = a;
        a = b;
        b = tmp;
    }

    //NB: Currently make no checks in XZ plane, only Y (aka Z) check
    public void updateFocus(Bounds inFocus) {
        float newMinZ = inFocus.center.y - inFocus.size.y / 2f;
        float newMaxZ = inFocus.center.y + inFocus.size.y / 2f;

        int newMinRampIndex = getRampIndexFromLocalYPosition(newMinZ);
        int newMaxRampIndex = getRampIndexFromLocalYPosition(newMaxZ);

        if (newMinRampIndex > newMaxRampIndex)
            swap(ref newMinRampIndex, ref newMaxRampIndex);

        updateFocus_freeOutOfFocusRamps(newMinRampIndex, newMaxRampIndex);
        updateFocus_placeStepsForFocus(newMinRampIndex, newMaxRampIndex);

        _lazyMapState._minRampIndex = newMinRampIndex;
        _lazyMapState._maxRampIndex = newMaxRampIndex;
    }

    private void updateFocus_placeStepsForFocus(int inNewMinRampIndex, int inNewMaxRampIndex) {

        //NB: Mathf.Min / Mathf.Max for cases when new range is out of current range at all

        updateFocus_placeStepsForFocus_placeSteps(
            inNewMinRampIndex,
            Mathf.Min(_lazyMapState._minRampIndex - 1, inNewMaxRampIndex),
            EUpdateFocus_placeStepsForFocus_placeSteps_InsertPlace.AtStart);

        updateFocus_placeStepsForFocus_placeSteps(
            Mathf.Max(_lazyMapState._maxRampIndex + 1, inNewMinRampIndex),
            inNewMaxRampIndex,
            EUpdateFocus_placeStepsForFocus_placeSteps_InsertPlace.AtEnd);
    }

    enum EUpdateFocus_placeStepsForFocus_placeSteps_InsertPlace
    {
        AtStart,
        AtEnd
    }

    void updateFocus_placeStepsForFocus_placeSteps(int indexFrom, int indexTo,
        EUpdateFocus_placeStepsForFocus_placeSteps_InsertPlace insertPlace)
    {
        if (indexFrom <= indexTo) {

            float startingAngel = indexFrom * anglePerStep;
            float startingZ = -indexFrom * stepDeltaZ;
            Vector3 stepPointDirection = Quaternion.Euler(0f, startingAngel, 0f) * Vector3.right;
            Vector3 stepZPoint = new Vector3(0f, startingZ, 0f);

            LinkedListNode<Ramp> baseNodeToInsert = null;
            switch (insertPlace)
            {
                case EUpdateFocus_placeStepsForFocus_placeSteps_InsertPlace.AtStart:
                    baseNodeToInsert = _lazyMapState._activeRamps.First;
                    break;
                case EUpdateFocus_placeStepsForFocus_placeSteps_InsertPlace.AtEnd:
                    baseNodeToInsert = _lazyMapState._activeRamps.Last;
                    break;
            }

            int stepsNum = (indexTo - indexFrom) + 1;
            for (int placedSteps = 0; placedSteps < stepsNum; ++placedSteps) {

                Vector3 nextStepPointDirection = stepsRotator * stepPointDirection;

                Vector3 stepNormal = (nextStepPointDirection - stepPointDirection).normalized;
                Vector3 additionalStepSizeDelta = stepNormal * _additionalStepSize * 0.5f;

                Vector3 stepPoint =
                    stepZPoint +
                    stepPointDirection * _radius -
                    additionalStepSizeDelta;

                stepZPoint.y -= stepDeltaZ;
                Vector3 nextStepPoint =
                    stepZPoint +
                    nextStepPointDirection * _radius +
                    additionalStepSizeDelta;

                Ramp newRamp = updateFocus_placeStepsForFocus_placeSteps_makeRamp();
                newRamp.placeLocal(stepPoint, nextStepPoint, _stepWidth, _stepHeight);
                switch (insertPlace)
                {
                    case EUpdateFocus_placeStepsForFocus_placeSteps_InsertPlace.AtStart:
                        if (baseNodeToInsert != null)
                            _lazyMapState._activeRamps.AddBefore(baseNodeToInsert, newRamp);
                        else
                            _lazyMapState._activeRamps.AddLast(newRamp); //NB: Case when there where no elements
                        break;
                    case EUpdateFocus_placeStepsForFocus_placeSteps_InsertPlace.AtEnd:
                        _lazyMapState._activeRamps.AddLast(newRamp);
                        break;
                }

                stepPointDirection = nextStepPointDirection;
            }
        }
    }


    Ramp updateFocus_placeStepsForFocus_placeSteps_makeRamp() {
        Ramp newRamp = null;

        if (_lazyMapState._freeRamps.Count == 0) {
            newRamp = Instantiate(_rampPrefab, transform);
        } else {
            newRamp = _lazyMapState._freeRamps.Pop();
            newRamp.gameObject.SetActive(true);
        }

        return newRamp;
    }

    private void updateFocus_freeOutOfFocusRamps(int newMinRampIndex, int newMaxRampIndex) {
        for (int rampIndex = _lazyMapState._minRampIndex,
            lastIndex = Mathf.Min(newMinRampIndex, _lazyMapState._maxRampIndex);
            rampIndex < lastIndex;
            ++rampIndex)
        {
            updateFocus_freeOutOfFocusRamps_freeRamp(_lazyMapState._activeRamps.First.Value);
            _lazyMapState._activeRamps.RemoveFirst();
        }

        for (int rampIndex = Mathf.Max(newMaxRampIndex, _lazyMapState._minRampIndex);
            rampIndex < _lazyMapState._maxRampIndex;
            ++rampIndex)
        {
            updateFocus_freeOutOfFocusRamps_freeRamp(_lazyMapState._activeRamps.Last.Value);
            _lazyMapState._activeRamps.RemoveLast();
        }
    }

    private void updateFocus_freeOutOfFocusRamps_freeRamp(Ramp inRamp) {
        inRamp.gameObject.SetActive(false);
        _lazyMapState._freeRamps.Push(inRamp);
    }

    //TODO: Make possible to set rounding way
    private int getRampIndexFromLocalYPosition(float inLocalYPosition) {
        return Mathf.CeilToInt(-inLocalYPosition / stepDeltaZ);
    }

    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _stepWidth = 4f;
    [SerializeField] private float _stepHeight = 1f;
    [SerializeField] private float _zAngle = 10;
    [SerializeField] private int _stepsInCircle = 10;
    [SerializeField] private float _additionalStepSize = 0f;

    [SerializeField] Ramp _rampPrefab = null;

#region Caches
    struct Cache
    {
        public float _anglePerStep;
        public float _zAngleRadians;
        public float _stepSize;
        public float _stepDeltaZ;
        public Quaternion _stepRotator;
        public float _stageZHeight;
    };
    private Cache _cache = new Cache();

    private float anglePerStep => _cache._anglePerStep;
    private float zAngleRadians => _cache._zAngleRadians;
    private float stepSize => _cache._stepSize;
    private float stepDeltaZ => _cache._stepDeltaZ;
    private Quaternion stepsRotator => _cache._stepRotator;
    private float stageZHeight => _cache._stageZHeight;
    #endregion

    struct LazyMapState
    {
        public int _minRampIndex;
        public int _maxRampIndex;
        public LinkedList<Ramp> _activeRamps;

        public Stack<Ramp> _freeRamps;
    };
    private LazyMapState _lazyMapState = new LazyMapState();






    [SerializeField] private Bounds testBounds = new Bounds(Vector3.zero, new Vector3(0f, 10f, 0f));

    public void TestSetZPosition(float zPosition)
    {
        testBounds.center = new Vector3(0, zPosition, 0);
    }
    
    private Bounds oldTestBounds = new Bounds();
}
