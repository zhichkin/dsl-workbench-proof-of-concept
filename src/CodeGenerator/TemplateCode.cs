using OneCSharp.DDL.Attributes;

namespace Zhichkin.OneCSharp
{
	[Entity(TableName = "MyTable")] public sealed class MyTestClass : ReferenceObject
	{
		[Field(Name = "MyField")] public string MyName { get; set; }
	}
}