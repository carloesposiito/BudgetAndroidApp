using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.Model
{
    public class Movement
    {
        int id;
        float value;
        string description;
        string date;
        string type;
        int walletId;

        public Movement() { }

        public Movement(int id, float value, string description, string date, string type, int walletId)
        {
            this.Id = id;
            this.Value = value;
            this.Description = description;
            this.Date = date;
            this.Type = type;
            this.WalletId = walletId;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get => id; set => id = value; }
        public float Value { get => value; set => this.value = value; }
        public string Description { get => description; set => description = value; }
        public string Date { get => date; set => date = value; }
        public string Type { get => type; set => type = value; }
        public int WalletId { get => walletId; set => walletId = value; }

        public override string ToString()
        {
            return $"{value} €, {description}, {date}, Type: {type}, Wallet: {walletId}";
        }
    }
}
