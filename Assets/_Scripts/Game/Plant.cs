using UnityEngine;
using QFramework;

namespace ProjectIndieFarm
{
	public partial class Plant : ViewController
	{
		public int XCell;
		public int YCell;

		private PlantState m_state = PlantState.Seed;

        public PlantState State => m_state;
		public void SetState(PlantState newState)
		{
			if(m_state != newState)
			{
                m_state = newState;

                //切换表现
                if (m_state == PlantState.Seed)
                    GetComponent<SpriteRenderer>().sprite = ResController.Instance.SeedSprite;
                else if(m_state == PlantState.Small)
					GetComponent<SpriteRenderer>().sprite = ResController.Instance.SmallPlantSprite;
				else if(m_state == PlantState.Ripe)
                    GetComponent<SpriteRenderer>().sprite = ResController.Instance.RipeSprite;

                //同步到SoilData
                FindObjectOfType<GridController>().SoilGrids[XCell, YCell].PlantState = m_state;

            }

		}

		void Start()
		{
			// Code Here
		}
	}
}
