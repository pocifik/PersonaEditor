using System;
using System.Collections.Generic;
using System.IO;

namespace PersonaEditorGUI.Classes
{
    [Flags]
    public enum myFileType
    {
        None    = 0,
        Text    = 1,
        Graphic = 2,
        FTD     = 4,
    }

    public struct ValidFile
    {
        public string path;
        public myFileType types;

        public ValidFile(string path, myFileType types)
        {
            this.path = path;
            this.types = types;
        }
    }

    public static class Parser
    {
        public static List<ValidFile> Run()
        {
            List<ValidFile> validFiles = new List<ValidFile>();

            string[] filePaths = Directory.GetFiles(@"E:\Games\P5RUS\cpk\", "*.*",
                                                    SearchOption.AllDirectories);

            for (int i = 0; i < filePaths.Length; i++)
            {
                string filePath = filePaths[i];
                string fileName = Path.GetFileName(filePath);
                byte[] fileData = File.ReadAllBytes(filePath);
                myFileType fileType = PersonaEditorLib.Utilities.PersonaFile.GetFileType(fileName);
                var file = PersonaEditorLib.Utilities.PersonaFile.OpenFile(fileName, fileData, fileType);
                if (file.Object != null)
                {
                    validFiles.Add(filePath, GetFileType(file.Object));
                }
            }

            return validFiles;
        }

        public static myFileType GetFileType(object fileObject)
        {
            if (fileObject is PersonaEditorLib.FileStructure.Text.BMD bmd)
            {
                if (bmd.Msg.Count > 0)
                {
                    return myFileType.Text;
                }
            }
            else if (fileObject is PersonaEditorLib.FileStructure.Graphic.DDS dds)
            {
                return myFileType.Graphic;
            }
            else if (fileObject is PersonaEditorLib.FileStructure.Container.BIN bin)
            {
                myFileType fileTypes = myFileType.None;
                for (int i = 0; i < bin.SubFiles.Count; i++)
                {
                    myFileType fileType = GetFileType(bin.SubFiles[i]);
                    if (!fileType.HasFlag(fileType))
                            fileTypes |= fileType;
                }

                return fileTypes;
            }
            else if (fileObject is PersonaEditorLib.FileStructure.Text.FTD ftd)
            {
                return myFileType.FTD;
            }

            return myFileType.None;
        }
    }
}
