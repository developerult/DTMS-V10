using System;
using NGL.FM.DAT.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using DAL = Ngl.FreightMaster.Data;

namespace NGL.FM.DAT
{
	public class Search : DAT
	{
		public Search(ConfiguredProperties properties) : base(properties) {}

		protected override bool RequireDistinctUserAccounts
		{
			get { return true; }
		}

        protected override DTO.DATResults Execute(DTO.tblLoadTender lt, DAL.WCFParameters oWCF)
		{
            var datReturn = new DTO.DATResults();
            datReturn.Success = true;

            DateTime when = DateTime.Now;
			PostAssetRequest sampleLoad = Post.BuildLoad(lt);

			// build a search request based on the load to be posted
			CreateSearchRequest searchRequest = BuildSearch(sampleLoad);

			SessionFacade session1;
			SessionFacade session2;
            if (Account1FailsLogin(lt.UserSecurityControl, lt.UserName, lt.LoadTenderControl, oWCF, out session1) || Account2FailsLogin(lt.UserSecurityControl, lt.UserName, lt.LoadTenderControl, oWCF, out session2))
			{
				string s = "Could not execute DAT Search because the one of the Logins Failed.";
                var p = new string[] { s };
                datReturn.AddMessage(DTO.DATResults.MessageType.Warnings, DTO.DATResults.MessageEnum.E_DATGeneralRetMsg, p);
                datReturn.Success = false;
                datReturn.LTControl = lt.LoadTenderControl;
                datReturn.UserName = lt.UserName;
                datReturn.LTBookControl = lt.LTBookControl;
                datReturn.LTCarrierName = lt.LTCarrierName;
                datReturn.LTCarrierNumber = lt.LTCarrierNumber;
                datReturn.LTBookSHID = lt.LTBookSHID;
			}
			else
			{
                //session1.DeleteAllAssets();
                session1.Post(sampleLoad, lt, oWCF);
				session2.Search(searchRequest);
				//result = Result.Success;
                //strRes = "Search executed successfully.";
			}

			//return result;
            return datReturn;
		}

        public static CreateSearchRequest BuildSearch(PostAssetRequest assetRequest)
        {
            Availability derivedAvailability = DeriveAvailability(assetRequest);
            Dimensions derivedDimensions = DeriveDimensions(assetRequest);

            var origin = new SearchArea { stateProvinces = new[] { StateProvince.CA, StateProvince.IL, StateProvince.WI } };
            //var orig = new SearchArea { zones = new[] { Zone.Central } };

            var destination = new SearchArea { zones = new[] { Zone.Canada } };
            //var dest = new Open { };

            var searchCriteria = new SearchCriteria
            {
                ageLimitMinutes = 15,
                ageLimitMinutesSpecified = true,
                assetType = AssetType.Shipment,
                availability = derivedAvailability,
                destination = new GeoCriteria { Item = destination },
                equipmentClasses = new[] { EquipmentClass.Flatbeds, EquipmentClass.Reefers, EquipmentClass.Containers },
                includeFulls = true,
                includeFullsSpecified = true,
                includeLtls = true,
                includeLtlsSpecified = true,
                limits = derivedDimensions,
                origin = new GeoCriteria { Item = origin }
            };

            var createSearchOperation = new CreateSearchOperation
            {
                criteria = searchCriteria,
                includeSearch = true,
                includeSearchSpecified = true,
                sortOrder = SortOrder.Closest,
                sortOrderSpecified = true
            };

            return new CreateSearchRequest { createSearchOperation = createSearchOperation };
        }

        private static Availability DeriveAvailability(PostAssetRequest assetRequest)
        {
            Availability availability = assetRequest.postAssetOperations[0].availability;
            DateTime timeEarlierThanLoadsEarliest = availability.earliest.AddHours(-1);
            //DateTime timeLaterThanLoadsLatest = availability.latest.AddHours(15);
            return new Availability
            {
                earliest = timeEarlierThanLoadsEarliest,
                earliestSpecified = true,
                latest = availability.latest,
                latestSpecified = true
            };
        }

        private static Dimensions DeriveDimensions(PostAssetRequest assetRequest)
        {
            Dimensions assetDimensions = assetRequest.postAssetOperations[0].dimensions;
            return new Dimensions
            {
                heightInches = assetDimensions.heightInches,
                heightInchesSpecified = false,
                lengthFeet = assetDimensions.lengthFeet + 10,
                lengthFeetSpecified = false,
                volumeCubicFeet = assetDimensions.volumeCubicFeet,
                volumeCubicFeetSpecified = true,
                weightPounds = assetDimensions.weightPounds,
                weightPoundsSpecified = true
            };
        }

	}
}