using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL = Ngl.FreightMaster.Data;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;

namespace NGL.FM.CarTar
{
    public class MinWeight
    {

        private static object _lock = new object();
        /// <summary>
        /// Gets the most precise minweight record based on region.
        /// </summary>
        /// <param name="bookRev"></param>
        /// <param name="minWeights"></param>
        /// <param name="interlineList"></param>
        /// <param name="Outbound"></param>
        /// <returns></returns>
        public static DTO.CarrTarMinWeight getMostPreciseMinWeightForBookRev(DTO.BookRevenue bookRev,
           DTO.CarrTarMinWeight[] minWeights,
           List<int> interlineList,
           bool Outbound)
        {
            lock (_lock)
            {

                DTO.CarrTarMinWeight mostPrecise = null;
                if (bookRev == null || minWeights == null || minWeights.Count() < 1) { return null; }
                bool blnInterline = interlineList.Contains(bookRev.BookControl);
                string Country;
                string State;
                string City;
                string Zip;
                if (Outbound)
                {
                    Country = bookRev.BookDestCountry;
                    State = bookRev.BookDestState;
                    City = bookRev.BookDestCity;
                    Zip = bookRev.BookDestZip;

                }
                else
                {
                    Country = bookRev.BookOrigCountry;
                    State = bookRev.BookOrigState;
                    City = bookRev.BookOrigCity;
                    Zip = bookRev.BookOrigZip;
                }
                //get class type
                List<int> lClassTypeControl = new List<int>();
                List<string> lClass = new List<string>();
                Dictionary<int, List<string>> dClasses = new Dictionary<int, List<string>>();
                if (bookRev.BookLoads != null && bookRev.BookLoads.Count() > 0)
                {
                    foreach (DTO.BookLoad l in bookRev.BookLoads)
                    {
                        if (l.BookItems != null && l.BookItems.Count() > 0)
                        {
                            foreach (DTO.BookItem i in l.BookItems)
                            {
                                if (!string.IsNullOrWhiteSpace(i.BookItemFAKClass))
                                {
                                    if (!lClassTypeControl.Contains((int)DAL.Utilities.TariffClassType.classFAK))
                                    {
                                        lClassTypeControl.Add((int)DAL.Utilities.TariffClassType.classFAK);
                                    }
                                    List<string> lfClass = new List<string>();
                                    if (!dClasses.ContainsKey((int)DAL.Utilities.TariffClassType.classFAK))
                                    {
                                        lfClass.Add(i.BookItemFAKClass);
                                        dClasses.Add((int)DAL.Utilities.TariffClassType.classFAK, lfClass);
                                    }
                                    else
                                    {
                                        lfClass = dClasses[(int)DAL.Utilities.TariffClassType.classFAK];
                                        lfClass.Add(i.BookItemFAKClass);
                                        dClasses[(int)DAL.Utilities.TariffClassType.classFAK] = lfClass;
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(i.BookItemNMFCClass))
                                {
                                    if (!lClassTypeControl.Contains((int)DAL.Utilities.TariffClassType.classNMFC))
                                    {
                                        lClassTypeControl.Add((int)DAL.Utilities.TariffClassType.classNMFC);
                                    }
                                    List<string> lfClass = new List<string>();
                                    if (!dClasses.ContainsKey((int)DAL.Utilities.TariffClassType.classNMFC))
                                    {
                                        lfClass.Add(i.BookItemNMFCClass);
                                        dClasses.Add((int)DAL.Utilities.TariffClassType.classNMFC, lfClass);
                                    }
                                    else
                                    {
                                        lfClass = dClasses[(int)DAL.Utilities.TariffClassType.classNMFC];
                                        lfClass.Add(i.BookItemNMFCClass);
                                        dClasses[(int)DAL.Utilities.TariffClassType.classNMFC] = lfClass;
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(i.BookItem49CFRCode))
                                {
                                    if (!lClassTypeControl.Contains((int)DAL.Utilities.TariffClassType.class49CFR))
                                    {
                                        lClassTypeControl.Add((int)DAL.Utilities.TariffClassType.class49CFR);
                                    }

                                    List<string> lfClass = new List<string>();
                                    if (!dClasses.ContainsKey((int)DAL.Utilities.TariffClassType.class49CFR))
                                    {
                                        lfClass.Add(i.BookItem49CFRCode);
                                        dClasses.Add((int)DAL.Utilities.TariffClassType.class49CFR, lfClass);
                                    }
                                    else
                                    {
                                        lfClass = dClasses[(int)DAL.Utilities.TariffClassType.class49CFR];
                                        lfClass.Add(i.BookItem49CFRCode);
                                        dClasses[(int)DAL.Utilities.TariffClassType.class49CFR] = lfClass;
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(i.BookItemDOTCode))
                                {
                                    if (!lClassTypeControl.Contains((int)DAL.Utilities.TariffClassType.classDOT))
                                    {
                                        lClassTypeControl.Add((int)DAL.Utilities.TariffClassType.classDOT);
                                    }

                                    List<string> lfClass = new List<string>();
                                    if (!dClasses.ContainsKey((int)DAL.Utilities.TariffClassType.classDOT))
                                    {
                                        lfClass.Add(i.BookItemDOTCode);
                                        dClasses.Add((int)DAL.Utilities.TariffClassType.classDOT, lfClass);
                                    }
                                    else
                                    {
                                        lfClass = dClasses[(int)DAL.Utilities.TariffClassType.classDOT];
                                        lfClass.Add(i.BookItemDOTCode);
                                        dClasses[(int)DAL.Utilities.TariffClassType.classDOT] = lfClass;
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(i.BookItemIATACode))
                                {
                                    if (!lClassTypeControl.Contains((int)DAL.Utilities.TariffClassType.classIATA))
                                    {
                                        lClassTypeControl.Add((int)DAL.Utilities.TariffClassType.classIATA);
                                    }

                                    List<string> lfClass = new List<string>();
                                    if (!dClasses.ContainsKey((int)DAL.Utilities.TariffClassType.classIATA))
                                    {
                                        lfClass.Add(i.BookItemIATACode);
                                        dClasses.Add((int)DAL.Utilities.TariffClassType.classIATA, lfClass);
                                    }
                                    else
                                    {
                                        lfClass = dClasses[(int)DAL.Utilities.TariffClassType.classIATA];
                                        lfClass.Add(i.BookItemIATACode);
                                        dClasses[(int)DAL.Utilities.TariffClassType.classIATA] = lfClass;
                                    }
                                }

                                if (!string.IsNullOrWhiteSpace(i.BookItemMarineCode))
                                {
                                    if (!lClassTypeControl.Contains((int)DAL.Utilities.TariffClassType.classMarine))
                                    {
                                        lClassTypeControl.Add((int)DAL.Utilities.TariffClassType.classMarine);
                                    }

                                    List<string> lfClass = new List<string>();
                                    if (!dClasses.ContainsKey((int)DAL.Utilities.TariffClassType.classMarine))
                                    {
                                        lfClass.Add(i.BookItemMarineCode);
                                        dClasses.Add((int)DAL.Utilities.TariffClassType.classMarine, lfClass);
                                    }
                                    else
                                    {
                                        lfClass = dClasses[(int)DAL.Utilities.TariffClassType.classMarine];
                                        lfClass.Add(i.BookItemMarineCode);
                                        dClasses[(int)DAL.Utilities.TariffClassType.classMarine] = lfClass;
                                    }
                                }
                            }
                        }
                    }
                }
                //check or FAK


                //If Not (String.IsNullOrWhiteSpace(BookItemFAKClass)) Then
                //               .bookitem

                //           End If
                //CarrTarMinWeightClassTypeControl
                //get the minimum weight
                //CarrTarMinWeightClassFrom
                //CarrTarMinWeightClassTo
                //mostPrecise
                List<DTO.CarrTarMinWeight> minwgts =
                (from d in minWeights
                 where
                 (
                    (lClassTypeControl.Count() < 1 || d.CarrTarMinWeightClassTypeControl == 0 || lClassTypeControl.Contains(d.CarrTarMinWeightClassTypeControl))
                    &&
                    (
                        (blnInterline == true && d.CarrTarMinWeightPointTypeControl == (int)DAL.Utilities.PointType.Interline)
                        ||
                        (blnInterline == false && d.CarrTarMinWeightPointTypeControl == (int)DAL.Utilities.PointType.Direct)
                        ||
                        d.CarrTarMinWeightPointTypeControl == (int)DAL.Utilities.PointType.Any
                    )
                    &&
                     (string.IsNullOrWhiteSpace(d.CarrTarMinWeightCountry) || d.CarrTarMinWeightCountry.ToUpper() == Country.ToUpper())
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarMinWeightState) || d.CarrTarMinWeightState.ToUpper() == State.ToUpper())
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarMinWeightCity) || d.CarrTarMinWeightCity.ToUpper() == City.ToUpper())
                     &&
                     (!bookRev.BookDateLoad.HasValue || (!d.CarrTarMinWeightEffDateFrom.HasValue || d.CarrTarMinWeightEffDateFrom.Value.Date <= bookRev.BookDateLoad.Value.Date))
                     &&
                     (!bookRev.BookDateLoad.HasValue || (!d.CarrTarMinWeightEffDateTo.HasValue || d.CarrTarMinWeightEffDateTo.Value.Date >= bookRev.BookDateLoad.Value.Date))
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarMinWeightZipFrom) || string.IsNullOrWhiteSpace(Zip) || Zip.Trim().Substring(0, d.CarrTarMinWeightZipFrom.Trim().Length).ToUpper().CompareTo(d.CarrTarMinWeightZipFrom.Trim().ToUpper()) >= 0) //zip is equal to or follows CarrTarMinChargeZipFrom
                     &&
                     (string.IsNullOrWhiteSpace(d.CarrTarMinWeightZipTo) || string.IsNullOrWhiteSpace(Zip) || Zip.Trim().Substring(0, d.CarrTarMinWeightZipTo.Trim().Length).ToUpper().CompareTo(d.CarrTarMinWeightZipTo.Trim().ToUpper()) <= 0) //zip is equal to or preceeds CarrTarDiscountZipTo
                 )
                 orderby
                    d.CarrTarMinWeightCity descending,
                    d.CarrTarMinWeightState descending,
                    d.CarrTarMinWeightCountry descending,
                    d.CarrTarMinWeightZipFrom descending,
                    d.CarrTarMinWeightZipTo ascending
                 select d).ToList();

