using InterfaceLibrary;

namespace CommonClient_WebServiseParts
{
    public interface IUnitManagable:IUnit
    {
        int I { get; set; }
        int J { get; set; }
    }
}
