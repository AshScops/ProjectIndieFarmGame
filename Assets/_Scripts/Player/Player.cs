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
                var virtualGrid = gridController.SoilGrids;

                PlantController.Instance.Plants.ForEach((x, y, plant) =>
                {
                    if(plant is not null)
                    {
                        var cellData = virtualGrid[x, y];
                        if (cellData is not null && cellData.Watered && plant.State != PlantState.Ripe)
                        {
                            plant.SetState(plant.State+1);
                            Debug.Log("plant.State" + plant.State);
                        }
                    }
                });

                //ˮ
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
            //��ȡ��Ӧ��
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

            //�滻��Ӧ��
            var gridController = FindObjectOfType<GridController>();
            var virtualGrid = gridController.SoilGrids;

            if (Input.GetMouseButtonDown(0))
			{
                //δ����
				if (virtualGrid[cellPos.x, cellPos.y] == null)
				{
                    //����
                    virtualGrid[cellPos.x, cellPos.y] = new SoilData();
                    Tilemap.SetTile(cellPos, gridController.Pen);
                }
                //�ѿ���δ��ֲ
                else if (!virtualGrid[cellPos.x, cellPos.y].HasPlant)
                {
                    //��ֲ
                    var plantGameObj = ResController.Instance.PlantPrefab
                        .Instantiate()
                        .Position(tileWorldPos);

                    var plant = plantGameObj.GetComponent<Plant>();
                    plant.XCell = cellPos.x;
                    plant.YCell = cellPos.y;
                    PlantController.Instance.Plants[cellPos.x, cellPos.y] = plant;

                    virtualGrid[cellPos.x, cellPos.y].HasPlant = true;
                }
                else
                {
                    //�ջ�
                    if(virtualGrid[cellPos.x, cellPos.y].PlantState == PlantState.Ripe)
                    {
                        //PlantController.Instance.Plants[cellPos.x, cellPos.y].SetState(PlantState.Old);
                        Destroy(PlantController.Instance.Plants[cellPos.x, cellPos.y].gameObject);
                        virtualGrid[cellPos.x, cellPos.y].HasPlant = false;
                        Global.FruitCnt.Value ++;
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (virtualGrid[cellPos.x, cellPos.y] is not null)
                {
                    virtualGrid[cellPos.x, cellPos.y] = null;
                    Tilemap.SetTile(cellPos, null);
                }
            }


            if(Input.GetKeyDown(KeyCode.E))
            {
                //�ѿ���
                if (virtualGrid[cellPos.x, cellPos.y] != null)
                {
                    if(!virtualGrid[cellPos.x, cellPos.y].Watered)
                    {
                        //��ˮ
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

            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("GamePass");
            }
        }


        private void OnGUI()
        {
            IMGUIHelper.SetDesignResolution(640, 360);
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("������" + Global.Days.Value);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("�ջ���Ŀ��" + Global.FruitCnt.Value);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("��ˮ��E");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("��һ�죺F");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("���ѡ���ˮ���ջ�������");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("�Ƴ�������Ҽ�");
            GUILayout.EndHorizontal();
        }
    }
}
