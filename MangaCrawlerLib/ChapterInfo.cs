﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Threading;
using System.Diagnostics;
using TomanuExtensions;
using TomanuExtensions.Utils;

namespace MangaCrawlerLib
{
    public class ChapterInfo
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ChapterState m_state;

        public TaskInfo Task;
        public string URL { get; private set; }
        public SerieInfo Serie { get; private set; }
        public string Title { get; private set; }
        
        internal ChapterInfo(SerieInfo a_serie, string a_url, string a_title)
        {
            Serie = a_serie;
            URL = HttpUtility.HtmlDecode(a_url);

            Title = a_title.Trim();
            Title = Title.Replace("\t", " ");
            while (Title.IndexOf("  ") != -1)
                Title = Title.Replace("  ", " ");
            Title = HttpUtility.HtmlDecode(Title);

            //m_state = DownloadManager.Downloaded.WasDownloaded(this) ?
            //    ChapterState.WasDownloaded : ChapterState.Initial;
            m_state = ChapterState.Initial;
        }

        public TaskInfo FindTask()
        {
            foreach (var task in DownloadManager.Tasks)
            {
                if (task.Chapter == this)
                    return task;
            }

            return null;
        }

        public ChapterState State
        {
            get
            {
                if (Task == null)
                    return m_state;
                else
                {
                    switch (Task.State)
                    {
                        case TaskState.Aborted: return ChapterState.Aborted;
                        case TaskState.Deleting: return ChapterState.Deleting;
                        case TaskState.Downloaded: return ChapterState.Downloaded;
                        case TaskState.Downloading: return ChapterState.Downloading;
                        case TaskState.Error: return ChapterState.Error;
                        case TaskState.Waiting: return ChapterState.Waiting;
                        case TaskState.Zipping: return ChapterState.Zipping;
                        default: throw new NotImplementedException();
                    }
                }
            }
           
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Serie, Title);
        }

        internal bool DownloadRequired
        {
            get
            {
                return (State == ChapterState.Error) ||
                       (State == ChapterState.Initial) &&
                       (State == ChapterState.Aborted);
            }
        }
    }
}
