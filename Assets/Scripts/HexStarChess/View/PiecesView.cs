using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HexStarChess.View
{
    public class PiecesView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _blueKing;
        [SerializeField]
        private GameObject _blueQueen;
        [SerializeField]
        private GameObject _blueRook;
        [SerializeField]
        private GameObject _blueBishop;
        [SerializeField]
        private GameObject _blueKnight;
        [SerializeField]
        private GameObject _bluePawn;

        [SerializeField]
        private GameObject _redKing;
        [SerializeField]
        private GameObject _redQueen;
        [SerializeField]
        private GameObject _redRook;
        [SerializeField]
        private GameObject _redBishop;
        [SerializeField]
        private GameObject _redKnight;
        [SerializeField]
        private GameObject _redPawn;
        
        public GameObject CreatePiece(PieceType pieceType, bool isUpForward, Vector3 position)
        {
            switch (pieceType)
            {
                case PieceType.King:
                    return Instantiate(isUpForward ? _blueKing : _redKing, position, Quaternion.identity);
                case PieceType.Queen:
                    return Instantiate(isUpForward ? _blueQueen : _redQueen, position, Quaternion.identity);
                case PieceType.Rook:
                    return Instantiate(isUpForward ? _blueRook : _redRook, position, Quaternion.identity);
                case PieceType.Bishop:
                    return Instantiate(isUpForward ? _blueBishop : _redBishop, position, Quaternion.identity);
                case PieceType.Knight:
                    return Instantiate(isUpForward ? _blueKnight : _redKnight, position, Quaternion.identity);
                case PieceType.Pawn:
                    return Instantiate(isUpForward ? _bluePawn : _redPawn, position, Quaternion.identity);
                default:
                    break;
            }

            return null;
        }
    }
}