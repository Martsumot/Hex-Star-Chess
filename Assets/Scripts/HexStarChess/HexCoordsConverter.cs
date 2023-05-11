using UnityEngine;

namespace HexStarChess
{
    public class HexCoordsConverter
    {
        private static readonly float _xOffset = 1.5f, _yOffset = 1.73f;
        public static Vector3 ConvertWorldPosition(HexBoardVector2Int hexCoords, float z = 0)
        {
            float x = hexCoords.x * _xOffset;
            float y = hexCoords.y * _yOffset;
            if(hexCoords.x % 2 == 1)
            {
                y += _yOffset / 2;
            }

            return new(x, y, z);
        }

        public static HexBoardVector2Int ConvertHexCoords(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt(worldPosition.x / _xOffset);
            int y = Mathf.RoundToInt((x % 2 == 0) ? (worldPosition.y / _yOffset) : (worldPosition.y - _yOffset / 2) / _yOffset);

            return new(x, y);
        }
    }
}
