using MediaDevices;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Ressuage
{
    class Camera 
    {
        //this methode open a local directory in order to select items
        //return list of photos selected
        public List<string> GetPhoto(string pathdirectory)
        {
            var listePhotos = new List<string>();
            //open file directory and select items
            using (OpenFileDialog folder = new OpenFileDialog() )
            {
                folder.InitialDirectory = pathdirectory;
                folder.Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg" + "|" + "All Files (*.*)|*.*";
                folder.Multiselect = true;
                if (folder.ShowDialog() == DialogResult.OK)
                {
                    foreach (string item in folder.FileNames)
                    {
                        listePhotos.Add(item);
                    }
                }
            }
            return listePhotos;
        }

        //methode to read file and write to a directory
        public void WriteSreamToDisk(string filePath, MemoryStream memoryStream)
        {
            using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = new byte[memoryStream.Length];
                memoryStream.Read(bytes, 0, (int)memoryStream.Length);
                file.Write(bytes, 0, bytes.Length);
                memoryStream.Close();
            }
        }

        //download photo from camera and save to new directory
        public void DownloadPhotos(string chemin_photos, string chemin_telechargement, string camera, string sn)
        {
            string[] directLampe = chemin_photos.Split(',');
            foreach (string dir in directLampe)
            {
                var devices = MediaDevice.GetDevices();
                using (var device = devices.FirstOrDefault(d => d.Description == camera))
                {
                    device.Connect();
                    var photoDir = device.GetDirectoryInfo(@dir);
                    var files = photoDir.EnumerateFiles("*.jpg|*.jpeg", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        device.DownloadFile(file.FullName, memoryStream);
                        memoryStream.Position = 0;
                        WriteSreamToDisk($"{chemin_telechargement}/{sn}/{file.Name}", memoryStream);//chemin télechargemnet de photo
                    }
                    device.Disconnect();
                }
            }
        }

        //delete photo from camera
        public void DeletePhotos(string chemin_photos, string camera)
        {
            string[] directLampe = chemin_photos.Split(',');
            foreach (string dir in directLampe)
            {
                var devices = MediaDevice.GetDevices();
                using (var device = devices.FirstOrDefault(d => d.Description == camera))
                {
                    device.Connect();
                    var photoDir = device.GetDirectoryInfo(@dir);
                    var files = photoDir.EnumerateFiles("*.jpg|*.jpeg|*.png" + "|" + "All Files (*.*)|*.*", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        device.DeleteFile(file.FullName);
                    }
                    device.Disconnect();
                }
            }
        }
    }
}
