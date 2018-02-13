using System.Reflection;
using InterfaceLibrary;
using WaveAlgorithm;

namespace WebGameService.Models
{
    public static class AlgorithmContainer
    {
        public static IAlgorithm AlgorithmWhite { get; set; } =new Algoritm2();
        public static IAlgorithm AlgorithmBlack { get; set; } = new Algoritm2(); 
    }
}