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
    [SerializeField] private LineRenderer lr;
    [SerializeField] private TeleportationProvider provider;
    private InputAction _thumbstick;
    private bool _isActive;

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
        //if (!hittingObject || hit.transform.GetComponent<TeleportationArea>() == null)
        if(!hittingObject || hit.transform.GetComponent<TeleportationAnchor>() == null )
        {
            rayInteractor.enabled = false;
            _isActive = false;
            return;
        }


        TeleportRequest request = new TeleportRequest()
        {
            destinationPosition = hit.point,
            // destinationRotation = ?,
        };

        provider.QueueTeleportRequest(request);

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
