using UnityEngine;
using UnityEngine.UI;

public class ScratchBonus : MonoBehaviour
{
    public TargetImage TargetImage { get; private set; }

    [SerializeField]
    private Text m_BonusView;

    private void Start()
    {
        var rect = GetComponent<RectTransform>();

        var X = rect.position.x - rect.rect.width * rect.pivot.x;
        var Y = rect.position.y - rect.rect.height * rect.pivot.y;
        var rectP = new Rect(X, Y, rect.rect.width, rect.rect.height);
        TargetImage = new TargetImage(rectP);

        TargetImage.Image = rect.GetComponent<Image>();
    }

    public void SetBonusValue(int bonus)
    {
        m_BonusView.text = string.Format("{0}$ ", bonus);
    }
}