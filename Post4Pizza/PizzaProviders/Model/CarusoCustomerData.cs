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
        public List<Product> products { get; set; }
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

    public class Product
    {
        public string id_product_attribute { get; set; }
        public string id_product { get; set; }
        public string cart_quantity { get; set; }
        public string instructions { get; set; }
        public string instructions_valid { get; set; }
        public string instructions_id { get; set; }
        public string id_shop { get; set; }
        public string name { get; set; }
        public string is_virtual { get; set; }
        public string description_short { get; set; }
        public string available_now { get; set; }
        public string available_later { get; set; }
        public string id_category_default { get; set; }
        public string id_supplier { get; set; }
        public string id_manufacturer { get; set; }
        public string on_sale { get; set; }
        public string ecotax { get; set; }
        public string additional_shipping_cost { get; set; }
        public string available_for_order { get; set; }
        public double price { get; set; }
        public string active { get; set; }
        public string unity { get; set; }
        public string unit_price_ratio { get; set; }
        public string quantity_available { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string depth { get; set; }
        public string out_of_stock { get; set; }
        public int weight { get; set; }
        public string date_add { get; set; }
        public string date_upd { get; set; }
        public string quantity { get; set; }
        public string link_rewrite { get; set; }
        public string category { get; set; }
        public string unique_id { get; set; }
        public string id_address_delivery { get; set; }
        public string advanced_stock_management { get; set; }
        public object supplier_reference { get; set; }
        public object id_customization { get; set; }
        public object customization_quantity { get; set; }
        public string price_attribute { get; set; }
        public string ecotax_attr { get; set; }
        public string reference { get; set; }
        public string weight_attribute { get; set; }
        public string ean13 { get; set; }
        public string upc { get; set; }
        public string id_image { get; set; }
        public string legend { get; set; }
        public string minimal_quantity { get; set; }
        public string wholesale_price { get; set; }
        public int reduction_type { get; set; }
        public int stock_quantity { get; set; }
        public int price_without_reduction { get; set; }
        public int price_with_reduction { get; set; }
        public double price_with_reduction_without_tax { get; set; }
        public double total { get; set; }
        public int total_wt { get; set; }
        public int price_wt { get; set; }
        public bool reduction_applies { get; set; }
        public bool quantity_discount_applies { get; set; }
        public bool allow_oosp { get; set; }
        public List<object> features { get; set; }
        public string attributes { get; set; }
        public string attributes_small { get; set; }
        public int rate { get; set; }
        public string tax_name { get; set; }
        public int price_without_quantity_discount { get; set; }
        public string reduction_formatted { get; set; }
        public int quantity_without_customization { get; set; }
        public int price_without_specific_price { get; set; }
        public bool is_discounted { get; set; }
    }
}