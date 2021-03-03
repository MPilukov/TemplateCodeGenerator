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
            Console.WriteLine("Write controller file path : ");
            var controllerPath = Console.ReadLine();
            
            Console.WriteLine("Write new method name (RecalculationMonitoringOperationsByEndDateAsync) : ");
            var methodName = Console.ReadLine();
            
            Console.WriteLine("It is query ? (y/n) :");
            var isQueryLine = Console.ReadLine();
            var isQuery = string.IsNullOrEmpty(isQueryLine) || isQueryLine.Equals("y");
            
            Console.WriteLine("Method for (добавления событий мониторинга) : ");
            var description = Console.ReadLine();

            var generator = new NewMethodGenerator();
            NewMethodGenerator.Generate(controllerPath, methodName, isQuery, description);
        }
    }
}