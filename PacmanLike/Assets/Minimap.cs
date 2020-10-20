using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [SerializeField] Transform _grid;
    [SerializeField] Image _image;

    // マップの色
    [SerializeField] Color _wallColor;
    [SerializeField] Color _groundColor;
    [SerializeField] Color _noneColor;

    // マップ用テクスチャ
    Texture2D _texture;



    void Start()
    {
        Tilemap groundTilemap = _grid.Find("Ground").GetComponent<Tilemap>();
        Tilemap wallTilemap = _grid.Find("Wall").GetComponent<Tilemap>();

        // テクスチャ作成
        Vector3Int size = wallTilemap.size;
        _texture = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);

        // こうしないと、画像がぼやける
        _texture.filterMode = FilterMode.Point;

        Vector3Int origin = wallTilemap.origin;

        // テクスチャ座標ごとの色を求める
        for (int y = 0; y < size.y; ++y)
        {
            for (int x = 0; x < size.x; ++x)
            {
                // Tilemapのグリッド座標
                Vector3Int cellPos = new Vector3Int(origin.x + x, origin.y + y, 0);

                // 壁タイルが存在する
                if (wallTilemap.GetTile(cellPos) != null)
                {
                    _texture.SetPixel(x, y, _wallColor);
                }
                // 地面タイルが存在する
                else if (groundTilemap.GetTile(cellPos) != null)
                {
                    _texture.SetPixel(x, y, _groundColor);
                }
                // なにもない場所
                else
                {
                    _texture.SetPixel(x, y, _noneColor);
                }
            }
        }

        // テクスチャ確定
        _texture.Apply();

        // テクスチャをImageに適用
        _image.rectTransform.sizeDelta = new Vector2(size.x, size.y);
        _image.sprite = Sprite.Create(_texture, new Rect(0, 0, size.x, size.y), Vector2.zero);

        // _imageをGridの中心に移動
        Vector2 leftDownWorldPos = wallTilemap.CellToWorld(origin);
        Vector2 rightUpWorldPos = wallTilemap.CellToWorld(origin + size);
        _image.transform.position = (leftDownWorldPos + rightUpWorldPos) * 0.5f;
    }

    private void OnDestroy()
    {
        Destroy(_texture);
    }
}