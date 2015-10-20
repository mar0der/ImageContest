namespace PhotoContest.App.Helpers
{
    using Dropbox.Api;
    using Dropbox.Api.Files;
    using Dropbox.Api.Sharing;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    public static class DropBoxManager
    {
        private const string path = "/PhotoContest/";
        private const string AccessToken = "uBxMwG9GRMAAAAAAAAAANriZN5WkqGOEpcPMFA2JXhv0hrKaHMbXzBV82gu4SWZ7";

        public static async Task<PathLinkMetadata> Upload(Stream stream, string fileName)
        {
            byte[] buffer = null;
            using (var fs = stream)
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }

            using (var dbx = new DropboxClient(AccessToken))
            {
                using (var mem = new MemoryStream(buffer))
                {
                    var updated = await dbx.Files.UploadAsync(path + fileName,
                        WriteMode.Overwrite.Instance,
                        body: mem);
                }

                return await dbx.Sharing.CreateSharedLinkAsync(path + fileName);
            }
        }
        public static async Task Delete(string fileName)
        {
            using (var dbx = new DropboxClient(AccessToken))
            {
                await dbx.Files.DeleteAsync(path + fileName);
            }
        }
    }
}