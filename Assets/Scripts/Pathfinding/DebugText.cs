using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI f, g, h, p;

    [SerializeField]
    private RectTransform arrow;

    public TextMeshProUGUI F { get => f; set => f = value; }
    public TextMeshProUGUI G { get => g; set => g = value; }
    public TextMeshProUGUI H { get => h; set => h = value; }
    public TextMeshProUGUI P { get => p; set => p = value; }
    public RectTransform Arrow { get => arrow; set => arrow = value; }
}
