using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler
{

    public IUseable MyUseable { get; set; }//A reference too the useable on the actionbutton

    public Button MyButton { get; private set; }//A reference to the actual button that this button uses

    public Image MyIcon { get => icon; set => icon = value; }

    [SerializeField]
    private Image icon;

    // Start is called before the first frame update
    void Start()
    {
        MyButton = GetComponent<Button>();

        MyButton.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()//This is executed the button clicked
    {
        if (MyUseable != null)
        {
            MyUseable.Use();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
