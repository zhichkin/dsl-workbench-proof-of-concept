using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneCSharp.Core;

namespace OneCSharp.Tests
{
    [TestClass]
    public class UnitTest_CoreModel
    {
        [TestMethod] public void AttachNamespaceToDomain1()
        {
            Domain domain = new Domain() { Name = "domain" };
            Namespace root = new Namespace() { Name = "root" };
            domain.Add(root);
            Assert.IsTrue(domain.Namespaces.Count > 0);
            Assert.IsTrue(root.Domain == domain);
            Assert.IsTrue(root.Parent == null);
        }
        [TestMethod] public void AttachNamespaceToDomain2()
        {
            Domain domain = new Domain() { Name = "domain" };
            Namespace root = domain.AddNamespace("root");
            Assert.IsTrue(domain.Namespaces.Count > 0);
            Assert.IsTrue(root.Domain == domain);
            Assert.IsTrue(root.Parent == null);
        }
        [TestMethod] public void AttachNamespaceToDomain3()
        {
            Domain domain = new Domain() { Name = "domain" };
            Namespace root = domain.AddNamespace("root");
            Namespace child = new Namespace() { Name = "child" };
            root.Add(child);
            Assert.IsTrue(root.Children.Count > 0);
            Assert.IsTrue(root.Domain == domain);
            Assert.IsTrue(root.Parent == null);
            Assert.IsTrue(child.Domain == domain);
            Assert.IsTrue(child.Parent == root);
            
            domain.Add(child);
            Assert.IsTrue(domain.Namespaces.Count == 2);
            root.Children.Remove(child);
            Assert.IsTrue(root.Children.Count == 0);
            Assert.IsTrue(child.Domain == domain);
            Assert.IsTrue(child.Parent == null);
        }
    }
}