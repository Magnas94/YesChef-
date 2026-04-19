using UnityEngine;
using UnityEngine.Animations.Rigging;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_MoveSpeed = 5f;
    [SerializeField] float m_RotationSpeed = 10f;

    [SerializeField] Animator m_Animator;

    [SerializeField] IngredientInstance m_CurrentItem;
    public IngredientInstance CurrentItem => m_CurrentItem;

    [SerializeField] LayerMask m_InteractableLayer;

    CharacterController m_CC;

    [SerializeField] float m_RayCastYOffset = -0.1f;
    [SerializeField] UIManager m_UIManager;
    IInteractable m_CurrentInteractable;
    
    // Rigging is used to make player hold tray
    [SerializeField] RigBuilder m_HandsRig;
    [SerializeField] GameObject m_Tray;

    Camera m_MainCam;

    private void Start()
    {
        m_CC = GetComponent<CharacterController>();
        m_MainCam = Camera.main;
        ShowTray(false);
    }

    void Update()
    {
        Move();
        GetInteractableInFront();
        UpdateUI();
        if (Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    void Move()
    {
        float l_H = Input.GetAxis("Horizontal");
        float l_V = Input.GetAxis("Vertical");

        Vector3 l_CamForward = m_MainCam.transform.forward;
        Vector3 l_CamRight = m_MainCam.transform.right;

        l_CamForward.y = 0.0f;
        l_CamRight.y = 0.0f;

        m_CC.Move((l_CamForward * l_V  + l_CamRight * l_H) * Time.deltaTime * m_MoveSpeed);
        transform.rotation = Quaternion.LookRotation(l_CamForward * l_V + l_CamRight * l_H);

        bool l_IsMoving = l_H != 0.0f || l_V != 0.0f;
        m_Animator.SetBool("_IsRunning", l_IsMoving);
        m_Animator.SetFloat("_Direction", 1.5f);
    }

    void GetInteractableInFront() 
    {
        var l_Colliders = Physics.OverlapSphere(transform.position, 1f, m_InteractableLayer);
        if (l_Colliders.Length > 0)
        {
            m_CurrentInteractable = l_Colliders[0].GetComponent<IInteractable>();
        }
        else
        {
            if (m_CurrentInteractable != null)
            {
                m_CurrentInteractable.OnInteractionLeft(this);
                m_CurrentInteractable = null;
            }
        }
    }

    void UpdateUI() 
    {
        if (m_CurrentInteractable != null)
            m_UIManager.GiveInteractionTip(true, m_CurrentInteractable as BaseStation);
        else
            m_UIManager.GiveInteractionTip(false);
    }

    void Interact()
    {
        if (m_CurrentInteractable == null)
            return;
        m_CurrentInteractable.Interact(this);
    }

    /// <summary>
    /// Give an item to the player
    /// </summary>
    /// <param name="item"></param>
    public void GiveItem(IngredientInstance item)
    {
        m_CurrentItem = item;
        item.transform.SetParent(m_Tray.transform);
        item.transform.localPosition = new Vector3(0, .1f, 0);
        item.transform.localEulerAngles = new Vector3(90f, 0, 0);
        ShowTray(true);
    }

    /// <summary>
    /// Take an item from the player
    /// </summary>
    /// <returns></returns>
    public IngredientInstance TakeItem()
    {
        var item = CurrentItem;
        m_CurrentItem = null;
        ShowTray(false);
        return item;
    }

    /// <summary>
    /// When player trashes them
    /// </summary>
    public void ClearItem()
    {
        if (CurrentItem != null)
        {
            Destroy(CurrentItem.gameObject);
            m_CurrentItem = null;
            ShowTray(false);
        }
    }

    /// <summary>
    /// Show or Hide the tray
    /// </summary>
    /// <param name="a_Show"></param>
    public void ShowTray(bool a_Show) 
    {
        m_HandsRig.layers[0].rig.weight = a_Show ? 1.0f : 0.0f;
        m_Tray.gameObject.SetActive(a_Show);
    }
}