using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Lab5 {
  class Program {
    static public string DictionaryPath = @"C:\Users\vyati\source\repos\Lab5\dictionary.txt";
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
            if (FileText.Contains(WordUncorrect[WordUncorrectIndex])) {
              FileText = FileText.Replace(WordUncorrect[WordUncorrectIndex], DictionaryWordCorrect[DictionaryIndex]);
            }
          }

          StreamWriter SW = new StreamWriter(FilePath);

          SW.WriteLine(FileText);

          SW.Close();
        }
      }
    }

    public static void ReadDictionary() {
      FileStream FileDictionary = new FileStream(DictionaryPath, FileMode.OpenOrCreate);

      StreamReader SR = new StreamReader(FileDictionary);

      string FileString;
      while ((FileString = SR.ReadLine()) != null) {
        string[] FileStringWord = FileString.Split(' ');

        foreach (var item in FileStringWord) {
          Console.WriteLine("Ulala " + item);
        }

        DictionaryWordCorrect.Add(FileStringWord.First());

        string[] FileStringUncorrectWord = new string[FileStringWord.Length - 1];

        for (int FileStringUncorrectWordIndex = 1; FileStringUncorrectWordIndex < FileStringUncorrectWord.Length + 1; 
          ++FileStringUncorrectWordIndex) {
          FileStringUncorrectWord[FileStringUncorrectWordIndex - 1] = FileStringWord[FileStringUncorrectWordIndex];
          Console.WriteLine("FUCK " + FileStringUncorrectWord[FileStringUncorrectWordIndex - 1]);
        }

        Dictionary.Add(FileStringWord.First(), FileStringUncorrectWord);
      }

      SR.Close();

      FileDictionary.Close();
    }

    static void Main(string[] args) {
      ReadDictionary();

      CorrectingTyposInTheDirectory(@"C:\Users\vyati\source\repos\Lab5\textfiles");
    }
  }
}