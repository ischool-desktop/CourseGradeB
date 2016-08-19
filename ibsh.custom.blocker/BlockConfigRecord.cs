using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ibsh.custom.blocker
{
    [TableName("ibsh.custom.blocker.config")]
    class BlockConfigRecord : ActiveRecord
    {
        private static BlockConfigRecord _Instance;
        public static BlockConfigRecord Instance
        {
            get
            {
                if (_Instance == null)
                {
                    AccessHelper accessHelper = new AccessHelper();
                    var list = accessHelper.Select<BlockConfigRecord>();
                    if (list.Count == 1)
                    {
                        _Instance = list[0];
                    }
                    else if (list.Count > 1)
                    {
                        _Instance = list[0];
                        for (int i = 1; i < list.Count; i++)
                        {
                            list[i].Deleted = true;
                        }
                        list.SaveAll();
                    }
                    else
                    {
                        _Instance = new BlockConfigRecord();
                    }
                }
                return _Instance;
            }
        }

        /// <summary>
        /// 開始時間
        /// </summary>
        [FISCA.UDT.Field(Field = "start_time", Indexed = true)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        [FISCA.UDT.Field(Field = "end_time", Indexed = true)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 說明
        /// </summary>
        [FISCA.UDT.Field(Field = "memo", Indexed = false)]
        public string Memo { get; set; }
    }
}
