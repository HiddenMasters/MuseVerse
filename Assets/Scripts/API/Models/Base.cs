using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace API.Models
{
    // Attendance Serializer
    [Serializable]
    public class AttendanceSerializer
    {
        public int profile;
        public DateTime attendance_date;
    }

    [Serializable]
    public class AttendancesSerializer
    {
        public AttendanceSerializer[] attendances;
    }

    // Auth Serializer
    [Serializable]
    public class AuthLoginSerializer
    {
        public string username;
        public string password;

        public AuthLoginSerializer(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
    
    [Serializable]
    public class AuthUserCreateSerializer
    {
        public string username;
        public string password;
        public string email;
        public string nickname;

        public AuthUserCreateSerializer(string username, string password, string email, string nickname)
        {
            this.username = username;
            this.password = password;
            this.email = email;
            this.nickname = nickname;
        }
    }

    [Serializable]
    public class AuthRenameSerializer
    {
        public string nickname;

        public AuthRenameSerializer(string nickname)
        {
            this.nickname = nickname;
        }
    }

    [Serializable]
    public class AuthUserSerializer
    {
        public int id;
        public string username;
        public string email;
    }

    [Serializable]
    public class AuthProfileSerializer
    {
        public int id;
        public string nickname;
        public float money;
    }

    [Serializable]
    public class AuthUserProfileSerializer
    {
        public int id;
        public DateTime created_at;
        public DateTime updated_at;
        public string username;
        public string email;
        public AuthProfileSerializer profile;
    }

    [Serializable]
    public class TokenSerializer
    {
        public string authorization;
    }

    // Exhibition Serializer
    [Serializable]
    public class ExhibitionCreateSerializer
    {
        public int item;
        public float price;
        public int hall;
        public int num;

        public ExhibitionCreateSerializer(int item, float price, int hall, int num)
        {
            this.item = item;
            this.price = price;
            this.hall = hall;
            this.num = num;
        }
    }

    [Serializable]
    public class ExhibitionSerializer
    {
        public int id;
        public ItemSerializer item;
        public TradeSerializer trade;
        public int owner;
        public int hall;
        public int num;
        public DateTime expire;
    }

    [Serializable]
    public class ExhibitionsSerializer
    {
        public ExhibitionSerializer[] exhibitions;
    }

    [Serializable]
    public class ExhibitionDetailSerializer
    {
        public ItemDetailSerializer item;
        public TradeDetailSerializer trade;
        public string owner;
        public DateTime expire;
    }
    
    // Item Serializer
    [Serializable]
    public class ItemSerializer
    {
        public int id;
        public string name;
        public int author;
        public string format;
        public string upload;
    }

    [Serializable]
    public class ItemsSerializer
    {
        public ItemSerializer[] items;
    }

    [Serializable]
    public class ItemDetailSerializer
    {
        public int id;
        public string name;
        public string author;
        public string format;
        public string upload;
        public DateTime created_at;
    }
    
    // PrivateRoom Serializer
    [Serializable]
    public class PrivateExhibitionCreateSerializer
    {
        public int item;
        public int num;

        public PrivateExhibitionCreateSerializer(int item, int num)
        {
            this.item = item;
            this.num = num;
        }
    }

    [Serializable]
    public class PrivateExhibitionSerializer
    {
        public int id;
        public int item;
        public int num;
    }

    [Serializable]
    public class PrivateExhibitionDetailSerializer
    {
        public string owner;
        public ItemDetailSerializer item;
    }
    
    // Trade Serializer
    [Serializable]
    public class TradeCreateSerializer
    {
        public int item;
        public float price;

        public TradeCreateSerializer(int item, float price)
        {
            this.item = item;
            this.price = price;
        }
    }

    [Serializable]
    public class TradeSerializer
    {
        public int id;
        public int seller;
        public ItemDetailSerializer item;
        public int buyer;
        public DateTime expire;
        public float price;
        public bool is_sell;
    }

    [Serializable]
    public class TradesSerializer
    {
        public TradeSerializer[] histories;
    }

    [Serializable]
    public class TradeChangePriceSerializer
    {
        public float price;

        public TradeChangePriceSerializer(float price)
        {
            this.price = price;
        }
    }

    [Serializable]
    public class TradeDetailSerializer
    {
        public int id;
        public string seller;
        public ItemDetailSerializer item;
        public string buyer;
        public DateTime expire;
        public float price;
        public bool is_sell;
    }
    
}