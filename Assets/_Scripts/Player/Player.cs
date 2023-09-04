using UnityEngine;
using QFramework;
using UnityEngine.Tilemaps;

namespace ProjectIndieFarm
{
	public partial class Player : ViewController
	{
		public Grid Grid;
		public Tilemap Tilemap;

		void Start()
		{
		}

		private void Update()
		{
            //获取对应块
            var cellPos = Grid.WorldToCell(transform.position);
            var tileWorldPos = Grid.CellToWorld(cellPos);
            tileWorldPos.x += Grid.cellSize.x * 0.5f;
            tileWorldPos.y += Grid.cellSize.y * 0.5f;

            if (cellPos.x < 0 || cellPos.y < 0 || cellPos.x >= 10 || cellPos.y >= 10)
            {
                TileSelectController.Instance.Hide();
                return;
            }
            else
            {
                TileSelectController.Instance.Position(tileWorldPos);
                TileSelectController.Instance.Show();
            }

            //替换对应块
            var gridController = FindObjectOfType<GridController>();
            var virtualGrid = gridController.ShowGrid;

            if (Input.GetMouseButtonDown(0))
			{
                //未开垦
				if (virtualGrid[cellPos.x, cellPos.y] == null)
				{
                    //开垦
                    virtualGrid[cellPos.x, cellPos.y] = new SoilData();
                    Tilemap.SetTile(cellPos, gridController.Pen);
                }
                //已开垦未种植
                else if (!virtualGrid[cellPos.x, cellPos.y].HasPlant)
                {
                    //种植
                    ResController.Instance.SeedPrefab
                        .Instantiate()
                        .Position(tileWorldPos);

                    virtualGrid[cellPos.x, cellPos.y].HasPlant = true;
                }
                else
                {

                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (virtualGrid[cellPos.x, cellPos.y] != null)
                {
                    virtualGrid[cellPos.x, cellPos.y] = null;
                    Tilemap.SetTile(cellPos, null);
                }
            }
        }

	}
}
