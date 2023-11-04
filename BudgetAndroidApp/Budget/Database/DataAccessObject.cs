using Budget.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Budget.Data
{
    public class DataAccessObject : SQLiteConnection
    {
        readonly SQLiteAsyncConnection database;

        public DataAccessObject(string dbPath) : base(dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Wallet>().Wait();
            database.CreateTableAsync<Movement>().Wait();
        }

        #region "Wallet"

        /// <summary>
        /// Get all wallets
        /// </summary>
        /// <returns></returns>
        public Task<List<Wallet>> GetWalletsAsync()
        {
            return database.Table<Wallet>().ToListAsync();
        }

        /// <summary>
        /// Get a specific wallet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Wallet> GetWalletAsync(int id)
        {
            return database.Table<Wallet>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Update a wallet or save a new one
        /// </summary>
        /// <param name="wallet"></param>
        /// <returns></returns>
        public Task<int> SaveWalletAsync(Wallet wallet)
        {
            if (wallet.Id != 0)
            {
                return database.UpdateAsync(wallet);
            }
            else
            {
                return database.InsertAsync(wallet);
            }
        }

        /// <summary>
        /// Delete a wallet
        /// </summary>
        /// <param name="wallet"></param>
        /// <returns></returns>
        public Task<int> DeleteWalletAsync(Wallet wallet)
        {
            return database.DeleteAsync(wallet);
        }

        #endregion

        #region "Movement"

        /// <summary>
        /// Get all movements
        /// </summary>
        /// <returns></returns>
        public Task<List<Movement>> GetMovementsAsync()
        {
            return database.Table<Movement>().ToListAsync();
        }

        /// <summary>
        /// Get a specific movement
        /// </summary>
        /// <param name="movementId"></param>
        /// <returns></returns>
        public Task<Movement> GetMovementAsync(int movementId)
        {
            return database.Table<Movement>()
                            .Where(i => i.Id == movementId)
                            .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Update a movement or save a new one
        /// </summary>
        /// <param name="movement"></param>
        /// <returns></returns>
        public Task<int> SaveMovementAsync(Movement movement)
        {
            if (movement.Id != 0)
            {
                return database.UpdateAsync(movement);
            }
            else
            {
                return database.InsertAsync(movement);
            }
        }

        /// <summary>
        /// Delete a movement
        /// </summary>
        /// <param name="wallet"></param>
        /// <returns></returns>
        public Task<int> DeleteMovementAsync(Movement movement)
        {
            return database.DeleteAsync(movement);
        }

        #endregion

    }
}
