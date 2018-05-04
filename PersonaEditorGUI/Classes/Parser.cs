using System;
using System.Collections.Generic;
using System.IO;

namespace PersonaEditorGUI.Classes
{
    [Flags]
    public enum FileType
    {
        None = 1,
        Text = 2,
        Graphic = 4,
    }

    public struct ValidFile
    {
        public string path;
        public FileType types;

        public ValidFile(string path, FileType types)
        {
            this.path = path;
            this.types = types;
        }
    }

    public static class Parser
    {
        public static void Run()
        {
            List<ValidFile> validFiles = new List<ValidFile>();

            string[] filePaths = Directory.GetFiles(@"E:\Games\P5RUS\cpk\", "*.*",
                                                    SearchOption.AllDirectories);

            for (int i = 0; i < filePaths.Length; i++)
            {
                string filePath = filePaths[i];
                string fileName = Path.GetFileName(filePath);
                byte[] fileData = File.ReadAllBytes(filePath);
                FileType fileType = PersonaEditorLib.Utilities.PersonaFile.GetFileType(fileName);
                var file = PersonaEditorLib.Utilities.PersonaFile.OpenFile(fileName, fileData, fileType);
                if (file.Object != null)
                {
                    validFiles.Add(filePath, GetFileType(file.Object));

                }
            }
        }

        public static FileType GetFileType(object fileObject)
        {
            if (fileObject is PersonaEditorLib.FileStructure.Text.BMD bmd)
            {
                if (bmd.Msg.Count > 0)
                {
                    return FileType.Text;
                }
            }
            else if (fileObject is PersonaEditorLib.FileStructure.Graphic.DDS dds)
            {
                return FileType.Graphic;
            }
            else if (fileObject is PersonaEditorLib.FileStructure.Container.BIN bin)
            {
                FileType fileTypes = FileType.None;
                for (int i = 0; i < bin.SubFiles.Count; i++)
                {
                    FileType fileType = GetFileType(bin.SubFiles[i]);
                    if (!fileType.HasFlag(fileType))
                        fileTypes |= fileType;
                }

                return fileTypes;
            }

            return FileType.None;
        }
    }
}
