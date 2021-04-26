using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderDecoration : MonoBehaviour
{
    private void Start() {
        initDecoratioins();
    }

    private void initDecoratioins() {
        if (_initData_decoration != null && _initData_decoration.Count > 0) {
            _decorationsLastIndex = _initData_decoration[0]._stepIndex;
            _decorationsFirstIndex = _initData_decoration[0]._stepIndex;

            _decorations = new SortedDictionary<int, Decoration>();
            foreach (InitData_Decoration initData_decoration in _initData_decoration) {
                _decorations.Add(
                    initData_decoration._stepIndex,
                    new Decoration(initData_decoration, transform));

                _decorationsFirstIndex = Mathf.Min(_decorationsFirstIndex, initData_decoration._stepIndex);
                _decorationsLastIndex = Mathf.Max(_decorationsLastIndex, initData_decoration._stepIndex);
            }
        }
    }

    //Ladder API
    public struct DecorationAttachPoint {
        public Vector3 _position;
        public Quaternion _rotation;
    }

    public void showStepDecoration(int inStepIndex, DecorationAttachPoint inDecorationAttachPoint) {
        Decoration stepDecoration = getDecorationForStepIndex(inStepIndex);
        if (stepDecoration != null) {
            stepDecoration._object.SetActive(true);
            stepDecoration._object.transform.localPosition = inDecorationAttachPoint._position;
            stepDecoration._object.transform.localRotation = inDecorationAttachPoint._rotation;
        }
    }

    public void hideStepDecoration(int inStepIndex) {
        Decoration stepDecoration = getDecorationForStepIndex(inStepIndex);
        if (stepDecoration != null)
            stepDecoration._object.SetActive(false);
    }

    private Decoration getDecorationForStepIndex(int inStepIndex) {
        int stepsRange = _decorationsLastIndex - _decorationsFirstIndex;
        int firstIndexRelatedStepIndex = inStepIndex - _decorationsFirstIndex;
        int normalizedFirstIndexRelatedStepIndex = firstIndexRelatedStepIndex % (stepsRange + 1);
        if (normalizedFirstIndexRelatedStepIndex < 0)
            normalizedFirstIndexRelatedStepIndex = normalizedFirstIndexRelatedStepIndex + stepsRange;

        Decoration stepDecoration = null;
        _decorations.TryGetValue(normalizedFirstIndexRelatedStepIndex, out stepDecoration);
        return stepDecoration;
    }

    [System.Serializable]
    public struct InitData_Decoration
    {
        public int _stepIndex;
        public GameObject _decorationPrefab;
    }
    [SerializeField] private List<InitData_Decoration> _initData_decoration;


    private class Decoration
    {
        public Decoration(InitData_Decoration inInitData, Transform inParentTransform) {
            _object = Instantiate(inInitData._decorationPrefab, inParentTransform);
            _object.SetActive(false);
        }

        public GameObject _object;
    }
    private SortedDictionary<int, Decoration> _decorations;
    private int _decorationsFirstIndex;
    private int _decorationsLastIndex;
}
