using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexStarChess.View
{
    public class Tile : MonoBehaviour
    {
        private readonly Dictionary<Renderer, Material[]> _selectionGlowMaterialDictionary = new();
        private readonly Dictionary<Renderer, Material[]> _movableGlowMaterialDictionary = new();
        private readonly Dictionary<Renderer, Material[]> _attackableGlowMaterialDictionary = new();
        private readonly Dictionary<Renderer, Material[]> _originalMaterialDictionary = new();

        private readonly Dictionary<Color, Material> _cachedGlowMaterials = new();

        [SerializeField]
        private Material _selectionGlowMaterial;
        [SerializeField]
        private Material _movableGlowMaterial;
        [SerializeField]
        private Material _attackableGlowMaterial;

        private GlowType GlowType = GlowType.None;

        private void Awake()
        {
            PrepareMaterialDictinaries();
        }

        private void PrepareMaterialDictinaries()
        {
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>(true))
            {
                Material[] originMaterials = renderer.materials;
                _originalMaterialDictionary.Add(renderer, originMaterials);

                Material[] selectionGlowMaterials = new Material[renderer.materials.Length];
                Material[] movableGlowMaterials = new Material[renderer.materials.Length];
                Material[] attackableGlowMaterials = new Material[renderer.materials.Length];

                for (int i = 0; i < originMaterials.Length; i++)
                {
                    if (!_cachedGlowMaterials.TryGetValue(originMaterials[i].color, out Material selectionMaterial))
                    {
                        selectionMaterial = new Material(_selectionGlowMaterial)
                        {
                            color = originMaterials[i].color
                        };
                    }
                    selectionGlowMaterials[i] = selectionMaterial;

                    if (!_cachedGlowMaterials.TryGetValue(originMaterials[i].color, out Material movableMaterial))
                    {
                        movableMaterial = new Material(_movableGlowMaterial)
                        {
                            color = originMaterials[i].color
                        };
                    }
                    movableGlowMaterials[i] = movableMaterial;

                    if (!_cachedGlowMaterials.TryGetValue(originMaterials[i].color, out Material attackableMaterial))
                    {
                        attackableMaterial = new Material(_attackableGlowMaterial)
                        {
                            color = originMaterials[i].color
                        };
                    }
                    attackableGlowMaterials[i] = attackableMaterial;
                }
                _selectionGlowMaterialDictionary.Add(renderer, selectionGlowMaterials);
                _movableGlowMaterialDictionary.Add(renderer, movableGlowMaterials);
                _attackableGlowMaterialDictionary.Add(renderer, attackableGlowMaterials);

            }
        }

        public void GlowHighlight(GlowType glowType)
        {
            if (GlowType == glowType)
            {
                return;
            }
            Dictionary<Renderer, Material[]> changingMaterialDictionary = null;

            switch (glowType)
            {
                case GlowType.None:
                    changingMaterialDictionary = _originalMaterialDictionary;
                    break;
                case GlowType.Selection:
                    changingMaterialDictionary = _selectionGlowMaterialDictionary;
                    break;
                case GlowType.Movable:
                    changingMaterialDictionary = _movableGlowMaterialDictionary;
                    break;
                case GlowType.Attackable:
                    changingMaterialDictionary = _attackableGlowMaterialDictionary;
                    break;
            }
            foreach (Renderer renderer in _originalMaterialDictionary.Keys)
            {
                renderer.materials = changingMaterialDictionary[renderer];
            }
            GlowType = glowType;
        }
    }

    public enum GlowType
    {
        None,
        Selection,
        Movable,
        Attackable

    }
}