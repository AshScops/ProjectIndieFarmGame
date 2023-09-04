using UnityEngine;
using QFramework;

namespace ProjectIndieFarm
{
	public enum PlantState
	{
		Seed = 0,
		Small,
		Ripe
	}

	public partial class PlantController : ViewController, ISingleton
	{
		public static PlantController Instance => SingletonProperty<PlantController>.Instance;

        public EasyGrid<Plant> Plants = new EasyGrid<Plant>(10, 10);



		public void OnSingletonInit()
		{
			
		}

		void Start()
		{
			// Code Here
		}
	}
}
