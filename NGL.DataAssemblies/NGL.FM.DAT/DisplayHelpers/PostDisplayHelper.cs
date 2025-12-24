namespace NGL.FM.DAT.DisplayHelpers
{
	public class PostDisplayHelper : DisplayHelper<PostAssetSuccessData>
	{
		public PostDisplayHelper(PostAssetSuccessData data) : base(data) {}

		protected override void Display(int indent)
		{
			DisplayLabel("Post Asset Success Data", indent);
			Display(Data.asset, indent + 1);
			Display(Data.alarm, indent + 1);
		}

		private static void Display(global::Alarm data, int indent)
		{
			if (data == null)
			{
				return;
			}

			DisplayLabel("Alarm", indent);
			DisplayValue("Alarm Id", data.alarmId, indent + 1);
			Display(data.alarmCriteria, indent + 1);
			DisplayValue("Basis Asset Id", data.basisAssetId, indent + 1);
			DisplayValue("Basis Asset Posters Reference Id", data.basisAssetPostersReferenceId, indent + 1);
			DisplayValue("Matches Remaining", data.matchesRemaining, indent + 1);
			if (data.matchingAssetIds != null)
			{
				foreach (string id in data.matchingAssetIds)
				{
					DisplayValue("Matching Asset Id", id, indent + 1);
				}
				DisplayValue("Max Matches", data.maxMatches, indent + 1);
			}
		}

		private static void Display(AlarmSearchCriteria data, int indent)
		{
			if (data == null)
			{
				return;
			}

			DisplayLabel("Alarm Search Criteria", indent);
			if (data.ageLimitMinutesSpecified)
			{
				DisplayValue("Age Limit Minutes", data.ageLimitMinutes, indent + 1);
			}
			if (data.lifetimeMinutesSpecified)
			{
				DisplayValue("Lifetime Minutes Specified", data.lifetimeMinutes, indent + 1);
			}
			if (data.maxMatchesSpecified)
			{
				DisplayValue("Max Matches Specified", data.maxMatches, indent + 1);
			}
			if (data.notifyAllApplicationsSpecified)
			{
				DisplayValue("Notify All Applications Specified", data.notifyAllApplications, indent + 1);
			}
			Display(data.destinationRadius, indent + 1, "Destination Radius");
			Display(data.originRadius, indent + 1, "Origin Radius");
			DisplayValue("Reference Id", data.referenceId, indent + 1);
		}

        public static void DisplayP(PostAssetRequest d, int indent)
        {
            PostAssetOperation data = d.postAssetOperations[0];
            var shipment = data.Item as Shipment;

            if (data == null)
            {
                return;
            }

            Display(shipment.destination, indent + 1, "Destination");
            Display(shipment.origin, indent + 1, "Origin");
            var eq = shipment.equipmentType;
            DisplayValue("Equipment Type", eq, indent + 1);
            
            DisplayLabel("Rate", indent + 1);
            var rate = shipment.rate;
            DisplayValue("Rate Type", rate.rateBasedOn, indent + 1);
            DisplayValue("Rate Dollars", rate.baseRateDollars, indent + 1);
            if (rate.rateMilesSpecified)
            {
                DisplayValue("Rate Miles", rate.rateMiles, indent + 1);
            }

            DisplayLabel("TruckStops", indent + 1);
            var truckStops = shipment.truckStops;
            DisplayValue("Enhancements", truckStops.enhancements, indent + 1);
            DisplayValue("Poster Display Name", truckStops.posterDisplayName, indent + 1);
            DisplayValue("Item", truckStops.Item, indent + 1);

            DisplayLabel("Post Asset Request", indent);
            Display(data.availability, indent + 1);
            if (data.countSpecified)
            {
                DisplayValue("Count", data.count, indent + 1);
            }
            if (data.ltlSpecified)
            {
                DisplayValue("LTL", data.ltl, indent + 1);
            }
            if (data.stopsSpecified)
            {
                DisplayValue("Stops", data.stops, indent + 1);
            }
            Display(data.dimensions, indent + 1);
            Display(data.availability, indent + 1);
            DisplayValue("Poster Ref ID",data.postersReferenceId, indent + 1);
            
            if (data.comments != null)
            {
                DisplayLabel("Comments", indent + 1);
                for (int i = 0; i < data.comments.Length; i++)
                {
                    int oneBasedIndexForDisplay = i + 1;
                    DisplayValue("Comment " + oneBasedIndexForDisplay, data.comments[i], indent + 1 + 1);
                }
            }

           
        }




	}
}