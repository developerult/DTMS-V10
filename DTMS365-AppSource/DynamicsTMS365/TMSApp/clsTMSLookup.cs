using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using LTS = Ngl.FreightMaster.Data.LTS;

namespace DynamicsTMS365.TMSApp
{
    /// <summary>
    /// This class should not be instanciated directly.  Callers should reference the instance, globalvLookup, in the Utilities class
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.0 on 2/21/2017
    ///     container for Global lists stored in Application State in Utilities class
    ///     
    /// </remarks>
    public class clsGlobalTMSLookup
    {

       
        private Dictionary<Tuple<DAL.NGLLookupDataProvider.StaticLists,DAL.NGLLookupDataProvider.ListSortType, object, int>, vLookUpDict> _StaticLists;
        public Dictionary<Tuple<DAL.NGLLookupDataProvider.StaticLists, DAL.NGLLookupDataProvider.ListSortType, object, int>, vLookUpDict> StaticLists {
            get {
                if (_StaticLists == null){
                    _StaticLists = new Dictionary<Tuple<DAL.NGLLookupDataProvider.StaticLists, DAL.NGLLookupDataProvider.ListSortType, object, int>, vLookUpDict>();
                }
                return _StaticLists;
            }
            set { _StaticLists = value; }
        }
        
        private Dictionary<Tuple<DAL.NGLLookupDataProvider.GlobalDynamicLists, DAL.NGLLookupDataProvider.ListSortType, object, int>, vLookUpDict> _GlobalDynamicLists;
        public Dictionary<Tuple<DAL.NGLLookupDataProvider.GlobalDynamicLists, DAL.NGLLookupDataProvider.ListSortType, object, int>, vLookUpDict> GlobalDynamicLists
        {
            get
            {
                if (_GlobalDynamicLists == null)
                {
                    _GlobalDynamicLists = new Dictionary<Tuple<DAL.NGLLookupDataProvider.GlobalDynamicLists, DAL.NGLLookupDataProvider.ListSortType, object, int>, vLookUpDict>();
                }
                return _GlobalDynamicLists;
            }
            set { _GlobalDynamicLists = value; }
        }


        /// <summary>
        /// Return an array or records used to display data in a dropdown list
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortKey"></param>
        /// <param name="Criteria"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR on 2/1/2019 for v-8.2
        ///   fixed bug where lookup list with out a unique control number (like PayCodes)
        ///   were not saved to dictionary correclty.  We now read these types of lists
        ///   From the db each time and do not try to save a copy in memory.
        /// Modified by RHR for v-8.2.1.004 on 11/23/2019
        /// added thread safe lock on list objects to prevent key already exists error
        /// Modified by RHR for v-8.3.0.002 on 10/23/2020
        ///   we no longer store any lookup lists in memory on the server
        ///  Modified by RHR for v-8.3.0.002 on 12/01/2020
        ///     We found several issue with the list storeage
        ///     1. we need to refresh the entier list async after each call so the next refresh will have current data
        ///     2. we found that the dictionary data only keys off of StaticLists id,  this does not provide a unique list
        ///        now we need to add a complex key using a Touble with four values
        ///        (a) StaticLists id int
        ///        (b) ListSortType  -- each sorted list is a different list
        ///        (c) Criteria -- each criteria is a different list
        ///        (d) UserControl -- each user needs a seperate copy because of security
        ///             some lists are not (yet) updated with user data but we will still 
        ///             apply the dictionary filter to ensure future compatibility       
        /// </remarks>
        public DTO.vLookupList[] getStaticvLookupList(DAL.NGLLookupDataProvider.StaticLists id, DAL.NGLLookupDataProvider.ListSortType sortKey = DAL.NGLLookupDataProvider.ListSortType.Name, object Criteria = null, DAL.WCFParameters Parameters = null)
        {
            DTO.vLookupList[] results = new DTO.vLookupList[] { };
            if (Parameters == null) { Parameters = Utilities.DALWCFParameters; }           
            DAL.NGLLookupDataProvider dalLookup = new DAL.NGLLookupDataProvider(Parameters);
            Tuple<DAL.NGLLookupDataProvider.StaticLists, DAL.NGLLookupDataProvider.ListSortType, object, int> listKey = Tuple.Create(id, sortKey, Criteria, Parameters.UserControl);
            //results = FillStaticvLookupList(dalLookup, id, sortKey, Criteria, Parameters);
            if (StaticLists.ContainsKey(listKey))
            {
                vLookUpDict container = StaticLists[listKey];
                results = container.ToArray();
                // refresh async
                var originalSynchronizationContext = System.Threading.SynchronizationContext.Current;
                try
                {
                    System.Threading.SynchronizationContext.SetSynchronizationContext(null);
                    ExecFillStaticvLookupListAsync(dalLookup, id, sortKey, Criteria, Parameters);
                }
                finally
                {
                    System.Threading.SynchronizationContext.SetSynchronizationContext(originalSynchronizationContext);
                }
            }
            else
            {
                // run synchronously
                results = FillStaticvLookupList(dalLookup, id, sortKey, Criteria, Parameters);
            }
            return results;
        }

