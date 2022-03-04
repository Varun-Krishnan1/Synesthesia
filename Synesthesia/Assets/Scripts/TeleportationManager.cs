using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    public Material correctMaterial;
    public Material incorrectMaterial; 
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private XRInteractorLineVisual lineVisual;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private TeleportationProvider provider;
    private InputAction _thumbstick;
    private bool _isActive;

    private XRRig rig; 
    private TeleportationAnchor currentAnchor;
    private bool onOrca;
    private bool beenAtBottle = false; 

    void Awake()
    {
        rig = GameManager.Instance.rig; 
    }
    void Start()
    {
        rayInteractor.enabled = false;

        var activate = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Activate");
        activate.Enable();
        activate.performed += OnTeleportActivate;

        var cancel = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Cancel");
        cancel.Enable();
        cancel.performed += OnTeleportCancel;

        _thumbstick = actionAsset.FindActionMap("XRI LeftHand").FindAction("Move");
        _thumbstick.Enable();
    }

    void Update()
    {

        if (!_isActive)
            return;

        bool hittingObject = rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit);
        if (hittingObject)
        {
            Debug.Log(hit.transform.gameObject); 
            if (hit.transform.GetComponent<TeleportationArea>() || hit.transform.GetComponent<TeleportationAnchor>())
            {
                lr.material = correctMaterial;
            }
            else
            {
                lr.material = incorrectMaterial;
            }
        }
        else
        {
            lr.material = incorrectMaterial;
        }

        if (_thumbstick.ReadValue<Vector2>() != Vector2.zero)
            return;

        //Debug.Log(rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit y));
        //Debug.Log(y.collider);

        // -- Ray cast canceled 
        if (!hittingObject || hit.transform.GetComponent<TeleportationAnchor>() == null)
        {
            rayInteractor.enabled = false;
            _isActive = false;
            return;
        }

        // -- as you teleport away from anchor re-enable the collider 
        if (currentAnchor)
        {
            currentAnchor.GetComponent<Collider>().enabled = true;

            if(onOrca)
            {
                rig.transform.parent = null;
                lineVisual.lineLength = 98f;
                onOrca = false; 
            }
        }

        TeleportationAnchor anchor = hit.transform.GetComponent<TeleportationAnchor>();
        rig.gameObject.transform.position = anchor.teleportAnchorTransform.position;

        AudioManager.Instance.PlayTeleportationSoundEffect(1f, 0f);

        // -- disable collider on new anchor 
        anchor.GetComponent<Collider>().enabled = false;
        currentAnchor = anchor;

        //TeleportRequest request = new TeleportRequest()
        //{
        //    destinationPosition = hit.point,
        //    // destinationRotation = ?,
        //};

        //provider.QueueTeleportRequest(request);
        TeleportationCannon cannon = hit.transform.GetComponent<TeleportationCannon>();
        if(cannon)
        {
            cannon.HideArrow(); 
        }

        TeleportationBottle bottle = hit.transform.GetComponent<TeleportationBottle>();
        if(bottle && !beenAtBottle)
        {
            AudioManager.Instance.StartStageTheme(3);
            AudioManager.Instance.PlaySoundEffect(18, .5f, .22f);
            StageThree.Instance.bottleFishSpawner.SetActive(true);
            beenAtBottle = true; 
        }


        Orca orca = hit.transform.GetComponent<Orca>();
        if(orca)
        {
            rig.gameObject.transform.position = anchor.teleportAnchorTransform.position;
            rig.transform.parent = orca.transform;

            lineVisual.lineLength = 200f; 

            onOrca = true; 
        }

        Treasure treasure = hit.transform.GetComponent<Treasure>();
        if (treasure)
        {
            treasure.CheckKeys(); 
        }

        rayInteractor.enabled = false;
        _isActive = false;
    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = true;
        _isActive = true;
    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = false;
        _isActive = false;
    }
}
