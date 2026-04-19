using UnityEngine;

public enum ProcessType
{
    None,
    Chop,
    Cook
}

[CreateAssetMenu(menuName = "YesChef/Food Type")]
public class FoodType : ScriptableObject
{
    /// <summary>
    /// ID of the food item
    /// </summary>
    [SerializeField] string m_Id;

    /// <summary>
    /// UI Representation
    /// </summary>
    [SerializeField] Sprite m_RawIcon;
    public Sprite RawIcon => m_RawIcon;

    [SerializeField] Sprite m_ProcessedIcon;
    public Sprite ProcessedIcon => m_ProcessedIcon;

    /// <summary>
    /// Display name - for UI purposes
    /// </summary>
    [SerializeField] string m_DisplayName;

    /// <summary>
    /// Score given by this item
    /// </summary>
    [SerializeField] int m_ScoreValue;
    public int ScoreValue => m_ScoreValue;

    /// <summary>
    /// Whether this food item needs to be processed for serving?
    /// </summary>
    [SerializeField] bool m_RequiresProcessing;
    public bool RequiresProcessing => m_RequiresProcessing;

    /// <summary>
    /// Which process does the food undergo for preparation
    /// </summary>
    [SerializeField] ProcessType m_RequiredProcess;
    public ProcessType RequiredProcess => m_RequiredProcess;

    [SerializeField] GameObject m_RawPrefab;
    public GameObject RawPrefab => m_RawPrefab;

    [SerializeField] GameObject m_ProcessedPrefab;
    public GameObject ProcessedPrefab => m_ProcessedPrefab;
}