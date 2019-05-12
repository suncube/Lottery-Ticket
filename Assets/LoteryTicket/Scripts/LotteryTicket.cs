using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LotteryTicket : MonoBehaviour
{
    [SerializeField]
    private Text BonusText; 

    [SerializeField]
    private ScratchBonus[] m_scratchBonus;

    private float[] probs = new[] { 0.4f, 0.25f, 0.2f, 0.15f};
    private int[] bonusRes= new[] { 3, 0, 1, 2};

    private int[] bonusValues = new[] {1000, 2000, 3000};
    private int[] usesBonusValues;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        BonusText.text = "?";
        BonusReward = 0;
        usesBonusValues = new int[m_scratchBonus.Length];

        // generate bonus
        var random = bonusRes[GetRandom(probs)];
        Debug.Log("random " + random);
        if (random == 3)
        {
            for (var index = 0; index < m_scratchBonus.Length; index++)
            {
                usesBonusValues[index] = bonusValues[Random.Range(0, 2)];
                m_scratchBonus[index].SetBonusValue(usesBonusValues[index]);
            }
            CanculateBonus();

        }
        else
        {
            BonusReward = bonusValues[random];
            Debug.Log("Generate " + BonusReward);

            for (var index = 0; index < m_scratchBonus.Length; index++)
            {
                usesBonusValues[index] = BonusReward;
                m_scratchBonus[index].SetBonusValue(BonusReward);
            }
        }

        Debug.Log("BonusReward " + BonusReward);
    }

    public void CanculateIntersects(Rect brushRect)
    {
        var opened = 0;

        for (var index = 0; index < m_scratchBonus.Length; index++)
        {
            var bonus = m_scratchBonus[index];
            if (bonus.TargetImage.isPaintedFlag)
            {
                continue;
            }
            opened++;

            Rect result;
            if (brushRect.Intersects(bonus.TargetImage.Rect, out result))
            {
                var addIntersectRects = bonus.TargetImage.AddIntersectRects(result);
                if (addIntersectRects)
                {
                    bonus.TargetImage.Image.color = Color.green;
                }
            }
        }

        if (opened == 0)
            ShowResult();

    }

    private void ShowResult()
    {
        BonusText.text = string.Format("{0}$ ",BonusReward);
    }

    private int BonusReward;
    private void CanculateBonus()
    {
        bool isEqual = true;
        var bonusValue = usesBonusValues[0];
        // operate
        for (var index = 1; index < usesBonusValues.Length; index++)
        {
            if (usesBonusValues[index] != bonusValue)
            {
                isEqual = false;
                break;
            }
        }

        if (isEqual)
        {
            BonusReward = bonusValue;
        }
        else
        {
            BonusReward = 0;
        }
    }

    // helper
    private int GetRandom(float[] probability)
    {
        float total = 0;

        for (int index = 0; index < probability.Length; index++)
            total += probability[index];

        if (total > 1)
            throw new Exception("Overall probability is greater than 1");

        var randomPoint = Random.value * total;

        for (int i = 0; i < probability.Length; i++)
        {
            if (randomPoint <= probability[i])
                return i;

            randomPoint -= probability[i];

        }

        return probability.Length - 1;
    }
}