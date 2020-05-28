using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{

    // A reference t o the useable on the actionbutton
    public IUseable MyUseable { get; set; }

    [SerializeField]
    private Text stackSize;

    private Stack<IUseable> useables = new Stack<IUseable>();

    private int count;

    // A reference to the actual button that this button uses
    public Button MyButton { get; private set; }

    public Image MyIcon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    public int MyCount
    {
        get
        {
            return count;
        }
    }

    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
    }

    public Stack<IUseable> MyUseables
    {
        get
        {
            return useables;
        }
        set
        {
            if (value.Count > 0)
            {
                MyUseable = value.Peek();
            }
            else
            {
                MyUseable = null;
            }

            useables = value;
        }
    }

    [SerializeField]
    private Image icon;

    // Use this for initialization
    void Start()
    {
        MyButton = GetComponent<Button>();

        MyButton.onClick.AddListener(OnClick);

        InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// This is executed the the button is clicked
    /// </summary>
    public void OnClick()
    {
        if (HandScript.MyInstance.MyMoveable == null)
        {
            if (MyUseable != null)
            {
                MyUseable.Use();
            }
            else if (MyUseables != null && MyUseables.Count > 0)
            {
                MyUseables.Peek().Use();
            }
        }
    }

    /// <summary>
    /// Checks if someone clicked on the actionbutton
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is IUseable)
            {
                SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
            }
        }
    }

    /// <summary>
    /// Sets the useable on an actionbutton
    /// </summary>
    public void SetUseable(IUseable useable)
    {
        if (useable is Item)
        {
            MyUseables = InventoryScript.MyInstance.GetUseables(useable);

            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;

            InventoryScript.MyInstance.FromSlot = null;
        }
        else
        {
            MyUseables.Clear();

            this.MyUseable = useable;
        }

        count = MyUseables.Count;

        UpdateVisual(useable as IMoveable);

        UIManager.MyInstance.RefreshToolTip(MyUseable as IDescribable);
    }

    /// <summary>
    /// Updates the visual representation of the actionbutton
    /// </summary>
    public void UpdateVisual(IMoveable moveable)
    {
        if (HandScript.MyInstance.MyMoveable != null)
        {
            HandScript.MyInstance.Drop();
        }

        MyIcon.sprite = moveable.MyIcon;

        MyIcon.color = Color.white;

        if (count > 1)
        {
            UIManager.MyInstance.UpdateStackSize(this);
        }
    }

    public void UpdateItemCount(Item item)
    {
        if (item is IUseable && MyUseables.Count > 0)
        {
            if(MyUseables.Peek().GetType() == item.GetType())
            {
                MyUseables = InventoryScript.MyInstance.GetUseables(item as IUseable);

                count = MyUseables.Count;

                UIManager.MyInstance.UpdateStackSize(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IDescribable tmp = null;

        if (MyUseable != null && MyUseable is IDescribable)
        {
            tmp = (IDescribable)MyUseable;

            //UIManager.MyInstance.ShowTooltip(transform.position, tmp);
        }
        else if (MyUseables.Count > 0)
        {
            //UIManager.MyInstance.ShowTooltip(transform.position, tmp);
        }
        else if (tmp != null)
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(1,0), transform.position, tmp);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }
}
