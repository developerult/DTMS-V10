using System;
using NGL.FM.DATIntegration.Infrastructure;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;

namespace NGL.FM.DATIntegration
{
	public class Post : DAT
	{
		public Post(ConfiguredProperties properties) : base(properties) {}

		protected override int Execute(DTO.tblLoadTender lt)
		{
			int result;
			PostAssetRequest sampleLoad = BuildLoad(lt);

			SessionFacade session;
			if (Account1FailsLogin(out session))
			{
				result = Result.Failure;
			}
			else
			{
				//session.DeleteAllAssets();
				session.Post(sampleLoad);
				result = Result.Success;
			}
			return result;
		}

        public static PostAssetRequest BuildLoad(DTO.tblLoadTender lt)
        {
            //string refId = when.Millisecond.ToString();
            
            var pcOrig = new PostalCode {code = lt.LTBookOrigZip, country = getCountryCode(lt.LTBookOrigCountry)};
            var pcDest = new PostalCode {code = lt.LTBookDestZip, country = getCountryCode(lt.LTBookDestCountry)};

            var Origin = new NamedPostalCode {city = lt.LTBookOrigCity, stateProvince = getStateProvince(lt.LTBookOrigState), postalCode = pcOrig};
            var Destination = new NamedPostalCode {city = lt.LTBookDestCity, stateProvince = getStateProvince(lt.LTBookDestState), postalCode = pcDest};
            EquipmentType? equip = eParse(lt.LTDATEquipType);
            //FIX THIS FIGURE OUT WHAT TO DO
            if (equip == null)
            {
                return null;
                //return Result.Invalid;
            }

            var shipment = new Shipment
            {
                equipmentType = equip.Value,
                origin = new Place { Item = Origin },
                destination = new Place { Item = Destination },
                rate = new ShipmentRate(),
                //truckStops = new TruckStops()
                truckStops =
                                new TruckStops
                                {
                                    //enhancements = new[] { TruckStopVideoEnhancement.Flash, TruckStopVideoEnhancement.Highlight },
                                    Item = new ClosestTruckStops(),
                                    //posterDisplayName = "12345"
                                }
            };

            var postAssetOperation = new PostAssetOperation
            {
                availability =
                    new Availability
                    {
                        earliest = lt.LTDATEarliestAvailable.Value,
                        earliestSpecified = true,
                        latest = lt.LTDATLastestAvailable.Value,
                        latestSpecified = true
                    },
                comments = new[] {lt.LTDATComment1, lt.LTDATComment2},
                count = 1,
			    countSpecified = true,
                ltlSpecified = false,
                dimensions =
                    new Dimensions
                    {
                        heightInchesSpecified = false,
                        lengthFeetSpecified = false,
                        volumeCubicFeet = lt.LTBookTotalCube,
                        volumeCubicFeetSpecified = true,
                        weightPounds = Convert.ToInt32(lt.LTBookTotalWgt),
                        weightPoundsSpecified = true
                    },
                includeAsset = true,
                includeAssetSpecified = true,
                Item = shipment,
                //postersReferenceId = "test",
                //stops = 0,
                stopsSpecified = false
            };

            return new PostAssetRequest { postAssetOperations = new[] { postAssetOperation } };
        }

	}
}