﻿using Game.Entity.Enum;
using Game.Facade;
using Game.Kernel;
using Game.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Game.Web.Module.AppManager
{
    public partial class MobileKindList : AdminPage
    {
        #region 窗口事件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GameKindItemDataBind();
            }
        }

        protected void anpNews_PageChanged(object sender, EventArgs e)
        {
            GameKindItemDataBind();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //判断权限
            AuthUserOperationPermission(Permission.Delete);
            string strQuery = "WHERE KindID in (" + StrCIdList + ")";
            try
            {
                FacadeManage.aidePlatformFacade.DeleteMobileKindItem(strQuery);
                ShowInfo("删除成功");
            }
            catch
            {
                ShowError("删除失败");
            }
            GameKindItemDataBind();
        }

        #endregion

        #region 数据绑定

        //绑定数据
        private void GameKindItemDataBind()
        {
            PagerSet pagerSet = FacadeManage.aidePlatformFacade.GetMobileKindItemList(anpNews.CurrentPageIndex, anpNews.PageSize, SearchItems, Orderby);
            if (pagerSet.PageSet.Tables[0].Rows.Count > 0)
            {
                litNoData.Visible = false;
            }
            else
            {
                litNoData.Visible = true;
            }

            rptMobileKindItem.DataSource = pagerSet.PageSet;
            rptMobileKindItem.DataBind();
            anpNews.RecordCount = pagerSet.RecordCount;
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string SearchItems
        {
            get
            {
                if (ViewState["SearchItems"] == null)
                {
                    StringBuilder condition = new StringBuilder();
                    condition.Append(" WHERE 1=1 ");

                    ViewState["SearchItems"] = condition.ToString();
                }

                return (string)ViewState["SearchItems"];
            }

            set { ViewState["SearchItems"] = value; }
        }

        /// <summary>
        /// 排序条件
        /// </summary>
        public string Orderby
        {
            get
            {
                if (ViewState["Orderby"] == null)
                {
                    ViewState["Orderby"] = "ORDER BY KindID ASC";
                }

                return (string)ViewState["Orderby"];
            }

            set { ViewState["Orderby"] = value; }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取游戏属性
        /// </summary>
        /// <param name="joinID"></param>
        /// <returns></returns>
        protected string GetMarkName(int gameFlag)
        {
            string rValue = "";
            if ((gameFlag & 1) > 0)
            {
                rValue += "ios,";
            }
            if ((gameFlag & 2) > 0)
            {
                rValue += "android,";
            }           

            if (rValue != "")
            {
                rValue = rValue.Substring(0, rValue.Length - 1);
            }
            return rValue;
        }
        #endregion
    }
}