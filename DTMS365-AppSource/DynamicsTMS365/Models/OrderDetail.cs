using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class OrderDetail
    {

    public int BookLoadControl { get; set; }
	public int BookLoadBookControl { get; set; }
	public string BookLoadPONumber { get; set; }
	public int BookItemControl { get; set; }
	public string BookItemItemNumber { get; set; }
	public string BookItemDescription { get; set; }
	public int BookItemQtyOrdered { get; set; }
	public double BookItemWeight { get; set; }
	public int BookItemCube { get; set; }
    public double BookItemPallets { get; set; }
	public string BookItem49CFRCode { get; set; }
	public string BookItemIATACode { get; set; }
	public string BookItemDOTCode { get; set; }
	public string BookItemMarineCode { get; set; }
	public string BookItemNMFCClass { get; set; }
	public string BookItemFAKClass { get; set; }

    }
}