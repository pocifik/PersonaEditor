﻿using PersonaEditorLib.Extension;
using System;
using System.IO;

namespace PersonaEditorLib.FileStructure.SPR
{
    public class SPDKey : BindingObject
    {
        public int ListIndex { get; private set; }
        public int TextureIndex { get; private set; }
        public int[] Unk0x08 { get; private set; } // x 6
        public int X0 { get; set; }
        public int Y0 { get; set; }
        public int Xdel { get; set; }
        public int Ydel { get; set; }
        public int[] Unk0x30 { get; private set; } // x 2
        public int[] Unk0x40 { get; private set; } // x 12
        public byte[] Comment { get; private set; }

        public SPDKey(BinaryReader reader)
        {
            ListIndex = reader.ReadInt32();
            TextureIndex = reader.ReadInt32();
            Unk0x08 = reader.ReadInt32Array(6);
            X0 = reader.ReadInt32();
            Y0 = reader.ReadInt32();
            Xdel = reader.ReadInt32();
            Ydel = reader.ReadInt32();
            Unk0x30 = reader.ReadInt32Array(4);

            //if (reader.ReadInt32() != Xdel)
            //    throw new Exception("SPDKey: Xdel wrong");
            //if (reader.ReadInt32() != Ydel)
            //    throw new Exception("SPDKey: Ydel wrong");

            Unk0x40 = reader.ReadInt32Array(12);
            Comment = reader.ReadBytes(0x30);
        }

        public int Size
        {
            get { return 0xa0; }
        }

        public void Get(BinaryWriter writer)
        {
            writer.Write(ListIndex);
            writer.Write(TextureIndex);
            writer.WriteInt32Array(Unk0x08);
            writer.Write(X0);
            writer.Write(Y0);
            writer.Write(Xdel);
            writer.Write(Ydel);
            writer.WriteInt32Array(Unk0x30);
            //writer.Write(Xdel);
            //writer.Write(Ydel);
            writer.WriteInt32Array(Unk0x40);
            writer.Write(Comment);
        }
    }

}