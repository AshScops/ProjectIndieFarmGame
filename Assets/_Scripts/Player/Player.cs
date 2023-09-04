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
				//��ȡxyֵ
				var x = transform.position.x;
				var y = transform.position.y;

				//��ȡ��Ӧ��
				var cellPos = Grid.WorldToCell(transform.position);

                //�滻��Ӧ��
				Tilemap.SetTile(cellPos, null);



            }
		}

	}
}
