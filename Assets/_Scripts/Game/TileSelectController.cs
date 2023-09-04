using UnityEngine;
using QFramework;

namespace ProjectIndieFarm
{
	public partial class TileSelectController : ViewController, ISingleton
	{
		public static TileSelectController Instance => SingletonProperty<TileSelectController>.Instance;

        public void OnSingletonInit()
		{
			
		}

	}
}
