﻿// See LICENSE.txt for license information.

namespace VictorBush.Ego.NefsLib.Header
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VictorBush.Ego.NefsLib.Item;

    /// <summary>
    /// Header part 6.
    /// </summary>
    public class Nefs16HeaderPart6
    {
        private readonly Dictionary<Guid, Nefs16HeaderPart6Entry> entriesByGuid;
        private readonly List<Nefs16HeaderPart6Entry> entriesByIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="Nefs16HeaderPart6"/> class.
        /// </summary>
        /// <param name="entries">A list of entries to instantiate this part with.</param>
        internal Nefs16HeaderPart6(IList<Nefs16HeaderPart6Entry> entries)
        {
            this.entriesByIndex = new List<Nefs16HeaderPart6Entry>(entries);
            this.entriesByGuid = new Dictionary<Guid, Nefs16HeaderPart6Entry>(entries.ToDictionary(e => e.Guid));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Nefs16HeaderPart6"/> class from a list of items.
        /// </summary>
        /// <param name="items">The list of items in the archive.</param>
        internal Nefs16HeaderPart6(NefsItemList items)
        {
            this.entriesByIndex = new List<Nefs16HeaderPart6Entry>();
            this.entriesByGuid = new Dictionary<Guid, Nefs16HeaderPart6Entry>();

            // Sort part 6 by item id. Part 1 and part 6 order must match.
            foreach (var item in items.EnumerateById())
            {
                var flags = Nefs16HeaderPart6Flags.None;
                flags |= item.Attributes.IsTransformed ? Nefs16HeaderPart6Flags.IsTransformed : 0;
                flags |= item.Attributes.IsDirectory ? Nefs16HeaderPart6Flags.IsDirectory : 0;
                flags |= item.Attributes.IsDuplicated ? Nefs16HeaderPart6Flags.IsDuplicated : 0;
                flags |= item.Attributes.IsCacheable ? Nefs16HeaderPart6Flags.IsCacheable : 0;
                flags |= item.Attributes.V16Unknown0x10 ? Nefs16HeaderPart6Flags.Unknown0x10 : 0;
                flags |= item.Attributes.IsPatched ? Nefs16HeaderPart6Flags.IsPatched : 0;
                flags |= item.Attributes.V16Unknown0x40 ? Nefs16HeaderPart6Flags.Unknown0x40 : 0;
                flags |= item.Attributes.V16Unknown0x80 ? Nefs16HeaderPart6Flags.Unknown0x80 : 0;

                var entry = new Nefs16HeaderPart6Entry(item.Guid);
                entry.Data0x00_Volume.Value = item.Attributes.Part6Volume;
                entry.Data0x02_Flags.Value = (byte)flags;
                entry.Data0x03_Unknown.Value = item.Attributes.Part6Unknown0x3;

                this.entriesByGuid.Add(item.Guid, entry);
                this.entriesByIndex.Add(entry);
            }
        }

        /// <summary>
        /// Gets entries for each item in the archive, accessible by Guid.
        /// </summary>
        public IReadOnlyDictionary<Guid, Nefs16HeaderPart6Entry> EntriesByGuid => this.entriesByGuid;

        /// <summary>
        /// Gets the list of entries in the order they appear in the header.
        /// </summary>
        public IList<Nefs16HeaderPart6Entry> EntriesByIndex => this.entriesByIndex;
    }
}
