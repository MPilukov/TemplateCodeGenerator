using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BaseNamespace;

namespace TemplateCodeGenerator
{
    /// <summary>
    /// Генератор нового апи-метода
    /// </summary>
    public class NewMethodGenerator
    {
        /// <summary>
        /// Сгенерировать новый метод для апи и зависимые контракты
        /// </summary>
        /// <param name="controllerPath">Абсолютный путь к файлу-контроллеру</param>
        /// <param name="methodName">Имя метода</param>
        /// <param name="url">Урл внутри контроллера</param>
        /// <param name="methodHttpType">Http-тип метода</param>
        /// <param name="isQuery">Это запрос (или команда)</param>
        /// <param name="isContainsResponse">Метод содержить ответ (или без ответа)</param>
        /// <param name="russianDescription">Описание метода на русском языке (постфикс описания)</param>
        public static void Generate(string controllerPath, string url, string methodHttpType, string methodName, bool isQuery, bool isContainsResponse, string russianDescription)
        {
            var (slnName, rootFolder) = GetSlnData(controllerPath);
            var methodNameWithoutAsync = methodName.EndsWith("Async") ? methodName.Remove(methodName.Length - 5, 5) : methodName;

            Generate(controllerPath, methodNameWithoutAsync, url, methodHttpType, isQuery, isContainsResponse, russianDescription, slnName, rootFolder);
        }

        private static (string, string) GetSlnData(string controllerPath)
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
            
            return (slnName, rootFolder);
        }

        /// <summary>
        /// Сгенерировать новый метод для апи и зависимые файлы
        /// </summary>
        /// <param name="controllerPath">Абсолютный путь к файлу-контроллеру</param>
        /// <param name="methodName">Имя метода</param>
        /// <param name="url">Урл внутри контроллера</param>
        /// <param name="methodHttpType">Http-тип метода</param>
        /// <param name="isQuery">Это запрос (или команда)</param>
        /// <param name="isContainsResponse">Метод содержить ответ (или без ответа)</param>
        /// <param name="russianDescription">Описание метода на русском языке (постфикс описания)</param>
        /// <param name="slnName">Имя проекта</param>
        /// <param name="rootFolder">Корневая папка проекта</param>
        private static void Generate(
            string controllerPath,
            string methodName,
            string url,
            string methodHttpType,
            bool isQuery,
            bool isContainsResponse,
            string russianDescription,
            string slnName,
            string rootFolder)
        {
            var controllerText = File.ReadAllText(controllerPath, Encoding.UTF8);

            var postfixNamespace = GetPostfixNamespaceForController(controllerText);
            var postfixNamespaceArray = postfixNamespace.Split(".").ToList();
            postfixNamespaceArray.Add(methodName);

            var applicationBaseFolder = $"{rootFolder}{slnName}.Application";
            var applicationNamespace = $"{slnName}.Application";
            
            foreach (var postfixNamespacePart in postfixNamespaceArray)
            {
                applicationBaseFolder = $"{applicationBaseFolder}\\{postfixNamespacePart}";
                applicationNamespace = $"{applicationNamespace}.{postfixNamespacePart}";
                CreateFolder(applicationBaseFolder);
            }

            var replacers = GetReplacers(methodName, url, isQuery, russianDescription, applicationNamespace);

            AddMethodToControllerText(controllerPath, controllerText, methodHttpType, isContainsResponse, applicationNamespace, replacers);

            var currentDirectory = Directory.GetCurrentDirectory();
            CopyModels(applicationBaseFolder,
                isContainsResponse
                    ? $"{currentDirectory}\\ApplicationModels\\WithResponse"
                    : $"{currentDirectory}\\ApplicationModels\\WithoutResponse", replacers);
            CopyModels(applicationBaseFolder, $"{currentDirectory}\\ApplicationModels\\Validators", replacers);

            // todo: application tests
        }

