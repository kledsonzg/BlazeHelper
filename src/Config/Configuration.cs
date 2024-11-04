namespace BlazeHelper.Config
{
    internal static class Configuration
    {
        private static readonly string _current_folder = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\") );
        internal static string ConfigurationFolder { get; } = Path.Combine(_current_folder, "config");
        internal static string WebFilesFolder { get; } = Path.Combine(_current_folder, "www");

        internal static void Start()
        {
            if(!Directory.Exists(WebFilesFolder) )
            {
                throw new DirectoryNotFoundException("O programa não encontrou a pasta de arquivos. Pasta: " + WebFilesFolder);
            }

            Program.Print("Main Folder: " + _current_folder);
        }
    }
}