using RM.Common.DotNetCode;
using RM.Common.DotNetData;
using RM.Common.DotNetJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using WZX.Busines.Util;

namespace RM.Busines.contract
{
    public class getContractCode : IRequiresSessionState
    {

        #region 获取进出口合同合同编号
        public static string getContractNumber(string buyercode, string sellercode)
        {
            string contractNumber = string.Empty;
            string year = DateTime.Now.Year.ToString().Substring(2, 2);
            string month = Util.Util.getNumberAddZero(DateTime.Now.Month.ToString(), 2);
            //获取后三位编码
            string code = "HT" + year + month;
            string lastThreeNumber = RM.Busines.Util.Util.getSequenceAutoAddZero(code, 3);
            //判断buyercode,sellercode 是否为3200，true转换为HK
            convertHK(ref buyercode, ref sellercode);
            bool b = getPriority(buyercode, sellercode);
            if (b)//买方优先级大于卖方，生成采购合同
            {
                contractNumber = buyercode.Trim() + "GY" + year + month + "-" + lastThreeNumber;
            }
            else
            {
                contractNumber = sellercode.Trim() + "XS" + year + month + "-" + lastThreeNumber;
            }

            return contractNumber;
        }



        #endregion

        #region 转换3200为HK
        public static void convertHK(ref string buyercode, ref string sellercode)
        {
            if (buyercode == ConstantUtil.HK_OLD)
            {
                buyercode = ConstantUtil.HK_NEW;
            }
            if (sellercode == ConstantUtil.HK_OLD)
            {
                sellercode = ConstantUtil.HK_NEW;
            }
        }
        #endregion

        #region 获取优先级信息,买方大于卖方返回true,否则返回false
        public static bool getPriority(string buyercode, string sellercode)
        {
            //查询买卖双方在合同优先级表中是否存在
            StringBuilder sbBuyer = new StringBuilder(@"select priority from EncodingRules where code=@buyercode");
            StringBuilder sbSeller = new StringBuilder(@"select priority from EncodingRules where code=@sellercode");
            string buyerPriority = DataFactory.SqlDataBase().getString(sbBuyer, new SqlParam[1] { new SqlParam("@buyercode", buyercode) }, "priority");
            string sellerPriority = DataFactory.SqlDataBase().getString(sbBuyer, new SqlParam[1] { new SqlParam("@sellercode", sellercode) }, "priority");
            int buyerPriInt = ConvertHelper.ToInt32<string>(buyerPriority, 0);
            int sellerPriInt = ConvertHelper.ToInt32<string>(sellerPriority, 0);
            return buyerPriInt > sellerPriInt ? true : false;
        }
        #endregion

        #region 比较两个相同key的hashtable的不同值，返回新的hashtable
        public static Hashtable getNewHashTableCompare(Hashtable newHash, Hashtable oldHash)
        {
            Hashtable retHash = new Hashtable();
            if (newHash.Keys.Count!=oldHash.Keys.Count)
            {
                 return retHash;
            }
            if (oldHash.ContainsKey("id"))//去除id标识列
            {
                oldHash.Remove("id");
            }
            foreach (string key in newHash.Keys)
            {
                if (!oldHash.ContainsKey(key))
                {
                    return retHash;
                }
                else
                {
                    if (newHash[key].ToString()!=oldHash[key].ToString())
                    {
                        retHash.Add(key, newHash[key].ToString());
                    }
                }

            }
            return retHash;
        }

        //public static Hashtable getNewListHashTableCompare(List<Hashtable>newListHash,List<Hashtable>oldListHash)
        //{
        //   //list集合中数量相等，逐个比对
        //    string s="";
        

        //}
        static bool IsEqual(Hashtable ht1, Hashtable ht2)
        {
            if (ht1.Keys.Count != ht2.Keys.Count)
                return false;
            foreach (string key in ht1.Keys)
            {
                if (!ht2.ContainsKey(key))
                {
                    return false;
                }
                else
                {
                    if (ht1[key].ToString() != ht2[key].ToString())
                        return false;
                }
            } return true;
        }

        #endregion

        #region 获取合同表中的hashtble,与传递过来的更改后的hashtable进行比较,并把生成的新hashtable 插入到数据库中
        public static void InsertHashByCompare(Hashtable newHash,string tableName,string contractNo)
        {

            #region old
            Hashtable htContract = new Hashtable();
            StringBuilder sb = new StringBuilder(string.Format(@"select * from {0} where contractNo='{1}'", tableName, contractNo));
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb, 0);
            if (dt.Rows.Count > 0)
            {
                htContract = DataTableHelper.DataRowToHashTable(dt.Rows[0]);
            }
            Hashtable finalHash = getNewHashTableCompare(newHash, htContract);

            #endregion


        }
        #endregion

    }
}
