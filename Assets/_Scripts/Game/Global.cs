using QFramework;

namespace ProjectIndieFarm
{
    public class Global
    {
        /// <summary>
        /// 默认第一天
        /// </summary>
        public static BindableProperty<int> Days = new BindableProperty<int>(1);

        /// <summary>
        /// 收获数目
        /// </summary>
        public static BindableProperty<int> FruitCnt = new BindableProperty<int>(0);


    }

}
