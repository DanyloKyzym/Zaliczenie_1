using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Program
{
    static void Main(string[] args)
    {
        MusicDatabase musicDatabase = new MusicDatabase();

        // Przykładowe dane do testowania
        Album album1 = new Album
        {
            AlbumId = 1,
            Title = "Album 1",
            Type = "CD",
            Duration = "60:00",
            Songs = new List<Song>
            {
                new Song
                {
                    SongId = 1,
                    TrackNumber = 1,
                    Title = "Song 1",
                    Duration = "3:30",
                    Composers = "Composer 1",
                    Performers = new List<string> { "Performer 1", "Performer 2" }
                },
                new Song
                {
                    SongId = 2,
                    TrackNumber = 2,
                    Title = "Song 2",
                    Duration = "4:00",
                    Composers = "Composer 2",
                    Performers = new List<string> { "Performer 3" }
                }
            },
            Performers = new List<string> { "Performer 1", "Performer 2", "Performer 3" }
        };

        musicDatabase.AddAlbum(album1);

        char option;
        do
        {
            Console.Clear();
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Wyświetl wszystkie płyty");
            Console.WriteLine("2. Wyświetl szczegóły płyty");
            Console.WriteLine("3. Wyświetl wykonawców płyty");
            Console.WriteLine("4. Wyświetl szczegóły utworu");
            Console.WriteLine("5. Zapisz bazę do pliku");
            Console.WriteLine("6. Wczytaj bazę z pliku");
            Console.WriteLine("7. Wyjście");
            Console.Write("Wybierz opcję: ");
            option = Console.ReadKey().KeyChar;
            Console.WriteLine();

            switch (option)
            {
                case '1':
                    musicDatabase.DisplayAllAlbums();
                    break;
                case '2':
                    Console.Write("Podaj ID płyty: ");
                    int albumId = int.Parse(Console.ReadLine());
                    musicDatabase.DisplayAlbumDetails(albumId);
                    break;
                case '3':
                    Console.Write("Podaj ID płyty: ");
                    albumId = int.Parse(Console.ReadLine());
                    musicDatabase.DisplayAlbumPerformers(albumId);
                    break;
                case '4':
                    Console.Write("Podaj ID płyty: ");
                    albumId = int.Parse(Console.ReadLine());
                    Console.Write("Podaj ID utworu: ");
                    int songId = int.Parse(Console.ReadLine());
                    musicDatabase.DisplaySongDetails(albumId, songId);
                    break;
                case '5':
                    Console.Write("Podaj nazwę pliku: ");
                    string fileName = Console.ReadLine();
                    musicDatabase.SaveToFile(fileName);
                    break;
                case '6':
                    Console.Write("Podaj nazwę pliku: ");
                    fileName = Console.ReadLine();
                    musicDatabase.LoadFromFile(fileName);
                    break;
                case '7':
                    break;
                default:
                    Console.WriteLine("Nieprawidłowa opcja.");
                    break;
            }
            if (option != '7')
            {
                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
                Console.ReadKey();
            }
        } while (option != '7');
    }
}

public class Album
{
    public int AlbumId { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string Duration { get; set; }
    public List<Song> Songs { get; set; }
    public List<string> Performers { get; set; }
}

public class Song
{
    public int SongId { get; set; }
    public int TrackNumber { get; set; }
    public string Title { get; set; }
    public string Duration { get; set; }
    public string Composers { get; set; }
    public List<string> Performers { get; set; }
}

public class MusicDatabase
{
    private List<Album> albums;

    public MusicDatabase()
    {
        albums = new List<Album>();
    }

    public void AddAlbum(Album album)
    {
        albums.Add(album);
    }

    public void DisplayAllAlbums()
    {
        Console.WriteLine("Tytuły wszystkich płyt:");
        foreach (var album in albums)
        {
            Console.WriteLine(album.Title);
        }
    }

    public void DisplayAlbumDetails(int albumId)
    {
        var album = albums.FirstOrDefault(a => a.AlbumId == albumId);
        if (album != null)
        {
            Console.WriteLine($"Tytuł: {album.Title}");
            Console.WriteLine($"Typ: {album.Type}");
            Console.WriteLine($"Czas trwania: {album.Duration}");
            Console.WriteLine("Spis utworów:");
            foreach (var song in album.Songs)
            {
                Console.WriteLine($"{song.TrackNumber}. {song.Title}");
            }
        }
        else
        {
            Console.WriteLine("Nie znaleziono płyty o podanym ID.");
        }
    }

    public void DisplayAlbumPerformers(int albumId)
    {
        var album = albums.FirstOrDefault(a => a.AlbumId == albumId);
        if (album != null)
        {
            Console.WriteLine("Wykonawcy na płycie:");
            foreach (var performer in album.Performers)
            {
                Console.WriteLine(performer);
            }
        }
        else
        {
            Console.WriteLine("Nie znaleziono płyty o podanym ID.");
        }
    }

    public void DisplaySongDetails(int albumId, int songId)
    {
        var album = albums.FirstOrDefault(a => a.AlbumId == albumId);
        if (album != null)
        {
            var song = album.Songs.FirstOrDefault(s => s.SongId == songId);
            if (song != null)
            {
                Console.WriteLine($"Numer utworu: {song.TrackNumber}");
                Console.WriteLine($"Tytuł utworu: {song.Title}");
                Console.WriteLine($"Czas trwania: {song.Duration}");
                Console.WriteLine("Wykonawcy:");
                foreach (var performer in song.Performers)
                {
                    Console.WriteLine(performer);
                }
                Console.WriteLine($"Kompozytor: {song.Composers}");
            }
            else
            {
                Console.WriteLine("Nie znaleziono utworu o podanym ID.");
            }
        }
        else
        {
            Console.WriteLine("Nie znaleziono płyty o podanym ID.");
        }
    }

    public void SaveToFile(string fileName)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (var album in albums)
            {
                writer.WriteLine($"{album.AlbumId}|{album.Title}|{album.Type}|{album.Duration}");
                foreach (var song in album.Songs)
                {
                    writer.WriteLine($"{song.SongId}|{song.TrackNumber}|{song.Title}|{song.Duration}|{song.Composers}|{string.Join(",", song.Performers)}");
                }
                writer.WriteLine($"Performers|{string.Join(",", album.Performers)}");
            }
        }
    }

    public void LoadFromFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            albums.Clear();
            Album currentAlbum = null;
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 4)
                    {
                        currentAlbum = new Album
                        {
                            AlbumId = int.Parse(parts[0]),
                            Title = parts[1],
                            Type = parts[2],
                            Duration = parts[3],
                            Songs = new List<Song>(),
                            Performers = new List<string>()
                        };
                        albums.Add(currentAlbum);
                    }
                    else if (parts.Length == 6)
                    {
                        var song = new Song
                        {
                            SongId = int.Parse(parts[0]),
                            TrackNumber = int.Parse(parts[1]),
                            Title = parts[2],
                            Duration = parts[3],
                            Composers = parts[4],
                            Performers = parts[5].Split(',').ToList()
                        };
                        currentAlbum?.Songs.Add(song);
                    }
                    else if (parts.Length == 2 && parts[0] == "Performers")
                    {
                        currentAlbum?.Performers.AddRange(parts[1].Split(',').ToList());
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Plik nie istnieje.");
        }
    }
}
