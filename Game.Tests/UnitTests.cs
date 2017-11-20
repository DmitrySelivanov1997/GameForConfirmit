using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Game.Models;
using Game.Models.BaseItems;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Game.Tests
{
    [TestClass]
    public class UnitTests
    {
        public Engine Engine;
        public WpfPrinter Printer;
        public Map MyMap;
        public WriteableBitmap WriteableBitmap;
        [TestMethod]
        public void UnitMovesByDirection()
        {
        }
    }
}
