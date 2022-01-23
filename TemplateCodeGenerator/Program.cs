using System;

namespace TemplateCodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello to TemplateCodeGenerator!");
            Console.WriteLine("Choose an option : ");
            Console.WriteLine("New method in controller : 1");
            var optionCode = Console.ReadLine();
            
            switch (optionCode)
            {
                case "1":
                    GenerateNewMethodInController();
                    break;
            }
        }

        private static void GenerateNewMethodInController()
        {
            Console.WriteLine("Write controller file full path : ");
            var controllerPath = Console.ReadLine();
            
            Console.WriteLine("Write new method name (RecalculationMonitoringOperationsByEndDateAsync) : ");
            var methodName = Console.ReadLine();
            
            Console.WriteLine("Choose an method type (post, get, delete) : ");
            var methodHttpType = Console.ReadLine();
            
            Console.WriteLine("Write uri in controller (\"notification-recipient-role\") : ");
            var url = Console.ReadLine();
            
            Console.WriteLine("It is query ? (y/n) :");
            var isQueryLine = Console.ReadLine();
            var isQuery = string.IsNullOrEmpty(isQueryLine) || isQueryLine.Equals("y");

            var isContainsResponse = true;
            if (isQuery)
            {
                isContainsResponse = true;
            }
            else
            {
                if ((methodHttpType ?? "").Equals("delete"))
                {
                    isContainsResponse = false;
                }
                else
                {
                    Console.WriteLine("Is is command with response (y/n) ? ");

                    var isContainsResponseLine = Console.ReadLine();
                    isContainsResponse = string.IsNullOrEmpty(isContainsResponseLine) || isContainsResponseLine.Equals("y");
                }
            }
            
            Console.WriteLine("Write description for method (добавления событий мониторинга) : ");
            var description = Console.ReadLine();

            NewMethodGenerator.Generate(controllerPath, url, methodHttpType, methodName, isQuery, isContainsResponse, description);
        }
    }
}