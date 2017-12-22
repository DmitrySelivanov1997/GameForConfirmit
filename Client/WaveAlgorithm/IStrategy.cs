using InterfaceLibrary;

namespace WaveAlgorithm
{
    public interface IStrategy
    {
        IItem [,] Map { get; set; }
        Direction FindUnitDirection(IUnit unit);
    }
}