        /// <summary>
        /// async wrapper for FillStaticvLookupList.
        /// </summary>
        /// <param name="dalLookup"></param>
        /// <param name="id"></param>
        /// <param name="sortKey"></param>
        /// <param name="Criteria"></param>
        /// <param name="Parameters"></param>
        /// <remarks>
        /// Created by RHR for v-8.3.0.002 on 12/01/2020
        ///     new logic to refresh static list async using Task.Run
        ///     this creates a new thread on the CPU
        ///     Developer Note.  to run the code async in the same thread
        ///     just use await without Task.Run  usefull when reading data from a web service
        /// </remarks>
        public async void ExecFillStaticvLookupListAsync(DAL.NGLLookupDataProvider dalLookup, DAL.NGLLookupDataProvider.StaticLists id, DAL.NGLLookupDataProvider.ListSortType sortKey = DAL.NGLLookupDataProvider.ListSortType.Name, object Criteria = null, DAL.WCFParameters Parameters = null)
        {
            //DTO.vLookupList[] results = await Task.Run(() => FillStaticvLookupList(dalLookup, id, sortKey, Criteria, Parameters));
            await Task.Run(() => FillStaticvLookupList(dalLookup, id, sortKey, Criteria, Parameters));
            //await  FillStaticvLookupList(dalLookup, id, sortKey, Criteria, Parameters);

        }

        /// <summary>
        /// Loads a vLookup list dictionary with fresh data
        /// </summary>
        /// <param name="dalLookup"></param>
        /// <param name="id"></param>
        /// <param name="sortKey"></param>
        /// <param name="Criteria"></param>
        /// <param name="ModDate"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        /// <remarks>
        ///  Created by RHR for v-8.3.0.002 on 12/012020
        ///    This function can be called synchronously or asynch via ExecFillStaticvLookupListAsync
        ///    it inserts or updates the requested StaticLists with new data.
        ///    When data already exists the caller should call ExecFillStaticvLookupListAsync after reading
        ///    the current data.  Users will get any updates on the next refresh.
        ///    we no long use the ModDate data instead all data is refreshed based on the key
        /// </remarks>
        public DTO.vLookupList[] FillStaticvLookupList(DAL.NGLLookupDataProvider dalLookup, DAL.NGLLookupDataProvider.StaticLists id, DAL.NGLLookupDataProvider.ListSortType sortKey = DAL.NGLLookupDataProvider.ListSortType.Name, object Criteria = null, DAL.WCFParameters Parameters = null)
        {
            DTO.vLookupList[] results = new DTO.vLookupList[] { };           
            vLookUpDict container = new vLookUpDict();
            //get the data from the db
            if (dalLookup == null)
            {
                if (Parameters == null) { Parameters = Utilities.DALWCFParameters; }
                dalLookup = new DAL.NGLLookupDataProvider(Parameters);
            }
            Tuple<DAL.NGLLookupDataProvider.StaticLists, DAL.NGLLookupDataProvider.ListSortType, object, int> listKey = Tuple.Create(id, sortKey, Criteria, Parameters.UserControl);
            lock (StaticLists)
            {              
                //get an updated list of data
                results = dalLookup.GetViewLookupStaticList(id, sortKey, Criteria, null);

                if (results != null && results.Any(x => x.Control > 0))
                {
                    // we can only save to the dictionary if the lookup data has numeric control numbers 
                    // if not we must read from the database each time because the vLookUpDict uses the 
                    // control number for each record as a unique key in the item dictionary                     
                    container.ReplaceItems(results);
                    container.intEnumVal = (int)id;
                    if (StaticLists.ContainsKey(listKey))
                    {
                        StaticLists[listKey] = container;
                    }
                    else
                    {
                        StaticLists.Add(listKey, container);
                    }
                } 
            }
            return results;
        }


