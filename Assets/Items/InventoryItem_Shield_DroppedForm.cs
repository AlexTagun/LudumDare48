using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem_Shield_Settings", menuName = "Settings/InventoryItems/Shield", order = 1)]
public class InventoryItem_Shield_Settings : ScriptableObject
{
    [SerializeField] public InventoryItem_Shield_HeroInventoryForm _inventoryFormPrefab = null;
    [SerializeField] public InventoryItem_Shield_DroppedForm _droppedFormPrefab = null;
}

// ------------------- Items

//NB: create...Form() should perform initialization from current state

public abstract class InventoryItem_DroppedForm
{
    public InventoryItem_PickedForm moveToInventory(Inventory inContainer) {
        InventoryItem_PickedForm pickedForm = createPickedForm();
        pickedForm.init(inContainer);

        destroy();

        return pickedForm;
    }

    protected abstract InventoryItem_PickedForm createPickedForm();
    protected virtual void destroy() { }
}

public abstract class InventoryItem_PickedForm
{
    public struct MoveToWorldSettings
    {
        public Vector3 position;
    }

    public InventoryItem_DroppedForm moveToWorld(MoveToWorldSettings inMoveSettings) {
        InventoryItem_DroppedForm droppedForm = createDroppedForm(inMoveSettings.position);

        purgeFromInventory();
        destroy();

        return droppedForm;
    }

    public void swapWith(InventoryItem_PickedForm inOther) {
        Inventory myInventory = this.inventoryChecked;
        Inventory otherInventory = inOther.inventoryChecked;

        this.purgeFromInventory();
        inOther.purgeFromInventory();

        this.setInventory(otherInventory);
        inOther.setInventory(myInventory);
    }

    protected abstract void bindToInventory(HeroItemBinding inBinding);
    protected abstract void unbindFromInventory();
    protected abstract InventoryItem_DroppedForm createDroppedForm(Vector3 inPosition);
    protected virtual void destroy() { }

    internal void init(Inventory inInventory) {
        setInventory(inInventory);
    }

    //NB: For Inventory System only
    private void setInventory(Inventory inInventory) {
        if (_inventory != null)
            throw new System.Exception("Has inventory before calling setInventory()");
        
        if (inInventory._item != null)
            throw new System.Exception("Other inventory should be empty, purging should be performed outside for Inventory System logic");

        this._inventory = inInventory;
        inInventory._item = this;
        this.bindToInventory(inventoryChecked._itemBinding);
    }

    internal void purgeFromInventory() {
        if (_inventory == null)
            throw new System.Exception("No inventory before calling setInventory()");

        unbindFromInventory();
        _inventory = null;
    }

    private Inventory inventoryChecked{
        get {
            if (_inventory == null)
                throw new System.Exception();

            return _inventory;
        }
    }

    private Inventory _inventory = null;
}

// ------------------- Inventory

public class HeroAttach
{
    public enum EAttachId
    {
        LeftHand,
        RightHand
    }

    [System.Serializable]
    public struct InitData_Attach
    {
        public EAttachId _attachId;
        public Transform _transform;
    }

    [System.Serializable]
    public struct InitData
    {
        public List<InitData_Attach> _attaches;
    }

    public void init(InitData inInitData) {
        foreach (InitData_Attach initData_attach in inInitData._attaches)
            _attaches.Add(new Attach(initData_attach._attachId, initData_attach._transform));
    }

    public void attach(GameObject inElement, EAttachId inAttachId) {
        Transform transform = getAttachTransform(inAttachId);
        if (transform) {
            inElement.transform.parent = transform;
        } else {
            //TODO: Use here UMAP assertation system
            throw new System.Exception("Cannot find attach for id " + inAttachId);
        }
    }

    public void detach(GameObject inElement) {
        //TODO: Add here checks if element was ever attached
        inElement.transform.parent = null;
    }

    private Transform getAttachTransform(EAttachId inAttachId) {
        //TODO: Cache delegate
        int kUnfoundIndex = -1;
        
        int index = _attaches.FindIndex((Attach inAttach) => {return (inAttach.attachId == inAttachId); });
        return (index != kUnfoundIndex) ? _attaches[index].transform : null;
    }

    private struct Attach
    {
        public Attach(EAttachId inAttachId, Transform inTransform) {
            _attachId = inAttachId;
            _transform = inTransform;
        }

        public EAttachId attachId => _attachId;
        public Transform transform => _transform;

        private EAttachId _attachId;
        private Transform _transform;
    }
    private List<Attach> _attaches;
}

// -----------------------------------------------

public class HeroItemBinding
{
    [System.Serializable]
    public struct InitData {
        public HeroAttach _heroAttach;
    }

    public void init(InitData inInitData) {
        _heroAttach = inInitData._heroAttach;
    }

    public HeroAttach heroAttach => _heroAttach;


    private HeroAttach _heroAttach = null;
}

// -----------------------------------------------

public class Inventory : MonoBehaviour
{
    //NB: Items access only
    internal InventoryItem_PickedForm _item = null;

    internal HeroItemBinding _itemBinding = null;
}

public class InventoryItem_Shield_HeroInventoryForm : MonoBehaviour
{
    private InventoryItem_Shield_HeroInventoryForm moveToHeroInventory(HeroInventory inContainer /* , no-transform-settings */) {
        if (inContainer != _inventory) {
            _inventory.setShield(null);

            inContainer.setShield(gameObject);
            _inventory = inContainer;
        }

        return this;
    }

    private InventoryItem_Shield_HeroInventoryForm swapWithHeroInventory(InventoryItem_Shield_HeroInventoryForm inContainer /* , no-transform-settings */) {
        if (inContainer != _inventory) {
            _inventory.setShield(null);

            inContainer.setShield(gameObject);
            _inventory = inContainer;
        }

        return this;
    }

    private HeroInventory _inventory;

    [SerializeField] private InventoryItem_Shield_Settings _settings = null;
}

public class InventoryItem_Shield_DroppedForm : MonoBehaviour
{
    private void Awake() {
        _pickingTrigger.onEnter.AddListener(processPickingTriggerEnter);
    }

    private InventoryItem_Shield_HeroInventoryForm moveToHeroInventory(HeroInventory inContainer /* , no-transform-settings */) {
        InventoryItem_Shield_HeroInventoryForm nextForm = Instantiate(_settings._inventoryFormPrefab);
        inContainer.setShield(nextForm.gameObject);

        Destroy(gameObject);
        
        return nextForm;
    }

    private void processPickingTriggerEnter(Collider inOther) {
        HeroInventory heroInventory = getHeroInvetoryFrom(inOther);
        if (heroInventory != null)
            pick(heroInventory);
    }

    private static HeroInventory getHeroInvetoryFrom(Collider inCollider) {
        return inCollider.transform.GetComponent<HeroInventory>();
    }

    private void pick(HeroInventory inHeroInventory) {
        moveToHeroInventory(inHeroInventory);
    }

    [SerializeField] private InventoryItem_Shield_Settings _settings = null;
    [SerializeField] private TriggerObject _pickingTrigger = null;
}
