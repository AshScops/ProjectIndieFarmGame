using UnityEngine;
using QFramework;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Linq;

namespace ProjectIndieFarm
{
	public partial class Player : ViewController
	{
		public Grid Grid;
		public Tilemap Tilemap;

		void Start()
		{
            Global.Days.Register((day) =>
            {
                var gridController = FindObjectOfType<GridController>();
                var virtualGrid = gridController.ShowGrid;

                //
                var smallPlants = SceneManager.GetActiveScene()
                    .GetRootGameObjects()
                    .Where((gameObj) =>
                    {
                        return gameObj.name.StartsWith("SmallPlant");
                    });

                foreach (var smallPlant in smallPlants)
                {
                    var cellPos = Grid.WorldToCell(smallPlant.transform.position);
                    var cellData = virtualGrid[cellPos.x, cellPos.y];
                    if (cellData != null && cellData.Watered)
                    {
                        ResController.Instance.RipePrefab
                            .Instantiate()
                            .Position(smallPlant.transform.position);

                        smallPlant.DestroySelf();
                    }
                }

                //
                var seeds = SceneManager.GetActiveScene()
                    .GetRootGameObjects()
                    .Where((gameObj) =>
                    {
                        return gameObj.name.StartsWith("Seed");
                    });

                foreach (var seed in seeds)
                {
                    var cellPos = Grid.WorldToCell(seed.transform.position);
                    var cellData = virtualGrid[cellPos.x, cellPos.y];
                    if (cellData != null && cellData.Watered)
                    {
                        ResController.Instance.SmallPlantPrefab
                            .Instantiate()
                            .Position(seed.transform.position);

                        seed.DestroySelf();
                    }
                }

                //水
                var waters = SceneManager.GetActiveScene()
                    .GetRootGameObjects()
                    .Where((gameObj) =>
                    {
                        return gameObj.name.StartsWith("Water");
                    });

                foreach (var water in waters)
                {
                    water.DestroySelf();
                }


                virtualGrid.ForEach((x, y, data) =>
                {
                    if (data != null)
                        data.Watered = false;
                });


            }).UnRegisterWhenGameObjectDestroyed(this);

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


            if(Input.GetKeyDown(KeyCode.E))
            {
                //已开垦
                if (virtualGrid[cellPos.x, cellPos.y] != null)
                {
                    if(!virtualGrid[cellPos.x, cellPos.y].Watered)
                    {
                        //浇水
                        ResController.Instance.WaterPrefab
                            .Instantiate()
                            .Position(tileWorldPos);

                        virtualGrid[cellPos.x, cellPos.y].Watered = true;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Global.Days.Value++;

            }
        }


        private void OnGUI()
        {
            IMGUIHelper.SetDesignResolution(640, 360);
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("天数：" + Global.Days.Value);
            GUILayout.EndHorizontal();
        }
    }
}
