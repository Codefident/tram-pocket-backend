using Microsoft.Extensions.Options;
using TramPocketBackend.Configuration;

namespace TramPocketBackend.Initializers;

public sealed class DataDirectoryInitializer(IHostEnvironment env, IOptions<GtfsOptions> options)
{
    public void Initialize()
    {
        var dataPath = Path.Combine(env.ContentRootPath, options.Value.DataPath);
        var gtfsExtractDir = Path.Combine(env.ContentRootPath, options.Value.ExtractPath);

        Directory.CreateDirectory(dataPath);
        Directory.CreateDirectory(gtfsExtractDir);
    }
}