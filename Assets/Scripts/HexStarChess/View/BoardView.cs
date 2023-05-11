using System.Collections.Generic;
using UnityEngine;

namespace HexStarChess.View
{
    public class BoardView : MonoBehaviour
    {
        public delegate void SelectEventHandler(HexBoardVector2Int hexCoords);
        [SerializeField]
        private GameObject _tilePrefab;

        [SerializeField]
        private LayerMask _selectionMask;

        private Tile _onPointerTile = null;

        private List<Tile> _movableHighlightTile = new();

        private List<Tile> _attackableHighlightTile = new();

        private Dictionary<HexBoardVector2Int, Tile> _hexCoordsGameObjectPairs = new();

        public void CreateBoard(List<HexBoardVector2Int> hexCoordsList)
        {
            foreach(HexBoardVector2Int hexCoords in hexCoordsList)
            {
                _hexCoordsGameObjectPairs.Add(hexCoords, CreateHexGameObject(hexCoords).GetComponent<Tile>());
            }
        }

        private GameObject CreateHexGameObject(HexBoardVector2Int hexCoords)
        {
            Vector3 worldPosition = HexCoordsConverter.ConvertWorldPosition(hexCoords);
            return Instantiate(_tilePrefab, worldPosition, Quaternion.identity);
        }

        public HexBoardVector2Int TileToHexCoords(Tile tile)
        {
            foreach(HexBoardVector2Int key in _hexCoordsGameObjectPairs.Keys)
            {
                if(_hexCoordsGameObjectPairs[key] == tile)
                {
                    return key;
                }
            }
            return null;
        }

        public void PointerEnterTile(Tile tile)
        {
            _onPointerTile?.GlowHighlight(GlowType.None);
            if(_movableHighlightTile.Contains(_onPointerTile))
            {
                _onPointerTile.GlowHighlight(GlowType.Movable);
            }
            if(_attackableHighlightTile.Contains(_onPointerTile))
            {
                _onPointerTile.GlowHighlight(GlowType.Attackable);
            }
            _onPointerTile = tile;
            _onPointerTile?.GlowHighlight(GlowType.Selection);
        }
        public void ResetHighlight()
        {
            foreach(Tile tile in _movableHighlightTile)
            {
                tile.GlowHighlight(GlowType.None);
            }
            _movableHighlightTile.Clear();
            foreach (Tile tile in _attackableHighlightTile)
            {
                tile.GlowHighlight(GlowType.None);
            }
            _attackableHighlightTile.Clear();
        }
        public void ShowRange(List<HexBoardVector2Int> movableRange, List<HexBoardVector2Int> attackableRange)
        {
            foreach(HexBoardVector2Int movableHexCoords in movableRange)
            {
                Tile tile = _hexCoordsGameObjectPairs[movableHexCoords];
                tile.GlowHighlight(GlowType.Movable);
                _movableHighlightTile.Add(tile);
            }
            foreach (HexBoardVector2Int attackableHexCoords in attackableRange)
            {
                Tile tile = _hexCoordsGameObjectPairs[attackableHexCoords];
                tile.GlowHighlight(GlowType.Attackable);
                _attackableHighlightTile.Add(tile);
            }
        }
    }
}