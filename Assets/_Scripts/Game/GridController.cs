using UnityEngine;
using QFramework;
using UnityEngine.Tilemaps;

namespace ProjectIndieFarm
{
	public partial class GridController : ViewController
	{
        private EasyGrid<SoilData> m_showGrid = new EasyGrid<SoilData>(10, 10);
        public EasyGrid<SoilData> ShowGrid => m_showGrid;
        public TileBase Pen;

        void Start()
		{
            m_showGrid[0, 0] = new SoilData();
            m_showGrid[2, 2] = new SoilData();


            m_showGrid.ForEach((x, y, data) =>
            {
                if(data != null)
                    Tilemap.SetTile(new Vector3Int(x, y), Pen);
            });

        }
	}
}
