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

			if(Input.GetMouseButtonDown(0))
			{
                //��ȡ��Ӧ��
                var cellPos = Grid.WorldToCell(transform.position);
                if (cellPos.x < 0 || cellPos.y < 0 || cellPos.x >= 10 || cellPos.y >= 10) return;

				//�滻��Ӧ��
				var gridController = FindObjectOfType<GridController>();
                var virtualgrid = gridController.ShowGrid;

                //δ����
				if (virtualgrid[cellPos.x, cellPos.y] == null)
				{
                    //����
                    virtualgrid[cellPos.x, cellPos.y] = new SoilData();
                    Tilemap.SetTile(cellPos, gridController.Pen);
                }
                //�ѿ���δ��ֲ
                else if (!virtualgrid[cellPos.x, cellPos.y].HasPlant)
                {
                    var tileWorldPos = Grid.CellToWorld(cellPos);
                    tileWorldPos.x += Grid.cellSize.x * 0.5f;
                    tileWorldPos.y += Grid.cellSize.y * 0.5f;

                    //��ֲ
                    ResController.Instance.SeedPrefab
                        .Instantiate()
                        .Position(tileWorldPos);

                    virtualgrid[cellPos.x, cellPos.y].HasPlant = true;
                }
                else
                {

                }

            }

            if (Input.GetMouseButtonDown(1))
            {
                //��ȡ��Ӧ��
                var cellPos = Grid.WorldToCell(transform.position);
                if (cellPos.x < 0 || cellPos.y < 0 || cellPos.x >= 10 || cellPos.y >= 10) return;

                //�滻��Ӧ��
                var gridController = FindObjectOfType<GridController>();
                var grid = gridController.ShowGrid;
                if (grid[cellPos.x, cellPos.y] != null)
                {
                    grid[cellPos.x, cellPos.y] = null;
                    Tilemap.SetTile(cellPos, null);
                }
            }
        }

	}
}
