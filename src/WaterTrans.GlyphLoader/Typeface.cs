﻿// <copyright file="Typeface.cs" company="WaterTrans">
// © 2020 WaterTrans
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using WaterTrans.GlyphLoader.Internal;

namespace WaterTrans.GlyphLoader
{
    /// <summary>
    /// Main class for WaterTrans.GlyphLoader.
    /// </summary>
    public class Typeface
    {
        private readonly Dictionary<string, TableDirectory> _tableDirectories = new Dictionary<string, TableDirectory>(StringComparer.OrdinalIgnoreCase);
        private readonly Stream _stream;
        private TableOfCMAP _tableOfCMAP;
        private TableOfMAXP _tableOfMAXP;
        private TableOfHEAD _tableOfHEAD;
        private TableOfHHEA _tableOfHHEA;
        private TableOfHMTX _tableOfHMTX;
        private TableOfOS2 _tableOfOS2;
        private TableOfVHEA _tableOfVHEA;
        private TableOfMORT _tableOfMORT;
        private TableOfGSUB _tableOfGSUB;
        private TableOfGPOS _tableOfGPOS;

        /// <summary>
        /// Initializes a new instance of the <see cref="Typeface"/> class.
        /// </summary>
        /// <param name="stream">The font file stream.</param>
        public Typeface(Stream stream)
            : this(stream, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Typeface"/> class.
        /// </summary>
        /// <param name="stream">The font file stream.</param>
        /// <param name="index">TrueType collections or OpenType collections index(Zero-based numbering).</param>
        public Typeface(Stream stream, int index)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new NotSupportedException("The stream does not support reading.");
            }

            _stream = stream;

            if (!stream.CanSeek)
            {
                _stream = new MemoryStream();
                stream.CopyTo(_stream);
            }

            _stream.Position = 0;

            if (IsCollection(_stream))
            {
                ReadCollectionTypeface(_stream, index);
            }
            else
            {
                ReadTypeface(_stream);
            }
        }

        /// <summary>
        /// Gets sfnt Major Version.
        /// </summary>
        internal ushort SfntVersionMajor { get; private set; }

        /// <summary>
        /// Gets sfnt Minor Version.
        /// </summary>
        internal ushort SfntVersionMinor { get; private set; }

        /// <summary>
        /// Gets number of tables.
        /// </summary>
        internal ushort NumTables { get; private set; }

        /// <summary>
        /// Gets (Maximum power of 2 &lt;= numTables) x 16.
        /// </summary>
        internal ushort SearchRange { get; private set; }

        /// <summary>
        /// Gets log2(maximum power of 2 &gt;= numTables).
        /// </summary>
        internal ushort EntrySelector { get; private set; }

        /// <summary>
        /// Gets numTables x 16-searchRange.
        /// </summary>
        internal ushort RangeShift { get; private set; }

        private bool IsCollection(Stream stream)
        {
            bool result;
            stream.Position = 0;
            using (var reader = new TypefaceReader(stream))
            {
                result = reader.ReadCharArray(4) == "ttcf";
            }
            stream.Position = 0;
            return result;
        }

        private void ReadCollectionTypeface(Stream stream, int index)
        {
            using (var reader = new TypefaceReader(stream))
            {
                string ttctag = reader.ReadCharArray(4);
                ushort ttcVersionMajor = reader.ReadUInt16();
                ushort ttcVersionMinor = reader.ReadUInt16();
                uint ttcDirectoryCount = reader.ReadUInt32();

                for (int i = 0; i <= Convert.ToInt32(ttcDirectoryCount - 1); i++)
                {
                    uint ttcDirectoryOffset = reader.ReadUInt32();
                    if (i == index)
                    {
                        stream.Position = ttcDirectoryOffset;
                        ReadTypeface(stream);
                        return;
                    }
                }
            }
            throw new ArgumentOutOfRangeException("Font index out of range.");
        }

        private void ReadCMAP(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.CMAP))
            {
                return;
            }

            _stream.Position = _tableDirectories[TableNames.CMAP].Offset;
            _tableOfCMAP = new TableOfCMAP(reader);
        }

        private void ReadMAXP(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.MAXP))
            {
                return;
            }

            _stream.Position = _tableDirectories[TableNames.MAXP].Offset;
            _tableOfMAXP = new TableOfMAXP(reader);
        }

        private void ReadHEAD(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.HEAD))
            {
                return;
            }

            _stream.Position = _tableDirectories[TableNames.HEAD].Offset;
            _tableOfHEAD = new TableOfHEAD(reader);
        }

        private void ReadHHEA(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.HHEA))
            {
                return;
            }

            _stream.Position = _tableDirectories[TableNames.HHEA].Offset;
            _tableOfHHEA = new TableOfHHEA(reader);
        }

        private void ReadHMTX(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.HMTX))
            {
                return;
            }

            _stream.Position = _tableDirectories[TableNames.HMTX].Offset;
            _tableOfHMTX = new TableOfHMTX(reader, _tableOfHHEA.NumberOfHMetrics, _tableOfMAXP.NumGlyphs);
        }

        private void ReadOS2(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.OS2))
            {
                return;
            }

            _stream.Position = _tableDirectories[TableNames.OS2].Offset;
            _tableOfOS2 = new TableOfOS2(reader);
        }

        private void ReadVHEA(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.VHEA))
            {
                return;
            }

            _stream.Position = _tableDirectories[TableNames.VHEA].Offset;
            _tableOfVHEA = new TableOfVHEA(reader);
        }

        private void ReadMORT(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.MORT))
            {
                return;
            }

            _stream.Position = _tableDirectories[TableNames.MORT].Offset;
            _tableOfMORT = new TableOfMORT(reader);
        }

        private void ReadGSUB(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GSUB))
            {
                return;
            }

            _stream.Position = _tableDirectories[TableNames.GSUB].Offset;
            _tableOfGSUB = new TableOfGSUB(reader);
        }

        private void ReadGPOS(TypefaceReader reader)
        {
            if (!_tableDirectories.ContainsKey(TableNames.GPOS))
            {
                return;
            }

            _stream.Position = _tableDirectories[TableNames.GPOS].Offset;
            _tableOfGPOS = new TableOfGPOS(reader);
        }

        private void ReadTypeface(Stream stream)
        {
            using (var reader = new TypefaceReader(stream))
            {
                ReadDirectory(reader);
                ReadCMAP(reader);
                ReadMAXP(reader);
                ReadHEAD(reader);
                ReadHHEA(reader);
                ReadHMTX(reader);
                ReadOS2(reader);
                ReadVHEA(reader);
                ReadMORT(reader);
                ReadGSUB(reader);
                ReadGPOS(reader);
            }
        }

        private void ReadDirectory(TypefaceReader reader)
        {
            SfntVersionMajor = reader.ReadUInt16();
            SfntVersionMinor = reader.ReadUInt16();
            NumTables = reader.ReadUInt16();
            SearchRange = reader.ReadUInt16();
            EntrySelector = reader.ReadUInt16();
            RangeShift = reader.ReadUInt16();

            for (int i = 1; i <= NumTables; i++)
            {
                string tableName = reader.ReadCharArray(4);
                var td = new TableDirectory(
                    tableName,
                    reader.ReadUInt32(),
                    reader.ReadUInt32(),
                    reader.ReadUInt32());

                _tableDirectories.Add(tableName, td);
            }
        }
    }
}