        /// <summary>
        /// Получить словарь базовых замен 
        /// </summary>
        /// <param name="methodName">Имя метода</param>
        /// <param name="url">Урл внутри контроллера</param>
        /// <param name="isQuery">Это запрос (или команда)</param>
        /// <param name="russianDescription">Описание метода на русском языке (постфикс описания)</param>
        /// <param name="baseNamespace">Базовый неймспей</param>
        /// <returns></returns>
        private static Dictionary<string, string> GetReplacers(string methodName, string url, bool isQuery, string russianDescription, string baseNamespace)
        {
            var request = isQuery ? $"{methodName}Query" : $"{methodName}Command";
            var response = isQuery ? $"{methodName}Response" : $"{methodName}Result";
            var descriptionQueryPrefix = isQuery ? "Запрос для" : "Команда для";

            return new Dictionary<string, string>
            {
                {"BaseDescriptionMethod", russianDescription},
                {NewMethodConstants.BaseNameMethodTemplate, methodName},
                // {"BaseNameMethod", methodName},
                
                {"BaseNamespace", baseNamespace},
                {".WithoutResponseNamespace", ""},
                {"BaseQueryName", isQuery ? "query" : "command"},
                
                {nameof(BaseHandler), $"{methodName}Handler"},
                {nameof(BaseQueryModel), request},
                {nameof(BaseResponseModel), response},
                {nameof(BaseQueryValidator), $"{request}Validator"},
                
                {"BaseDescriptionResponsePrefix", $"Результат"},
                {"BaseDescriptionQueryPrefix", descriptionQueryPrefix},
                
                {NewMethodConstants.BaseUriAddressTemplate, url},
            };
        }

        private static void AddMethodToControllerText(
            string controllerPath, string controllerText, string methodHttpType, 
            bool isContainsResponse, string applicationNamespace, Dictionary<string, string> replacers)
        {
            var newFileText = controllerText.Clone().ToString() ?? "";
            
            var lastCloseBracketIdx = newFileText.LastIndexOf("}");
            newFileText = newFileText.Substring(0, lastCloseBracketIdx);
            
            lastCloseBracketIdx = newFileText.LastIndexOf("}");
            newFileText = newFileText.Substring(0, lastCloseBracketIdx);
            
            lastCloseBracketIdx = newFileText.LastIndexOf("}");
            var lastSemicolonIdx = newFileText.LastIndexOf(";");

            var lastIdx = lastCloseBracketIdx > lastSemicolonIdx ? lastCloseBracketIdx : lastSemicolonIdx;
            newFileText = newFileText.Substring(0, lastIdx + 1);

            var firstUsing = @$"using {applicationNamespace};
";
            var textOfMethodInController = GetTextOfMethodInController(methodHttpType, isContainsResponse, replacers);

            newFileText = $"{firstUsing}{newFileText}{textOfMethodInController}";
            
            using (var sw  = new StreamWriter(File.Open(controllerPath, FileMode.Truncate), Encoding.UTF8)) 
            {
                sw.WriteLine(newFileText);             
            }
        }

        private static string GetTextOfMethodInController(
            string methodHttpType, bool isContainsResponse, Dictionary<string, string> replacers)
        {
            var textOfTemplateInController = "";

            switch (methodHttpType.ToUpper())
            {
                case "POST":
                    textOfTemplateInController = isContainsResponse
                        ? NewMethodConstants.NewPostMethodTemplateInController
                        : NewMethodConstants.NewPostWithoutResponseMethodTemplateInController;
                    break;
                case "GET":
                    textOfTemplateInController = NewMethodConstants.NewGetMethodTemplateInController;
                    break;
                case "DELETE":
                    textOfTemplateInController = NewMethodConstants.NewDeleteMethodTemplateInController;
                    break;
                default:
                    throw new Exception($"Тип {methodHttpType} не поддерживается");
            }

            return Replace(textOfTemplateInController, replacers);
        }

        /// <summary>
        /// Получить продолжение неймспейса контроллера
        /// </summary>
        /// <param name="controllerText"></param>
        /// <returns></returns>
        private static string GetPostfixNamespaceForController(string controllerText)
        {
            const string separator = ".Controllers.";
            var startIdx = controllerText.IndexOf(separator) + separator.Length;
            var endIdx = controllerText.IndexOf("{");
            
            return controllerText.Substring(startIdx, endIdx - startIdx).Replace("\r\n", "");
        }

        /// <summary>
        /// Скопировать необходимые модели для методв
        /// </summary>
        /// <param name="toFolder">Папка-источник</param>
        /// <param name="fromFolder">Папка-цель</param>
        /// <param name="replacers">Словарь с заменами</param>
        private static void CopyModels(string toFolder, string fromFolder, Dictionary<string, string> replacers)
        {
            var fileNames = Directory.GetFiles(fromFolder);
            foreach (var fileName in fileNames)
            {
                var fileText = File.ReadAllText(fileName, Encoding.UTF8);
                
                var newFileName = Replace(Path.GetFileName(fileName), replacers);
                var newFileText= Replace(fileText, replacers);

                using (var sw  = new StreamWriter(File.Open($"{toFolder}\\{newFileName}", FileMode.OpenOrCreate), Encoding.UTF8)) 
                {
                    sw.WriteLine(newFileText);             
                }
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

        private static void CreateFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
    }
}