        /// <summary>
        /// Overload which always goes to the database for a filtered list of records.  is not stored in memory
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortKey"></param>
        /// <param name="Criteria"></param>
        /// <returns></returns>
        public DTO.vLookupList[] getGlobalDynamicvLookupList(DAL.NGLLookupDataProvider.GlobalDynamicLists id, DAL.NGLLookupDataProvider.ListSortType sortKey = DAL.NGLLookupDataProvider.ListSortType.Name, object Criteria = null, DAL.WCFParameters Parameters = null)
        {
            DTO.vLookupList[] results = new DTO.vLookupList[] { };
            if (Parameters == null) { Parameters = Utilities.DALWCFParameters; }
            DAL.NGLLookupDataProvider dalLookup = new DAL.NGLLookupDataProvider(Parameters);
            Tuple<DAL.NGLLookupDataProvider.GlobalDynamicLists, DAL.NGLLookupDataProvider.ListSortType, object, int> listKey = Tuple.Create(id, sortKey, Criteria, Parameters.UserControl);
            if (GlobalDynamicLists.ContainsKey(listKey))
            {                
                vLookUpDict container = GlobalDynamicLists[listKey];
                results = container.ToArray();
                // refresh async
                
                var originalSynchronizationContext = System.Threading.SynchronizationContext.Current;
                try
                {
                    System.Threading.SynchronizationContext.SetSynchronizationContext(null);
                    ExecFillGlobalDynamicvLookupListAsync(dalLookup, id, sortKey, Criteria, Parameters);
                }
                finally
                {
                    System.Threading.SynchronizationContext.SetSynchronizationContext(originalSynchronizationContext);
                }
            }
            else
            {                
                // run synchronously
                results = FillGlobalDynamicvLookupList(dalLookup, id, sortKey, Criteria, Parameters);
            }
            return results;
        }

        
        public async void ExecFillGlobalDynamicvLookupListAsync(DAL.NGLLookupDataProvider dalLookup, DAL.NGLLookupDataProvider.GlobalDynamicLists id, DAL.NGLLookupDataProvider.ListSortType sortKey = DAL.NGLLookupDataProvider.ListSortType.Name, object Criteria = null, DAL.WCFParameters Parameters = null)
        {
            DTO.vLookupList[] results = await Task.Run(() => FillGlobalDynamicvLookupList(dalLookup, id, sortKey, Criteria, Parameters));

        }

        public DTO.vLookupList[] FillGlobalDynamicvLookupList(DAL.NGLLookupDataProvider dalLookup, DAL.NGLLookupDataProvider.GlobalDynamicLists id, DAL.NGLLookupDataProvider.ListSortType sortKey = DAL.NGLLookupDataProvider.ListSortType.Name, object Criteria = null, DAL.WCFParameters Parameters = null)
        {
            DTO.vLookupList[] results = new DTO.vLookupList[] { };
            //if we do not have a list be sure mod date filter is null (get all)
            vLookUpDict container = new vLookUpDict();
            //get the data from the db
            if (dalLookup == null)
            {
                if (Parameters == null) { Parameters = Utilities.DALWCFParameters; }
                dalLookup = new DAL.NGLLookupDataProvider(Parameters);
            }
            Tuple<DAL.NGLLookupDataProvider.GlobalDynamicLists, DAL.NGLLookupDataProvider.ListSortType, object, int> listKey = Tuple.Create(id, sortKey, Criteria, Parameters.UserControl);

            lock (GlobalDynamicLists)
            {
                //get an updated list of data
                results = dalLookup.GetViewLookupGlobalDynamicList(id, sortKey, Criteria, null);

                if (results != null && results.Any(x => x.Control > 0))
                {
                    // we can only save to the dictionary if the lookup data has numeric control numbers 
                    // if not we must read from the database each time because the vLookUpDict uses the 
                    // control number for each record as a unique key in the item dictionary    
                    container.ReplaceItems(results);
                    container.intEnumVal = (int)id;               
                    if (GlobalDynamicLists.ContainsKey(listKey))
                    {
                        GlobalDynamicLists[listKey] = container;
                    }
                    else
                    {
                        GlobalDynamicLists.Add(listKey, container);
                    }
                }
            }
            return results;
        }

        public void Clear()
        {
            _StaticLists = new Dictionary<Tuple<DAL.NGLLookupDataProvider.StaticLists, DAL.NGLLookupDataProvider.ListSortType, object, int>, vLookUpDict>();
            _GlobalDynamicLists = new Dictionary<Tuple<DAL.NGLLookupDataProvider.GlobalDynamicLists, DAL.NGLLookupDataProvider.ListSortType, object, int>, vLookUpDict>();
        }
    }

    public class clsUserLists
    {
        private Dictionary<int , clsUserTMSLookup> _UserLists;

        public Dictionary<int , clsUserTMSLookup> UserLists
        {
            get
            {
                if (_UserLists == null)
                {
                    _UserLists = new Dictionary<int , clsUserTMSLookup>();
                }
                return _UserLists;
            }
            set { _UserLists = value; }
        }

