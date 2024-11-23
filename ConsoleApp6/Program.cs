using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    public static void FindFilesByMaskAndDate(string path, string mask, DateTime startDate, DateTime endDate, string reportFile)
    {
        try
        {
            var files = Directory.GetFiles(path, mask, SearchOption.AllDirectories);

            using (StreamWriter writer = new StreamWriter(reportFile))
            {
                writer.WriteLine($"Поиск файлов по маске '{mask}' в каталоге {path}, между датами {startDate} и {endDate}:\n");

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    DateTime lastModified = fileInfo.LastWriteTime;

                    if (lastModified >= startDate && lastModified <= endDate)
                    {
                        writer.WriteLine($"{file} | Дата модификации: {lastModified}");
                    }
                }
            }

            Console.WriteLine($"Отчет записан в файл {reportFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при поиске файлов: {ex.Message}");
        }
    }

    public static List<string> FindFilesAndDirectories(string rootPath, string mask)
    {
        List<string> foundFiles = new List<string>();

        try
        {
            var files = Directory.GetFiles(rootPath, mask, SearchOption.AllDirectories);
            foundFiles.AddRange(files);

            var directories = Directory.GetDirectories(rootPath, mask, SearchOption.AllDirectories);
            foundFiles.AddRange(directories);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при поиске файлов: {ex.Message}");
        }

        return foundFiles;
    }

    public static void DeleteFilesOrDirectories(List<string> files)
    {
        Console.WriteLine("Выберите действие:");
        Console.WriteLine("1. Удалить все найденные файлы/каталоги");
        Console.WriteLine("2. Удалить выбранный файл/каталог");
        Console.WriteLine("3. Удалить диапазон файлов/каталогов");
        string action = Console.ReadLine();

        try
        {
            if (action == "1")
            {
                foreach (var file in files)
                {
                    if (Directory.Exists(file))
                    {
                        Directory.Delete(file, true); 
                        Console.WriteLine($"Удален каталог: {file}");
                    }
                    else
                    {
                        File.Delete(file); 
                        Console.WriteLine($"Удален файл: {file}");
                    }
                }
            }
            else if (action == "2")
            {
                for (int i = 0; i < files.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {files[i]}");
                }

                Console.Write("Введите номер файла/каталога для удаления: ");
                int choice = int.Parse(Console.ReadLine()) - 1;

                if (choice >= 0 && choice < files.Count)
                {
                    string fileToDelete = files[choice];
                    if (Directory.Exists(fileToDelete))
                    {
                        Directory.Delete(fileToDelete, true); 
                        Console.WriteLine($"Удален каталог: {fileToDelete}");
                    }
                    else
                    {
                        File.Delete(fileToDelete); 
                        Console.WriteLine($"Удален файл: {fileToDelete}");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный номер.");
                }
            }
            else if (action == "3")
            {
                Console.WriteLine("Введите диапазон (например, 2-5):");
                string rangeInput = Console.ReadLine();
                string[] range = rangeInput.Split('-');
                int start = int.Parse(range[0]) - 1;
                int end = int.Parse(range[1]);

                for (int i = start; i <= end && i < files.Count; i++)
                {
                    string fileToDelete = files[i];
                    if (Directory.Exists(fileToDelete))
                    {
                        Directory.Delete(fileToDelete, true);
                        Console.WriteLine($"Удален каталог: {fileToDelete}");
                    }
                    else
                    {
                        File.Delete(fileToDelete);
                        Console.WriteLine($"Удален файл: {fileToDelete}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Некорректный выбор.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении: {ex.Message}");
        }
    }

    static void Main()
    {
        Console.WriteLine("Выберите задачу:");
        Console.WriteLine("1. Поиск файлов по маске и диапазону дат");
        Console.WriteLine("2. Поиск файлов/каталогов по маске с удалением");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            Console.WriteLine("Введите путь к каталогу:");
            string path = Console.ReadLine();

            Console.WriteLine("Введите маску (например, *.txt):");
            string mask = Console.ReadLine();

            Console.WriteLine("Введите дату начала (например, 2023-01-01):");
            DateTime startDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Введите дату окончания (например, 2023-12-31):");
            DateTime endDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Введите имя файла для отчета (например, report.txt):");
            string reportFile = Console.ReadLine();

            FindFilesByMaskAndDate(path, mask, startDate, endDate, reportFile);
        }
        else if (choice == "2")
        {
            Console.WriteLine("Введите путь для поиска (например, C:\\):");
            string rootPath = Console.ReadLine();

            Console.WriteLine("Введите маску для поиска (например, *.*):");
            string mask = Console.ReadLine();

            List<string> files = FindFilesAndDirectories(rootPath, mask);

            Console.WriteLine($"Найдено {files.Count} объектов:");
            for (int i = 0; i < files.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {files[i]}");
            }

            DeleteFilesOrDirectories(files);
        }
        else
        {
            Console.WriteLine("Некорректный выбор.");
        }
    }
}
