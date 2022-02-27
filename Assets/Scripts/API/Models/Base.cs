using System;
using System.Collections.Generic;

namespace API.Models
{
    [Serializable]
    public class RegisterSerializer
    {
        public string username;
        public string password;
        public string nickname;
        public string gender;
        public string email;
        
        public RegisterSerializer(string username, string password, string nickname, bool gender, string email = null)
        {
            this.username = username;
            this.password = password;
            this.nickname = nickname;
            this.gender = gender ? "male" : "female";
            this.email = email;
        }
    }
    
    [Serializable]
    public class LoginSerializer
    {
        public string username;
        public string password;

        public LoginSerializer(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    [Serializable]
    public class AuthenticationSerializer
    {
        public string authorization;
    }

    [Serializable]
    public class TradeSerializer
    {
        public int item;
        public float orderPrice;
        public float immediatePrice;

        public TradeSerializer(int item, float orderPrice, float immediatePrice)
        {
            this.item = item;
            this.orderPrice = orderPrice;
            this.immediatePrice = immediatePrice;
        }
    }
    
    [Serializable]
    public class ExhibitionTradeSerializer
    {
        public int id;
        public string owner;
        public int item;
        public float orderPrice;
        public float immediatePrice;

        public ExhibitionTradeSerializer(int id, string owner, int item, float orderPrice, float immediatePrice)
        {
            this.id = id;
            this.owner = owner;
            this.item = item;
            this.orderPrice = orderPrice;
            this.immediatePrice = immediatePrice;
        }
    }

    [Serializable]
    public class TradeItemSerializer
    {
        public int id;
        public int owner;
        public int item;
        public DateTime expire;
        public float order_price;
        public float immediate_price;
        public bool is_sell;
    }

    [Serializable]
    public class TradeListSerializer
    {
        public List<TradeItemSerializer> ItemSerializers;
    }
    
    [Serializable]
    public class TradeExtendSerializer
    {
        public int extendDays;

        public TradeExtendSerializer(int extendDays)
        {
            this.extendDays = extendDays;
        }
    }

    [Serializable]
    public class ItemSerializer
    {
        public string name;
        public string format;
        public float price;
        public int id;
        public int author;
        public string upload;

        public ItemSerializer(string name, string format, float price, int id, int author, string upload)
        {
            this.name = name;
            this.format = format;
            this.price = price;
            this.id = id;
            this.author = author;
            this.upload = upload;
        }
    }
    
    [Serializable]
    public class ExhibitionItemSerializer
    {
        public string name;
        public float price;
        public int id;
        public string author;

        public ExhibitionItemSerializer(string name, float price, int id, string author)
        {
            this.name = name;
            this.price = price;
            this.id = id;
            this.author = author;
        }
    }

    [Serializable]
    public class InventorySerializer
    {
        public int id;
        public string upload;

        public InventorySerializer(int id, string upload)
        {
            this.id = id;
            this.upload = upload;
        }
    }

    [Serializable]
    public class InventoriesSerializer
    {
        public InventorySerializer[] inventories;
    }

    [Serializable]
    public class SampleItemSerializer
    {
        public int id;
        public string name;
        public float price;
        public string upload;
    }
    
    [Serializable]
    public class ExhibitionInventorySerializer
    {
        public SampleItemSerializer item;
        public DateTime expire;
    }

    [Serializable]
    public class ExhibitionInventoriesSerializer
    {
        public ExhibitionInventorySerializer[] exhibitionInventories;
    }
    
    [Serializable]
    public class ItemsURLSerializer
    {
        public string[] urls;
    }

    [Serializable]
    public class OrderCreateSerializer
    {
        public int item;
        public int trade;
        public string status = "buy";

        public OrderCreateSerializer(int item, int trade, string status)
        {
            this.item = item;
            this.trade = trade;
            this.status = status;
        }
    }
    [Serializable]
    public class OrderSerializer
    {
        public int buyer;
        public int item;
        public float price;
        public int trade;
        public string status;

        public OrderSerializer(int buyer, int item, float price, int trade, string status)
        {
            this.buyer = buyer;
            this.item = item;
            this.price = price;
            this.trade = trade;
            this.status = status;
        }
    }

    [Serializable]
    public class ExhibitionSerializer
    {
        public ExhibitionItemSerializer item;
        public ExhibitionTradeSerializer trade;
        public int hall;
        public int num;
        public DateTime expire;
        public int max_widht;
        public int max_height;

        public ExhibitionSerializer(ExhibitionItemSerializer item, ExhibitionTradeSerializer trade, int hall, int num, DateTime expire, int maxWidht, int maxHeight)
        {
            this.item = item;
            this.trade = trade;
            this.hall = hall;
            this.num = num;
            this.expire = expire;
            max_widht = maxWidht;
            max_height = maxHeight;
        }
    }
}