        /// <summary>
        /// Reads and returns the TMS User Lookup list
        /// </summary>
        /// <param name="UserControl"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2.1.004 on 11/23/2019
        /// added thread safe lock on list objects to prevent key already exists error
        /// Note: this function only returns a clsUserTMSLookup data if the Userlist contains the usercontrol
        ///     even when we add the usercontrol to the list the results are not updated.
        ///     this may be a bug but we cannot determine the impact so no changes have been made to the logic
        /// </remarks>
        public clsUserTMSLookup getUserTMSLookup(int UserControl)
        {
            clsUserTMSLookup results = new clsUserTMSLookup();
            
            /// Modified by RHR for v-8.2.1.004 on 11/23/2019
            /// added thread safe lock on list objects to prevent key already exists error
           lock(UserLists)
            {
                if (!UserLists.ContainsKey(UserControl)) {

                    UserLists.Add(UserControl, results);
                    return results;
                }else
                {
                    results = UserLists[UserControl];
                }  
            }
                     

            return results;
        }
    }
    public class clsUserTMSLookup
    {
       
       

        private Dictionary<DAL.NGLLookupDataProvider.UserDynamicLists, vLookUpDict> _UserDynamicLists;

        public Dictionary<DAL.NGLLookupDataProvider.UserDynamicLists, vLookUpDict> UserDynamicLists
        {
            get
            {
                if (_UserDynamicLists == null)
                {
                    _UserDynamicLists = new Dictionary<DAL.NGLLookupDataProvider.UserDynamicLists, vLookUpDict>();
                }
                return _UserDynamicLists;
            }
            set { _UserDynamicLists = value; }
        }

        /// <summary>
        /// Read User Dynamic list data and add to a global dictionary
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortKey"></param>
        /// <param name="Criteria"></param>
        /// <param name="ModDate"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2.1.004 on 11/23/2019
        /// added thread safe lock on list objects to prevent key already exists error
        /// </remarks>
        public DTO.vLookupList[] getUserDynamicvLookupList(DAL.NGLLookupDataProvider.UserDynamicLists id, DAL.NGLLookupDataProvider.ListSortType sortKey = DAL.NGLLookupDataProvider.ListSortType.Name, object Criteria = null, System.DateTime? ModDate = null, DAL.WCFParameters Parameters = null)
        {
            DTO.vLookupList[] results = new DTO.vLookupList[] { };
            //if we do not have a list be sure mod date filter is null (get all)
            if (!UserDynamicLists.ContainsKey(id)) { ModDate = null; }
            //get the data from the db
            if ( Parameters == null ) { Parameters = Utilities.DALWCFParameters; }
            DAL.NGLLookupDataProvider dalLookup = new DAL.NGLLookupDataProvider(Parameters);
            //get any changes since the last ModDate if provided
            results = dalLookup.GetViewLookupUserDynamicList(id, sortKey, Criteria, ModDate);
            if (results != null)
            {
                vLookUpDict nLookup = new vLookUpDict();
                nLookup.UpdateItems(results);
                nLookup.intEnumVal = (int)id;
                //Modified by RHR for v-8.2.1.004 on 11/23/2019
                // added thread safe lock on list objects to prevent key already exists error
                lock (UserDynamicLists)
                {
                    if (UserDynamicLists.ContainsKey(id))
                    {
                        UserDynamicLists[id] = nLookup;
                    }
                    else
                    {
                        UserDynamicLists.Add(id, nLookup);
                    }
                }
                
            }

            return results;
        }

    }



    /// <summary>
    /// class holds a list of one vlookup list in the Items dictionary
    /// </summary>
    /// <remarks>
    /// Created by RHR for v-8.0 on 2/21/2017
    /// </remarks>
    public class vLookUpDict
    {
        public int intEnumVal = 0;
        public DateTime LastModDate { get; set; }
        public Dictionary<int, DTO.vLookupList> Items {get; set;}

        public List<DTO.vLookupList> ToList()
        {      

            if (Items != null ) { return Items.Values.ToList(); } else { return new List<DTO.vLookupList>(); }
        }

        public DTO.vLookupList[] ToArray()
        {
            DTO.vLookupList[] results = new DTO.vLookupList[] { };
            if (Items != null) { results = Items.Values.ToArray(); }
            return results;
        }

        public bool UpdateItems(DTO.vLookupList[] changes)
        {
            bool blnRet = true;
            LastModDate = DateTime.Now;
            if (Items == null) {
                Items = new Dictionary<int, DTO.vLookupList>();

            }
            foreach (DTO.vLookupList change in changes)
            {
                int intKey = change.Control;
                if (Items.Keys.Contains(intKey))
                {
                    Items[intKey] = change;
                } else
                {
                    Items.Add(intKey, change);
                }
            }
            
            return blnRet;
        }

        public void ReplaceItems(DTO.vLookupList[] oData)
        {
            LastModDate = DateTime.Now;            
            Items = new Dictionary<int, DTO.vLookupList>();            
            foreach (DTO.vLookupList line in oData)
            {
                if (!Items.Keys.Contains(line.Control))
                {
                    Items.Add(line.Control, line);
                }
                                  
            }            
        }





    }

}