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
    public enum GameResult
    {
        WhiteWin,
        BlackWin,
        BlackArmyDestroyed,
        WhiteArmyDestroyed,
        WhiteBaseDestroyed,
        BlackBaseDestroyed,
        NotAGameOver
    }
    public class MapManager
    {
        
        public Map Map { get; set; }

        public MapManager(Map map)
        {
            Map = map;
        }
        public void MoveUnit(int y, int x, IUnitManagable unit)
        {
            Map.SetItem(y, x, unit.TypeOfObject);
            Map.SetItem(unit.Y, unit.X, TypesOfObject.FreeSpace);
            unit.I = y;
            unit.J = x;
        }

        public void SpawnUnitNearBase(TypesOfObject unit)
        {
            var baseTmp = unit == TypesOfObject.UnitWhite ? Map.BaseList.Find(x=>x.TypeOfObject ==TypesOfObject.BaseWhite) : Map.BaseList.Find(x => x.TypeOfObject == TypesOfObject.BaseBlack);
            for (var i = baseTmp.Y - 1; i <= baseTmp.Y + 1; i++)
            {
                for (var j = baseTmp.X - 1; j <= baseTmp.X + 1; j++)
                {
                    if (i != baseTmp.Y && j!=baseTmp.X && (Map.Array[i, j] == TypesOfObject.Food 
                        || Map.Array[i, j] == TypesOfObject.Brick 
                        || Map.Array[i, j] == TypesOfObject.FreeSpace))
                    {
                        Map.SetItem(i, j, unit);
                        Map.AddUnitToArmy(new Unit(i,j,unit,Map));
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
        public void UpdateArmies()
        {
            foreach (var unitManagable in Map.BufferArmy)
            {
                if (Map.Army.Contains(unitManagable))
                {
                    Map.Army.Remove(unitManagable);
                    continue;
                }
                Map.Army.Add(unitManagable);
            }
            Map.BufferArmy.Clear();
        }
        public GameResult CheckForGameOver()
        {
            //if (Map.Army.FindAll(x => x.TypeOfObject == TypesOfObject.UnitBlack).Count == 0)
            //{
            //    return GameResult.BlackArmyDestroyed;
            //}
            //if (Map.Army.FindAll(x => x.TypeOfObject == TypesOfObject.UnitWhite).Count == 0)
            //{
            //    return GameResult.WhiteArmyDestroyed;
            //}
            //foreach (var Base in Map.BaseList)
            //{
            //    if (!Base.GetIsAlive())
            //        return Base.TypeOfObject == TypesOfObject.BaseWhite
            //            ? GameResult.WhiteBaseDestroyed
            //            : GameResult.BlackBaseDestroyed;
            //}
            return GameResult.NotAGameOver;
        }
    }
}
