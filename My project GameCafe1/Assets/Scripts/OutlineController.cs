using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Targetable))]
public class OutlineController : MonoBehaviour
{
    private static Material outlineMaterial;
    private Material[] originalMaterials;
    private Material[] modifiedMaterials;
    private Renderer objectRenderer;
    private Targetable targetable;
    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterials = objectRenderer.materials;
        targetable = GetComponent<Targetable>();

        // Initialize shared material if not already set
        if (outlineMaterial == null)
        {
            outlineMaterial = Resources.Load<Material>("Materials/OutlineMaterial");
            if (outlineMaterial == null)
            {
                Debug.LogError("Outline material not found in Resources!");
            }
        }
        // Initialize modifiedMaterials
        modifiedMaterials = new Material[originalMaterials.Length + 1];
        System.Array.Copy(originalMaterials, modifiedMaterials, originalMaterials.Length);
        modifiedMaterials[modifiedMaterials.Length - 1] = outlineMaterial;
    }
    private void Start()
    {
        targetable.OnTargetableEvent += ShowOutline;
        targetable.OnExitTargtableEvent += HideOutline;
    }
    public void ShowOutline()
    {
        objectRenderer.materials = modifiedMaterials;
    }

    public void HideOutline()
    {
        objectRenderer.materials = originalMaterials;
    }
}
