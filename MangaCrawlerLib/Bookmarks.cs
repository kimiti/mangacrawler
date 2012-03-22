﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MangaCrawlerLib
{
    /// <summary>
    /// Thread safe, copy on write semantic.
    /// </summary>
    public class Bookmarks
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Serie> m_bookmarks = new List<Serie>();

        internal void Load()
        {
            m_bookmarks = Catalog.LoadBookmarks();
        }

        internal void RemoveNotExisted()
        {
            m_bookmarks = (from serie in m_bookmarks
                           where serie.Server.Series.Contains(serie)
                           select serie).ToList();
        }

        private void Save()
        {
            Catalog.SaveBookmarks();
        }

        public void Add(Serie a_serie)
        {
            var copy = m_bookmarks.ToList();
            copy.Add(a_serie);
            m_bookmarks = copy;

            foreach (var chapter in a_serie.Chapters)
                chapter.BookmarkIgnored = true;

            Save();
        }

        public IEnumerable<Serie> List
        {
            get
            {
                return m_bookmarks;
            }
        }

        public void Remove(Serie a_serie)
        {
            var copy = m_bookmarks.ToList();
            copy.Remove(a_serie);
            m_bookmarks = copy;

            Save();
        }

        public IEnumerable<Serie> GetSeriesWithNewChapters()
        {
            return (from serie in m_bookmarks
                    where serie.GetNewChapters().Any()
                    select serie).ToList();
        }
    }
}