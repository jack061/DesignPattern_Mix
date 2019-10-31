using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RM.Common.DotNetCode;

namespace RM.Busines.Busines
{
    /// <summary>
    /// 根据key值生成序列号
    ///author:zy
    /// </summary>
    public class Sequence
    {
        /// <summary>
        /// 根据键值获取序列号
        /// </summary>
        /// <param name="sequencename">名称</param>
        /// <returns></returns>
        public static int getSequence(String sequencename) 
        {
            int sequencevalue =0;
            StringBuilder sql = new StringBuilder();
            sql.Append("update ").Append("BASE_SEQUENCE").Append(" set SEQUENCEVALUE= SEQUENCEVALUE+SequenceStep ").Append(" where SequenceName='").Append(sequencename).Append("'");
            DataFactory.SqlDataBase().ExecuteBySql(sql);
            sql = new StringBuilder("");
            sql.Append("select SequenceValue from ").Append("BASE_SEQUENCE").Append(" where SequenceName='").Append(sequencename).Append("'");
            SqlParam[] param = new SqlParam[]{};
            sequencevalue = DataFactory.SqlDataBase().getInt(sql, param, "SequenceValue");
            if (sequencevalue <= 0) 
            {
                createSequenceKey(sequencename);
                sequencevalue = getSequence(sequencename);
            }
            return sequencevalue;
        }

        /// <summary>
        /// 根据键值创建序列
        /// </summary>
        /// <param name="sequencename"></param>
        private static void createSequenceKey(String sequencename)
        {
            try
            {
                SqlParam[] param = new SqlParam[]{};
                int count = DataFactory.SqlDataBase().getInt((new StringBuilder("select count(*)as c from BASE_SEQUENCE where SequenceName='")).Append(sequencename).Append("'"), param, "c");
                if (count == 0)
                {
                    StringBuilder sql = (new StringBuilder("insert into BASE_SEQUENCE(SequenceName,SequenceValue,SequenceStep)values('")).Append(sequencename).Append("',0,1)");
                    DataFactory.SqlDataBase().ExecuteBySql(sql);
                }
            }
            catch (Exception e)
            {
                
            }
        }

        /// <summary>
        /// 根据key值获取序列号后根据位数前导补零
        /// </summary>
        /// <param name="sequenceName">序列号key</param>
        /// <param name="num">位数  不足左补零</param>
        /// <returns></returns>
        public static String getSequenceAutoAddZero(String sequenceName, int num)
        {
            String sequenceValue = getSequence(sequenceName).ToString();
            for (int i = sequenceValue.Trim().Length; i < num; i++)
            {
                sequenceValue = "0" + sequenceValue;
            }
            return sequenceValue;
        }
    }
}
