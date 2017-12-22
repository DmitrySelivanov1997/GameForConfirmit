using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using InterfaceLibrary;
using WaveAlgorithm;

namespace WebGameService.Models
{
    public static class AlgorithmContainer
    {
        public static IAlgorithm AlgorithmWhite { get; set; } =new Algoritm2();
        public static IAlgorithm AlgorithmBlack { get; set; } = new Algoritm2(); 
        public static Assembly AlgoritmWhiteAssembly { get; set; }
        public static Assembly AlgoritmBlackAssembly { get; set; }
    }
}