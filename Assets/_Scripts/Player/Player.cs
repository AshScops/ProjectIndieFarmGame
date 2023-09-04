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
			if(Input.GetKeyDown(KeyCode.Space))
			{
				//获取xy值
				var x = transform.position.x;
				var y = transform.position.y;

				//获取对应块
				var cellPos = Grid.WorldToCell(transform.position);

                //替换对应块
				Tilemap.SetTile(cellPos, null);



            }
		}

	}
}
