using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Game.Models.BaseItems;
using InterfaceLibrary;

namespace Game.Models
{
    public class MapManager
    {

        public delegate void GameOverMessager(string message);
        public event GameOverMessager GameOver = delegate { };
        public Map Map { get; }

        public MapManager(Map map)
        {
            Map = map;
        }
        public void RemoveOldUnitAddNewOne(int y, int x, IUnitManagable unit)
        {
            Map.SetItem(y, x, unit.Color == Colors.White ? TypesOfObject.UnitWhite : TypesOfObject.UnitBlack);
            Map.SetItem(unit.Y, unit.X, TypesOfObject.FreeSpace);
            unit.I = y;
            unit.J = x;
        }

        public void AddNewUnitNearBase(TypesOfObject unit)
        {
            var baseTmp = unit == TypesOfObject.UnitWhite ? Map.BaseWhite : Map.BaseBlack;
            for (var i = baseTmp.Y - 1; i <= baseTmp.Y + 1; i++)
            {
                for (var j = baseTmp.X - 1; j <= baseTmp.X + 1; j++)
                {
                    if (i != j && (Map.Array[i, j] == TypesOfObject.Food || Map.Array[i, j] == TypesOfObject.Brick || Map.Array[i, j] == TypesOfObject.FreeSpace))
                    {
                        Map.SetItem(i, j, unit);
                        Map.AddUnitToArmy(unit, i,j);
                        return;
                    }
                }
            }
        }

        public void UnitDied(IUnitManagable unit)
        {
            Map.SetItem(unit.Y, unit.X, TypesOfObject.FreeSpace);
            Map.RemoveUnitFromArmy(unit);
        }

        public void CheckForGameOver()
        {
            if (Map.BlackArmy.Count == 0 )
            {
                GameOver("Армия черных разбита");
                return;
            }
            if (Map.WhiteArmy.Count == 0)
            {
                GameOver("Армия белых разбита");
                return;
            }
            if (!Map.BaseBlack.GetIsAlive())
            {
                GameOver("База черных разбита");
                return;
            }
            if (!Map.BaseWhite.GetIsAlive())
            {
                GameOver("База белых разбита");
            }
        }
    }
}
