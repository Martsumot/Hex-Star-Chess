using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HexStarChess.View 
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;

        [SerializeField]
        private LayerMask _layerMask;

        public Action<Tile> OnPointerEnterTile;

        public Action<Tile> OnSelectTile;

        private Tile _tileOnPointer = null;
        private void Awake()
        {
            _playerInputActions = new();
            _playerInputActions.Enable();
            _playerInputActions.Player.ChangePosition.performed += PointerEnterTile;
            _playerInputActions.Player.Select.performed += SelectTile;
        }

        private void OnDestroy()
        {
            _playerInputActions.Player.ChangePosition.performed -= PointerEnterTile;
            _playerInputActions.Player.Select.performed -= SelectTile;
            _playerInputActions.Disable();
        }

        private void PointerEnterTile(InputAction.CallbackContext callbackContext)
        {
            Ray ray = Camera.main.ScreenPointToRay(callbackContext.ReadValue<Vector2>());
            //rayÇ™ê⁄êGÇµÇΩéûÇÃÇ›
            if (Physics.Raycast(ray, out RaycastHit raycastHit, _layerMask))
            {
                Tile tile = raycastHit.collider.gameObject.GetComponent<Tile>();
                if (tile != _tileOnPointer)
                {
                    _tileOnPointer = tile;
                    OnPointerEnterTile?.Invoke(_tileOnPointer);
                }
            }//rayÇ™ê⁄êGÇπÇ∏ÅA_selectedTileÇ…Ç»Ç…Ç©Ç™ê›íËÇ≥ÇÍÇƒÇ¢ÇΩéûÇÃÇ›
            else if(_tileOnPointer is not null)
            {
                _tileOnPointer = null;
                OnPointerEnterTile?.Invoke(_tileOnPointer);
            }
        }

        private void SelectTile(InputAction.CallbackContext callbackContext)
        {
            OnSelectTile?.Invoke(_tileOnPointer);
        }
    }
}