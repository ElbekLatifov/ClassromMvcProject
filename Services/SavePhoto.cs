
using System.IO;
namespace C.Services;
public class SavePhoto
{
    private static void CheckFile(string path)
    {
        if(!Directory.Exists($"wwwroot/{path}"))
        {
            Directory.CreateDirectory($"wwwroot/{path}");
        }
    }

    public static async Task<string> SaveUrl(IFormFile file, string path)
    {
        CheckFile(path);
        var filename = Guid.NewGuid() + Path.GetExtension(file.FileName);

        var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        await File.WriteAllBytesAsync(Path.Combine("wwwroot", path, filename), ms.ToArray());
        return $"/{path}/{filename}";
    }

    public static async Task<string> SaveUsersFile(IFormFile file)
    {
        return await SaveUrl(file, "UserFiles");
    }

    public static async Task<string> SaveSchoolsFile(IFormFile file)
    {
        return await SaveUrl(file, "SchoolFiles");
    }
}