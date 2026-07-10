using System;
using System.Reflection;

internal class Class1
{
	internal delegate void Delegate0(object o);

	internal static Module eibbDcEcC;

	static Class1()
	{
		eibbDcEcC = typeof(global::Class1).Assembly.ManifestModule;
	}

	internal static void zNv24UMMsIZ8c(int typemdt)
	{
		Type type = eibbDcEcC.ResolveType(33554432 + typemdt);
		FieldInfo[] fields = type.GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			MethodInfo method = (MethodInfo)eibbDcEcC.ResolveMethod(fieldInfo.MetadataToken + 100663296);
			fieldInfo.SetValue(null, (MulticastDelegate)Delegate.CreateDelegate(type, method));
		}
	}
}
