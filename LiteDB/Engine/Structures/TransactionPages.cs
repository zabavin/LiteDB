﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace LiteDB.Engine
{
    /// <summary>
    /// Represent a simple structure to store added/removed pages in a transaction. One instance per transaction
    /// </summary>
    internal class TransactionPages
    {
        /// <summary>
        /// Get how many pages are involved in this transaction across all snapshots
        /// </summary>
        public int TransactionSize { get; set; } = 0;

        /// <summary>
        /// Contains all dirty pages already persist in WAL (used in all snapshots). Store in [uint, PagePosition] to reuse same method in save pages into wal and get saved page positions on wal
        /// </summary>
        public Dictionary<uint, PagePosition> DirtyPagesWal { get; private set; } = new Dictionary<uint, PagePosition>();

        /// <summary>
        /// Handle created pages during transaction (for rollback) - Is a list because order is important
        /// </summary>
        public List<uint> NewPages { get; private set; } = new List<uint>();

        /// <summary>
        /// First deleted pageID 
        /// </summary>
        public uint FirstDeletedPageID { get; set; } = uint.MaxValue;

        /// <summary>
        /// Last deleted pageID
        /// </summary>
        public uint LastDeletedPageID { get; set; } = uint.MaxValue;

        /// <summary>
        /// Get deleted page count
        /// </summary>
        public int DeletedPages { get; set; }

        /// <summary>
        /// New collections added in this transaction
        /// </summary>
        public Dictionary<string, uint> NewCollections { get; set; } = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Deleted collections in this transaction
        /// </summary>
        public HashSet<string> DeletedCollections { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }
}