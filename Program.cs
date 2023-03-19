using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Lab5 {
  class Program {
    static public string DictionaryPath = @"\dictionary.txt";
    static public List<string> DictionaryWordCorrect = new List<string>();
    static public Dictionary<string, string[]> Dictionary = new Dictionary<string, string[]>();

    public static string ReadFile(string FilePath) {
      string FileText;

      StreamReader SR = new StreamReader(FilePath);

      FileText = SR.ReadToEnd();

      SR.Close();

      return FileText;
    }

    public static void CorrectingTyposInTheDirectory(string DirectoryPath) {
      var FilesTXTInDirectory = Directory.EnumerateFiles(DirectoryPath, "*.txt", SearchOption.AllDirectories);

      foreach (var FileTXT in FilesTXTInDirectory) {
        string FilePath = FileTXT;
        string FileText = ReadFile(FilePath);

        for (int DictionaryIndex = 0; DictionaryIndex < Dictionary.Count; ++DictionaryIndex) {
          string[] WordUncorrect = Dictionary[DictionaryWordCorrect[DictionaryIndex]];

          for (int WordUncorrectIndex = 0; WordUncorrectIndex < WordUncorrect.Length; ++WordUncorrectIndex) {
            if (FileText.ToLower().Contains(WordUncorrect[WordUncorrectIndex])) {
              FileText = FileText.ToLower().Replace(WordUncorrect[WordUncorrectIndex], DictionaryWordCorrect[DictionaryIndex]);
            }
          }
        }

        StreamWriter SW = new StreamWriter(FilePath);

        SW.WriteLine(FileText);

        SW.Close();
      }
    }

    public static void ReadDictionary() {
      FileStream FileDictionary = new FileStream(DictionaryPath, FileMode.OpenOrCreate);

      StreamReader SR = new StreamReader(FileDictionary);

      string FileString;
      while ((FileString = SR.ReadLine()) != null) {
        string[] FileStringWord = FileString.Split(' ');

        DictionaryWordCorrect.Add(FileStringWord.First());

        string[] FileStringUncorrectWord = new string[FileStringWord.Length - 1];

        for (int FileStringUncorrectWordIndex = 1; FileStringUncorrectWordIndex < FileStringUncorrectWord.Length + 1; 
          ++FileStringUncorrectWordIndex) {
          FileStringUncorrectWord[FileStringUncorrectWordIndex - 1] = FileStringWord[FileStringUncorrectWordIndex];
        }

        Dictionary.Add(FileStringWord.First(), FileStringUncorrectWord);
      }

      SR.Close();

      FileDictionary.Close();
    }

    public static void ReplacePhoneNumber(string DirectoryPath) {
      var FilesTXTInDirectory = Directory.EnumerateFiles(DirectoryPath, "*.txt", SearchOption.AllDirectories);

      foreach (var FileTXT in FilesTXTInDirectory) {
        string FilePath = FileTXT;
        string FileText = ReadFile(FilePath);
        string PatternString = @"\((\d{3})\)\s(\d{3}-\d{2}-\d{2})";

        Regex Pattern = new Regex(PatternString);
        MatchCollection Matches = Pattern.Matches(FileText);

        string[] MatchesString = new string[Matches.Count];

        for (int MatchesStringIndex = 0; MatchesStringIndex < MatchesString.Length; ++MatchesStringIndex) {
          MatchesString[MatchesStringIndex] = Matches[MatchesStringIndex].ToString();
        }

        for (int MatchesStringIndex = 0; MatchesStringIndex < MatchesString.Length; ++MatchesStringIndex) {
          string Result = MatchesString[MatchesStringIndex];

          Result = Result.Replace("-", " ");
          Result = Result.Replace("(", "");
          Result = Result.Replace(")", ""); // 012 345 67 89
          Result = "+38" + Result.First() + " " + Result.Substring(1); // +380 12 345 67 89

          FileText = FileText.Replace(MatchesString[MatchesStringIndex], Result);
        }

        StreamWriter SW = new StreamWriter(FilePath);

        SW.WriteLine(FileText);

        SW.Close();
      }
    }

    static void Main(string[] args) {
      Console.WriteLine("Что нужно сделать?\n\t1. Исправить очепятки в директории\n\t" +
        "2. Привести телефонные номера формата (012) 345-67-89 к +380 12 345 67 89");
      int UserChoice = int.Parse(Console.ReadLine());

      while (true) {
        switch (UserChoice) {
          case 1:
            ReadDictionary();

            Console.Write("Введите путь к директории: ");
            string DirectoryPath = Console.ReadLine();

            CorrectingTyposInTheDirectory(DirectoryPath);

            break;
          case 2:
            Console.Write("Введите путь к директории: ");
            DirectoryPath = Console.ReadLine();

            ReplacePhoneNumber(DirectoryPath);

            break;
          default:
            Console.WriteLine("Такого пункта не существует!");

            break;
        }
      }

      //string strText = "Ваш телефон (012) 345-67-89, (312) 344-62-81";
      //string strFind = @"\((\d{3})\)\s(\d{3}-\d{2}-\d{2})";
      //Console.WriteLine(strText);
      //Console.WriteLine();
      //Regex Pattern = new Regex(strFind);
      //MatchCollection Matches = Pattern.Matches(strText);

      //string[] MatchesString = new string[Matches.Count];

      //for (int MatchesStringIndex = 0; MatchesStringIndex < MatchesString.Length; ++MatchesStringIndex) {
      //  MatchesString[MatchesStringIndex] = Matches[MatchesStringIndex].ToString();
      //}

      //foreach (var item in MatchesString) {
      //  Console.WriteLine(item);
      //}

      //for (int MatchesStringIndex = 0; MatchesStringIndex  < MatchesString.Length; ++MatchesStringIndex) {
      //  string Result = MatchesString[MatchesStringIndex];

      //  Result = Result.Replace("-", " ");
      //  Result = Result.Replace("(", "");
      //  Result = Result.Replace(")", ""); // 012 345 67 89
      //  Result = "+38" + Result.First() + " " + Result.Substring(1);

      //  strText = strText.Replace(MatchesString[MatchesStringIndex], Result);
      //}



      //Console.WriteLine();
      //Console.WriteLine(strText);
      //Console.ReadKey();
    }
  }
}