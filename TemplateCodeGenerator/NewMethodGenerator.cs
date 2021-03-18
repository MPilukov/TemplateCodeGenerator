using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TemplateCodeGenerator
{
    public class NewMethodGenerator
    {
        public static void Generate(string controllerPath, string methodName, bool isQuery, string description)
        {
            var pathParts = controllerPath.Split("\\");
            var slnName = "";
            var rootFolder = "";
            
            for (var i = 0; i < pathParts.Length; i++)
            {
                if (pathParts[i].Equals("Controllers"))
                {
                    var apiSln = pathParts[i - 1];
                    slnName = apiSln.Replace(".Api", "");
                    rootFolder = controllerPath.Substring(0, controllerPath.IndexOf(apiSln));
                }
            }

            var methodNameWithoutAsync = methodName.EndsWith("Async") ? methodName.Remove(methodName.Length - 5, 5) : methodName;

            Generate(methodNameWithoutAsync, controllerPath, description, isQuery, slnName, rootFolder);
        }

        private static void Generate(string methodName, string controllerPath, string description, bool isQuery, string slnName, string rootFolder)
        {
            var controllerText = File.ReadAllText(controllerPath, Encoding.UTF8);

            var postfixNamespace = GetPostfixNamespace(controllerText);
            var postfixNamespaceArray = postfixNamespace.Split(".").ToList();
            postfixNamespaceArray.Add(methodName);

            var appBaseFolder = $"{rootFolder}{slnName}.Application";
            var appNamespace = $"{slnName}.Application";
            
            foreach (var postfixNamespacePart in postfixNamespaceArray)
            {
                appBaseFolder = $"{appBaseFolder}\\{postfixNamespacePart}";
                appNamespace = $"{appNamespace}.{postfixNamespacePart}";
                CreateFolder(appBaseFolder);
            }

            var replacers = GetReplacers(isQuery, description, methodName, appNamespace);
            ReplaceControllerText(controllerPath, controllerText, appNamespace, replacers);
            CopyModels(appBaseFolder, $"{Directory.GetCurrentDirectory()}\\ApplicationModels", replacers);

            // 1. добавить метод в controller
            // 2. создать модели
            //      запроса/команды
            //      ответа
            //      валидатора
            //      хендлера
            // 3. тесты
            //      валидатора
            //      хендлера
        }

        /// <summary>
        /// <see cref=""/>
        /// </summary>
        /// <param name="isQuery"></param>
        /// <param name="description"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private static Dictionary<string, string> GetReplacers(bool isQuery, string description, string methodName, string allNamespace)
        {
            var request = isQuery ? $"{methodName}Query" : $"{methodName}Command";
            var response = isQuery ? $"{methodName}Response" : $"{methodName}Result";
            var descriptionQueryPrefix = isQuery ? "Запрос для" : "Команда для";

            return new Dictionary<string, string>
            {
                {"BaseDescriptionMethod", description},
                {"BaseNameMethod", methodName},
                
                {"BaseNamespace", allNamespace},
                {"baseQueryName", isQuery ? "query" : "command"},
                
                {"BaseHandler", $"{methodName}Handler"},
                {"BaseQueryModel", request},
                {"BaseResponseModel", response},
                {"BaseQueryValidator", $"{request}Validator"},
                
                {"BaseDescriptionResponsePrefix", $"Результат"},
                {"BaseDescriptionQueryPrefix", descriptionQueryPrefix},
            };
        }

        private static void ReplaceControllerText(string controllerPath, string controllerText, string allNamespace, Dictionary<string, string> replacers)
        {
            var newFileText = controllerText.Clone().ToString() ?? "";
            
            var lastIdx = newFileText.LastIndexOf("}");
            newFileText = newFileText.Substring(0, lastIdx);
            
            lastIdx = newFileText.LastIndexOf("}");
            newFileText = newFileText.Substring(0, lastIdx);
            
            var lastIdxFirst = newFileText.LastIndexOf("}");
            var lastIdxSecond = newFileText.LastIndexOf(";");

            lastIdx = lastIdxFirst < lastIdxSecond ? lastIdxFirst : lastIdxSecond;
            newFileText = newFileText.Substring(0, lastIdx + 1);

            var firstUsing = @$"using {allNamespace};
";
            newFileText = $"{firstUsing}{newFileText}{Replace(NewMethodConstant.NewMethodInController, replacers)}";
            
            using (var sw  = new StreamWriter(File.Open(controllerPath, FileMode.Truncate), Encoding.UTF8)) 
            {
                sw.WriteLine(newFileText);             
            }
            // using (var sw = File.CreateText(controllerPath))
            // {
            //     sw.WriteLine(newFileText);
            // }
        }

        private static void CreateFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        private static string GetPostfixNamespace(string controllerText)
        {
            const string separator = ".Controllers.";
            var startIdx = controllerText.IndexOf(separator) + separator.Length;
            var endIdx = controllerText.IndexOf("{");
            
            return controllerText.Substring(startIdx, endIdx - startIdx).Replace("\r\n", "");
        }

        private static void CopyModels(string toFolder, string fromFolder, Dictionary<string, string> replacers)
        {
            var fileNames = Directory.GetFiles(fromFolder);
            foreach (var fileName in fileNames)
            {
                var filText = File.ReadAllText(fileName, Encoding.UTF8);
                
                var newFileName = Replace(Path.GetFileName(fileName), replacers);
                var newFileText= Replace(filText, replacers);

                using (var sw  = new StreamWriter(File.Open($"{toFolder}\\{newFileName}", FileMode.OpenOrCreate), Encoding.UTF8)) 
                {
                    sw.WriteLine(newFileText);             
                }
                
                // using (var sw = File.CreateText($"{toFolder}\\{newFileName}"))
                // {
                //     sw.WriteLine(newFileText);
                // }
            }
        }

        private static string Replace(string text, Dictionary<string, string> replacers)
        {
            var textClone = text.Clone().ToString() ?? "";
                
            foreach (var replacer in replacers)
            {
                textClone = textClone.Replace(replacer.Key, replacer.Value);
            }

            return textClone;
        }
    }
}