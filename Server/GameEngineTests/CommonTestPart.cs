using CommonClient_WebServiseParts;
using InterfaceLibrary;

namespace GameEngineTests
{
    class CommonTestPart
    {

        public static void AddUnitOnMap(int y, int x, Map map, TypesOfObject obj)
        {
            map.SetItem(y, x, obj);
            map.Army.Add(new Unit(y, x, obj, map));
        }

        public static Map GenerateMap10X10WithFreeSpace()
        {
            var map = new Map(10);
            for (int i = 0; i < map.GetLength(); i++)
            {
                for (int j = 0; j < map.GetLength(); j++)
                {
                    map.SetItem(i, j, TypesOfObject.FreeSpace);
                }
            }
            return map;
        }
    }
}
