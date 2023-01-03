// See https://aka.ms/new-console-template for more information
using FolderMonitor.SnapShot;
   
Console.WriteLine("Hello, World!");

FolderWatcher monitor = new FolderWatcher(@"C:\temp");

Console.ReadLine();
monitor.Dispose();  

