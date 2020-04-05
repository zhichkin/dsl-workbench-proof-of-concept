namespace OneCSharp.Integrator.Model
{
    public class IntegratorSettings
    {
        public Node[] Nodes { get; set; }
    }
    public class Node
    {
        public string DbServer { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
//{
//    "Nodes" : [
//        {
//            "DbServer" : "",
//            "UserName" : "",
//            "Password" : "",
//            "HttpHost" : ""
//        }
//    ]
//}