using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneCSharp.Core.Model;

namespace OneCSharp.Tests
{
    [TestClass]
    public class UnitTest_CoreModel
    {
        [TestMethod] public void AttachNamespaceToDomain1()
        {
            //Domain domain = new Domain() { Name = "domain" };
            //Namespace root = new Namespace() { Name = "root" };
            //domain.AddChild(root);
            //Assert.IsTrue(domain.Namespaces.Count > 0);
            //Assert.IsTrue(root.Domain == domain);
            //Assert.IsTrue(root.Owner == null);
        }
        [TestMethod] public void AttachNamespaceToDomain2()
        {
            //Domain domain = new Domain() { Name = "domain" };
            //Namespace child = new Namespace() { Name = "root" };
            //domain.AddChild(child);
            //Assert.IsTrue(domain.Namespaces.Count > 0);
            //Assert.IsTrue(child.Domain == domain);
            //Assert.IsTrue(child.Owner == null);
        }
        [TestMethod] public void AttachNamespaceToDomain3()
        {
            //Domain domain = new Domain() { Name = "domain" };
            //Namespace root = new Namespace() { Name = "root" };
            //domain.AddChild(root);
            //Namespace child = new Namespace() { Name = "child" };
            //root.AddChild(child);
            //Assert.IsTrue(root.Namespaces.Count > 0);
            //Assert.IsTrue(root.Domain == domain);
            //Assert.IsTrue(root.Owner == null);
            //Assert.IsTrue(child.Domain == domain);
            //Assert.IsTrue(child.Owner == root);
            
            //domain.AddChild(child);
            //Assert.IsTrue(domain.Namespaces.Count == 2);
            //root.Namespaces.Remove(child);
            //Assert.IsTrue(root.Namespaces.Count == 0);
            //Assert.IsTrue(child.Domain == domain);
            //Assert.IsTrue(child.Owner == null);
        }
    }
}