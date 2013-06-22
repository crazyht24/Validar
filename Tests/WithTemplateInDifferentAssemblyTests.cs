﻿using System.IO;
using System.Reflection;
using FluentValidation;
using NUnit.Framework;

[TestFixture]
public class WithTemplateInDifferentAssemblyTests
{

    string beforeAssemblyPath;
    string afterAssemblyPath;
    Assembly assembly;

    public WithTemplateInDifferentAssemblyTests()
    {
        var type1 = typeof (IValidator);
        var type2 = typeof(ValidationTemplate);
        
        AppDomainAssemblyFinder.Attach();
        beforeAssemblyPath = Path.GetFullPath(@"..\..\..\AssemblyWithNoValidationTemplate\bin\Debug\AssemblyWithNoValidationTemplate.dll");
#if (!DEBUG)
        beforeAssemblyPath = beforeAssemblyPath.Replace("Debug", "Release");
#endif
        afterAssemblyPath = WeaverHelper.Weave(beforeAssemblyPath);
        assembly = Assembly.LoadFile(afterAssemblyPath);
    }

    [Test]
    public void DataErrorInfo()
    {

            var instance = assembly.GetInstance("Person");

            ValidationTester.TestDataErrorInfo(instance);
    }

    [Test]
    public void NotifyDataErrorInfo()
    {
        var instance = assembly.GetInstance("Person");
        ValidationTester.TestNotifyDataErrorInfo(instance);
    }
    [Test]
    public void PeVerify()
    {
        Verifier.Verify(afterAssemblyPath);
    }

}