                //mostPrecise
                //List<DTO.CarrTarMinWeight> minwgts

                if (minwgts != null && minwgts.Count() > 0)
                {
                    // set mostPrecise to the first record returned
                    mostPrecise = minwgts[0];
                    decimal maxMinVal = 0;
                    foreach (DTO.CarrTarMinWeight w in minwgts)
                    {
                        if (lClassTypeControl.Count() > 0 && w != null && w.CarrTarMinWeightClassTypeControl != 0 && !string.IsNullOrWhiteSpace(w.CarrTarMinWeightClassFrom))
                        {

                            // Dictionary<int, List<string>> dClasses
                            if (dClasses.ContainsKey(w.CarrTarMinWeightClassTypeControl))
                            {

                                List<string> lfClass = dClasses[w.CarrTarMinWeightClassTypeControl];
                                if (lfClass != null && lfClass.Count() > 0)
                                {
                                    foreach (string c in lfClass)
                                    {
                                        int iFrom = 0;
                                        int iTo = 0;
                                        if (int.TryParse(w.CarrTarMinWeightClassFrom, out iFrom))
                                        {
                                            int iVal = 0;
                                            if (int.TryParse(c, out iVal))
                                            {
                                                if (int.TryParse(w.CarrTarMinWeightClassTo, out iTo))
                                                {
                                                    if (iVal >= iFrom && iVal <= iTo)
                                                    {
                                                        if (maxMinVal < w.CarrTarMinWeightPerLoad)
                                                        {
                                                            maxMinVal = w.CarrTarMinWeightPerLoad;
                                                            mostPrecise = w;
                                                        }
                                                    }
                                                }
                                                else if (iVal == iFrom)
                                                {
                                                    if (maxMinVal < w.CarrTarMinWeightPerLoad)
                                                    {
                                                        maxMinVal = w.CarrTarMinWeightPerLoad;
                                                        mostPrecise = w;
                                                    }
                                                }
                                            }
                                            else if (c == w.CarrTarMinWeightClassFrom)
                                            {
                                                if (maxMinVal < w.CarrTarMinWeightPerLoad)
                                                {
                                                    maxMinVal = w.CarrTarMinWeightPerLoad;
                                                    mostPrecise = w;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return mostPrecise;
            }
        }
        private static object _lock2 = new object();
        /// <summary>
        /// select the highest weight calculated based on the following rules:
        /// a.	Minimum weight per shipment.
        /// b.	Minimum weight per pallet multiplied by the number of pallets.
        /// c.	Actual Weight of shipment.
        /// </summary>
        /// <returns></returns>
        public static double selectHighestWeight(decimal CarrTarMinWeightPerPallet,
            double totalPalletsForLoad,
            decimal CarrTarMinWeightPerLoad,
            double totalWgtForLoad)
        {
            lock (_lock2)
            {
                double minWeightPerPallet = Convert.ToDouble(CarrTarMinWeightPerPallet) * totalPalletsForLoad;
                double[] numbers = new double[]{Convert.ToDouble(CarrTarMinWeightPerLoad),
                                            minWeightPerPallet,
                                            totalWgtForLoad};
                double ratedWeight = numbers.Max();//select the highest weight calculated 
                return ratedWeight;
            }
        }

    }
}
