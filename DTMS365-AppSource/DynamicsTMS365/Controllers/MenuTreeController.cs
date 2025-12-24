using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;


namespace DynamicsTMS365.Controllers
{
    public class MenuTreeController : NGLControllerBase
    {
        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.MenuTreeController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion

        #region " REST Services"

        [HttpPost, ActionName("ExpandNode")]
        public Models.Response ExpandNode(int id)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                SaveMenuTreeNodeExpanded(id, true);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        [HttpPost, ActionName("CollapseNode")]
        public Models.Response CollapseNode(int id)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                SaveMenuTreeNodeExpanded(id, false);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        [HttpPost, ActionName("UpdateMenuItemCaption")]
        public Models.Response UpdateMenuItemCaption(int itemId, string newCaption, string newCaptionLocal)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLSecurityDataProvider dataProvider = new DAL.NGLSecurityDataProvider(Parameters);
                var item = dataProvider.getMenuTreeDataByMenuTreeControl(itemId);
                if (item == null)
                    throw new NullReferenceException($"menu item with menuTreeControl: {itemId} could not be found.");

                item.MenuTreeCaption = newCaption;
                item.MenuTreeCaptionLocal = newCaptionLocal;
                dataProvider.InsertOrUpdateMenuTreeData(item);
                Utilities.GlobalMenuTreeByUser.Remove(Parameters.UserControl);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        [HttpPost, ActionName("AddMenuItemToFavorites")]
        public Models.Response AddMenuItemToFavorites(int favoritesId, int itemId, int itemPosition)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLSecurityDataProvider dataProvider = new DAL.NGLSecurityDataProvider(Parameters);
                var item = dataProvider.getMenuTreeDataByMenuTreeControl(itemId);
                if (item == null)
                    throw new NullReferenceException($"menu item with menuTreeControl: {itemId} could not be found.");

                item.MenuTreeParentID = favoritesId;
                item.MenuTreeControl = 0;
                item.MenuTreeSequenceNo = itemPosition;
                item.MenuTreeName += "Favorites";

                dataProvider.InsertOrUpdateMenuTreeData(item);
                Utilities.GlobalMenuTreeByUser.Remove(Parameters.UserControl);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        [HttpPost, ActionName("ResetMenu")]
        public Models.Response ResetMenu()
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                var userControl = Parameters.UserControl;
                DAL.NGLSecurityDataProvider dataProvider = new DAL.NGLSecurityDataProvider(Parameters);
                dataProvider.DeleteMenuTreeByUserSecurityControl(userControl);

                Utilities.GlobalMenuTreeByUser.Remove(Parameters.UserControl);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        [HttpPost, ActionName("RemoveMenuItemFromFavorites")]
        public Models.Response RemoveMenuItemFromFavorites(int itemId)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLSecurityDataProvider dataProvider = new DAL.NGLSecurityDataProvider(Parameters);
                dataProvider.RemoveMenuItem(itemId);
                Utilities.GlobalMenuTreeByUser.Remove(Parameters.UserControl);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        [HttpPost, ActionName("ToggleMenuItemVisibility")]
        public Models.Response ToggleMenuItemVisibility(int id)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLSecurityDataProvider dataProvider = new DAL.NGLSecurityDataProvider(Parameters);
                dataProvider.ToggleMenuTreeVisibility(id);
                Utilities.GlobalMenuTreeByUser.Remove(Parameters.UserControl);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        [HttpPost, ActionName("ReorderMenu")]
        public Models.Response ReorderMenu(int parentId, int itemId, int position)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLSecurityDataProvider dataProvider = new DAL.NGLSecurityDataProvider(Parameters);
                dataProvider.UpdateMenuTreePositionData(parentId, itemId, position);
                Utilities.GlobalMenuTreeByUser.Remove(Parameters.UserControl);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        [HttpPost, ActionName("ReorderRootMenu")]
        public Models.Response ReorderRootMenu(int userSecurityControl, int itemId, int position)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            try
            {
                DAL.NGLSecurityDataProvider dataProvider = new DAL.NGLSecurityDataProvider(Parameters);
                dataProvider.UpdateRootMenuTreePositionData(userSecurityControl, itemId, position);
                Utilities.GlobalMenuTreeByUser.Remove(Parameters.UserControl);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        #endregion

        #region " public methods"

        public void SaveMenuTreeNodeExpanded(int id, bool blnExpand)
        {
            if (Parameters.UserControl < 1) { return; }

            DAL.NGLSecurityDataProvider oSec = new DAL.NGLSecurityDataProvider(Parameters);
            string strOld = "";
            string strNew = "";

            if (blnExpand) { strOld = string.Format("mtc: '{0}', expanded: false", id); strNew = string.Format("mtc: '{0}', expanded: true", id); }
            else { strOld = string.Format("mtc: '{0}', expanded: true", id); strNew = string.Format("mtc: '{0}', expanded: false", id); }

            if (Utilities.GlobalMenuTreeByUser.ContainsKey(Parameters.UserControl))
            {
                var gmtbu = Utilities.GlobalMenuTreeByUser[Parameters.UserControl];
                var strMTOrig = gmtbu.strField;
                var strMT = strMTOrig.Replace(strOld, strNew);

                Models.GenericResult gr = new Models.GenericResult { };
                gr.strField = strMT;
                gr.dtField = gmtbu.dtField;
                Utilities.GlobalMenuTreeByUser[Parameters.UserControl] = gr;
            }
            oSec.SaveMenuTreeNodeExpanded(id, blnExpand);
        }

        #endregion

    }
}