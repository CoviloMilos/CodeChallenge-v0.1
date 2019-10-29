using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using CodeCompilerAPI.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;


namespace CodeCompilerAPI.Services
{
    public class CompileService : ICompileService
    {
        
        //example of request body
        /*" public static int[] sortArray(int[] arr){int temp; for (int i = 0; i < arr.Length - 1; i++) { for (int j = i + 1; j < arr.Length; j++) { if (arr[i] > arr[j]) { temp = arr[i]; arr[i] = arr[j]; arr[j] = temp; } } } return arr;}" 
            public bool checkNum(int n) { if (n%2==0) { return true; }else { return false; }}
            public static int[] reverseArray(int[] arr) { Array.Reverse(arr); return arr; }
            public static string toLowerToUpper(string el) { if(el.Length < 4) { return el.ToUpper(); } else { return el.Replace(el.Substring(0,4), el.Substring(0,4).ToLower()); } }*/
        public Models.Task CompileCode(string inputCode, Models.Task task)
        {
            
            string code = @"    using System;
                                namespace CodeCompilerAPI
                                {
                                    public class Compile
                                    {";
            code += inputCode + "}}";

            //Produces a syntax tree by parsing the source text.
            var tree = SyntaxFactory.ParseSyntaxTree(code);

            //name of file which will be used to store code
            string fileName = "Temp" + DateTime.Now.ToString("dddd-dd-MMMM-yyyy-HH-mm-ss") + ".dll";
            string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            var systemRefLocation = typeof(object).GetTypeInfo().Assembly.Location;
            var systemReference =  MetadataReference.CreateFromFile(systemRefLocation);

            var compilation = CSharpCompilation.Create(fileName)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(systemReference)
                .AddSyntaxTrees(tree);


            EmitResult compilationResult = compilation.Emit(path);
            
            
            if(compilationResult.Success)
            {
                Assembly asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
                
                foreach (var testCase in task.Cases)
                {
                    try{

                        System.Object result = executeCode(asm, testCase.FirstInputParameter, testCase.SecondInputParameter, task.MethodName, task.ReturnDataType, task.FirstInputParameterDataType, task.SecondInputParameterDataType);
                    
                        switch (task.ReturnDataType)
                        {
                            case "bool":
                                if (bool.Parse(testCase.ValidReturnValue)  == (bool) result) 
                                    testCase.CaseResult = true;
                                else
                                    testCase.CaseResult = false;
                                break;

                            case "array":
                                var tempValidReturnValue = testCase.ValidReturnValue.Split(",").Select(x => Int32.Parse(x)).ToArray();
                                if (tempValidReturnValue.SequenceEqual(((IEnumerable)result).Cast<int>()) == true)
                                    testCase.CaseResult = true; 
                                break;
                            
                            case "string":
                                var caseResult = string.Compare(testCase.ValidReturnValue, result.ToString());
                                testCase.CaseResult = caseResult == 0 ? true : false;
                                break;

                        }


                        string info = $"Test case num: {testCase.CaseNum.ToString()} \nTest case valid result: {testCase.ValidReturnValue.ToString()} \nActual test case result: {result.ToString()}";
                        Console.WriteLine(info);


                    }catch( Exception e) {
                        var helpLinks = e.HelpLink;
                        string issue = $"Message: {e.GetBaseException().ToString()}, \nHelp link: {e.HelpLink}";
                        Console.WriteLine(issue);
                    }
                }
                DeleteFile(fileName);
                return task;
            }
            else
            {
                foreach (Diagnostic codeIssue in compilationResult.Diagnostics)
                {
                    string issue = $"ID: {codeIssue.Id}, Message: {codeIssue.GetMessage()}, Location: {codeIssue.Location.GetLineSpan()}, Severity: {codeIssue.Severity}";
                    Console.WriteLine(issue);
                }
                DeleteFile(fileName);
                return null;
            }
        }

        private void DeleteFile(string fileName) {

            var dir = new DirectoryInfo(Directory.GetCurrentDirectory().ToString());

            foreach (var file in dir.EnumerateFiles("Temp*.dll")) {

                if (file.Name != fileName)
                    file.Delete();
            }
        }

        private System.Object executeCode(Assembly asm, string firstInputPara, string secondInputPara, string methodname, string returnDataType, string firstInputParaDataType, string secondInputParaDataType) 
        {

            if (secondInputPara == null) 
                return asm.GetType("CodeCompilerAPI.Compile").GetMethod(methodname).Invoke(null, new object[] { parseInputParameter(firstInputParaDataType, firstInputPara) });
            else if (secondInputPara != null)
                return asm.GetType("CodeCompilerAPI.Compile").GetMethod(methodname).Invoke(null, new object[] { parseInputParameter(firstInputParaDataType, firstInputPara), parseInputParameter(secondInputParaDataType, secondInputPara)});
            
            return null;
        }

        private System.Object parseInputParameter(string dataType, string inputParameter)
        {
            Object tempObj = new Object();
            switch (dataType)
            {
                case "int":
                    tempObj = Int32.Parse(inputParameter);
                    break;
                case "array":
                    tempObj = new int[inputParameter.Split(",").Length];
                    tempObj = Array.ConvertAll(inputParameter.Split(","), int.Parse);
                    break;
                case "bool":
                    tempObj = Boolean.Parse(inputParameter);
                    break;
                case "string":
                    tempObj = (String) inputParameter;
                    break;
                default:
                    tempObj = null;
                    break;
            }

            return tempObj;
        }
    }
}