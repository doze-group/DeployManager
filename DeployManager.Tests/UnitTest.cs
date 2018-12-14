using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeployManager.Entities;
using System.Diagnostics;

namespace DeployManager.Tests
{
    [TestClass]
    public class UnitTest
    {
        Controller controller;
        AppExec test_item;
        

        public UnitTest()
        {
            controller = new Controller();
            test_item = new AppExec()
            {
                Name = "Github",
                Path = "D:/sedeB/SEM3/shell.bat"
            };
        }


        public AppExec item_validation(AppExec appexec)
        {
            Assert.IsNotNull(appexec);
            Assert.IsInstanceOfType(appexec, typeof(AppExec));
            return appexec;
        }
        [TestMethod]
        public void TestCreate()
        {
            Assert.IsTrue(controller.Create(item_validation(test_item)));
          
            Trace.WriteLine("after line");
            //Console.ReadLine();
          
        }
        [TestMethod]
        public void TestDelete()
        {
            TestCreate();
            int index = 0;
            Assert.IsInstanceOfType(index, typeof(int));
            Assert.IsTrue(controller.Delete(index));
        }
        [TestMethod]
        public void TestUpdate()
        {
            TestCreate();
            int index = 0;
            Assert.IsInstanceOfType(index, typeof(int));
            Assert.IsTrue(controller.Update(item_validation(test_item), index));
        }
        [TestMethod]
        public void TestFind()
        {
            TestCreate();
            int index = 0;
            Assert.IsInstanceOfType(index, typeof(int));
            Assert.IsInstanceOfType(controller.FindItem(test_item.Id), typeof(AppExec));
        }

    }
}
