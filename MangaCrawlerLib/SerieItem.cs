﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MangaCrawlerLib;
using System.Diagnostics;

namespace MangaCrawlerLib
{
    public class SerieItem
    {
        public readonly SerieInfo SerieInfo;

        private Object m_lock = new Object();
        private int m_progress;
        private ItemState m_state;

        public SerieItem(SerieInfo a_info)
        {
            SerieInfo = a_info;
            Initialize();
        }

        public int Progress
        {
            get
            {
                return m_progress;
            }
            set
            {
                lock (m_lock)
                {
                    m_progress = value;
                }
            }
        }

        public void Initialize()
        {
            lock (m_lock)
            {
                m_progress = 0;
                m_state = ItemState.Initial;
            }
        }

        public override string ToString()
        {
            lock (m_lock)
            {
                return String.Format("name: {0}, state: {1}", SerieInfo.Name, m_state);
            }
        }

        public bool DownloadRequired
        {
            get
            {
                return (m_state == ItemState.Error) || (m_state == ItemState.Initial);
            }
        }

        public ItemState State
        {
            get
            {
                return m_state;
            }
            set
            {
                lock (m_lock)
                {
                    m_state = value;
                }
            }
        }
    }
}