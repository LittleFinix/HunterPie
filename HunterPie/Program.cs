using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace HunterPie;

public class Program
{
    public static int Main(string[] args)
    {
        return BuildAvaloniaApp()
#pragma warning disable CS0618 // Type or member is obsolete
            .With<IAssetLoader>(new CustomAssetLoader(new StandardAssetLoader(typeof(App).Assembly)))
#pragma warning restore CS0618 // Type or member is obsolete
            .UseRuntimePlatformSubsystem(RegisterPlatformSubsystem)
            .StartWithClassicDesktopLifetime(args);
    }

    private static AppBuilder BuildAvaloniaApp()
    {
        FontManagerOptions options;
        
        if (OperatingSystem.IsLinux())
            options = new() { DefaultFamilyName = "DejaVu Sans" };
        else
            options = new() { DefaultFamilyName = "Arial" };
    
        return AppBuilder.Configure<App>()
            .With(options)
            .UsePlatformDetect();
    }

    private static void RegisterPlatformSubsystem()
    {
        if (!UriParser.IsKnownScheme("avares"))
            UriParser.Register(new GenericUriParser(
                GenericUriParserOptions.GenericAuthority |
                GenericUriParserOptions.NoUserInfo |
                GenericUriParserOptions.NoPort |
                GenericUriParserOptions.NoQuery |
                GenericUriParserOptions.NoFragment), "avares", -1);
    }
}

public class CustomAssetLoader : IAssetLoader
{
    private readonly IAssetLoader _baseLoader;

    public CustomAssetLoader(IAssetLoader baseLoader)
    {
        _baseLoader = baseLoader;
    }

    private Stream? OpenWebUrl(Uri uri, Uri? baseUri = null)
    {
        if (uri.Scheme is not "http" and not "https")
            return null;

        HttpClient client = new();

        try
        {
            byte[] bytes = client.GetByteArrayAsync(uri).Result;
            return new MemoryStream(bytes);
        }
        catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public void SetDefaultAssembly(Assembly assembly) => _baseLoader.SetDefaultAssembly(assembly);

    public bool Exists(Uri uri, Uri? baseUri = null) => OpenWebUrl(uri, baseUri) is not null || _baseLoader.Exists(uri, baseUri);

    public Stream Open(Uri uri, Uri? baseUri = null) => OpenWebUrl(uri, baseUri) ?? _baseLoader.Open(uri, baseUri);

    public (Stream stream, Assembly assembly) OpenAndGetAssembly(Uri uri, Uri? baseUri = null) => _baseLoader.OpenAndGetAssembly(uri, baseUri);

    public Assembly? GetAssembly(Uri uri, Uri? baseUri = null) => _baseLoader.GetAssembly(uri, baseUri);

    public IEnumerable<Uri> GetAssets(Uri uri, Uri? baseUri) => _baseLoader.GetAssets(uri, baseUri);
}
