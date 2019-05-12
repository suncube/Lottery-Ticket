using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScratchPainter : MonoBehaviour
{
    // move to helper
    public GameObject brushContainer;
    public Camera canvasCam;

    public GraphicRaycaster m_Raycaster;

    // to LoteryImage
    public RectTransform StratchCover;
    public LotteryTicket LotteryTicket;
    public RawImage RawImage;

    private float brushSize = 1.0f;
    private Rect BrushRect = new Rect(0, 0, 100, 100);
    private RenderTexture renderTexture;

    private Vector3 mousePosition;
    private EventSystem m_EventSystem; 

    private int brushCounter;
    private int MAX_BRUSH_COUNT;

    void Start()
    {
        brushCounter = 0;
        MAX_BRUSH_COUNT = 1000;

        m_EventSystem = GetComponent<EventSystem>();

        renderTexture = new RenderTexture((int)StratchCover.sizeDelta.x, (int)StratchCover.sizeDelta.y, 1);
        RawImage.texture = renderTexture;
        canvasCam.targetTexture = renderTexture;
        //
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.mousePosition != mousePosition)
            {
                mousePosition = Input.mousePosition;
                TouchProcessing();
            }
        }
    }

    private void TouchProcessing()
    {
        if (brushCounter >= MAX_BRUSH_COUNT)
            return;

        Vector3 screenpos = Vector3.zero;
        Vector3 uvWorldPosition = Vector3.zero;

        if (GetPositionsOnPaintView(ref screenpos, ref uvWorldPosition))
        {

            PaintStratch(uvWorldPosition);
            
            BrushRect.x = screenpos.x - BrushRect.width / 2;
            BrushRect.y = screenpos.y - BrushRect.height / 2;

            LotteryTicket.CanculateIntersects(BrushRect);

        }

    }

    private void PaintStratch(Vector3 uvWorldPosition)
    {
        // 
        GameObject brushObj;

        brushObj = (GameObject)Instantiate(Resources.Load("BrushEntity"));
        brushObj.GetComponent<SpriteRenderer>().color = Color.red;

        var position = canvasCam.ViewportToWorldPoint(uvWorldPosition);
        brushObj.transform.parent = brushContainer.transform;
        brushObj.transform.position = new Vector3(position.x, position.y, 0);
        brushObj.transform.localScale = Vector3.one * brushSize;
        //
        brushCounter++;
    }

    private bool GetPositionsOnPaintView(ref Vector3 canvasPos, ref Vector3 uvPos)
    {
        var m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        m_Raycaster.Raycast(m_PointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.name == StratchCover.gameObject.name)
            {
                Vector2 localPos = Vector2.zero;

                localPos = RectTransformUtility.PixelAdjustPoint(result.screenPosition, StratchCover,
                    GetComponent<Canvas>());

                canvasPos = localPos;

                Vector2 posIn;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(StratchCover, mousePosition, null, out posIn))
                {

                    var X = (-StratchCover.rect.width / 2 - posIn.x) / StratchCover.rect.width;
                    var Y = (-StratchCover.rect.height / 2 - posIn.y) / StratchCover.rect.height;
                    uvPos = new Vector3((X * -1), (Y * -1), 0);
                }

                return true;
            }
        }

        return false;
    }
}