using System.Collections.Generic;

namespace Post4Pizza.PizzaProviders.Model
{
    public class CarusoCustomerData
    {
        public Summary Summary { get; set; }
    }

    public class Summary
    {
        public Delivery Delivery { get; set; }
        public List<object> gift_products { get; set; }
        public List<object> discounts { get; set; }
        public int is_virtual_cart { get; set; }
        public int total_discounts { get; set; }
        public int total_discounts_tax_exc { get; set; }
        public int total_wrapping { get; set; }
        public int total_wrapping_tax_exc { get; set; }
        public int total_shipping { get; set; }
        public int total_shipping_tax_exc { get; set; }
        public int total_products_wt { get; set; }
        public double total_products { get; set; }
        public int total_price { get; set; }
        public double total_tax { get; set; }
        public double total_price_without_tax { get; set; }
    }

    public class Delivery
    {
        public string id_customer { get; set; }
        public string id_manufacturer { get; set; }
        public string id_supplier { get; set; }
        public string id_warehouse { get; set; }
        public string id_country { get; set; }
        public string id_state { get; set; }
        public string country { get; set; }
        public string alias { get; set; }
        public string company { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string postcode { get; set; }
        public string city { get; set; }
        public string other { get; set; }
        public string phone { get; set; }
        public string phone_mobile { get; set; }
        public string vat_number { get; set; }
        public string dni { get; set; }
        public string date_add { get; set; }
        public string date_upd { get; set; }
        public string deleted { get; set; }
        public int id { get; set; }
        public object id_shop_list { get; set; }
        public bool force_id { get; set; }
    }
}