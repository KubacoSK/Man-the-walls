using UnityEngine;
using UnityEngine.Tilemaps;

public class TileHighlighter : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap highlightTilemap;
    public TileBase highlightTile; // The tile to use for highlighting

    private Vector3Int lastPosition = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

    private void Update()
    {
        // Calculate the world position of the tile under the mouse.
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

        if (cellPosition != lastPosition)
        {
            // Clear the previous highlight
            ClearLastHighlight();

            // Check if the mouse position is over a valid tile in the tilemap
            if (tilemap.HasTile(cellPosition))
            {
                // Place the highlight tile on the new tilemap
                highlightTilemap.SetTile(cellPosition, highlightTile);
                lastPosition = cellPosition;
            }
        }
    }

    private void OnMouseExit()
    {
        // Clear the highlight when the mouse exits the tile
        ClearLastHighlight();
    }

    private void ClearLastHighlight()
    {
        // Check if the last tile was highlighted and remove the highlight
        if (highlightTilemap.HasTile(lastPosition))
        {
            highlightTilemap.SetTile(lastPosition, null);
        }
    }
}