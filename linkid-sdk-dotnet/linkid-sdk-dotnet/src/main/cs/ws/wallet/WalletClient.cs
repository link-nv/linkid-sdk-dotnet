using System;

namespace safe_online_sdk_dotnet
{
    public interface WalletClient
    {
        /// <summary>
        /// Enroll a user for a wallet organization. Optionally specify initial credit to add to the wallet if applicable
        /// </summary>
        /// <param name="userId">the linkID of the user to enroll</param>
        /// <param name="walletOrganizationId">the wallet organization ID</param>
        /// <param name="amount">the optional initial amount</param>
        /// <param name="currency">the optional currency of the initial amount</param>
        /// <returns>the wallet ID</returns>
        /// <exception cref="WalletEnrollException">Something went wrong, check the error code in the exception</exception>
        String enroll(String userId, String walletOrganizationId, double amount, LinkIDCurrency currency);

        /// <summary>
        /// Add credit for specific user/wallet
        /// </summary>
        /// <param name="userId">the linkID user</param>
        /// <param name="walletId">the wallet ID of the linkID user</param>
        /// <param name="amount">amount to add</param>
        /// <param name="currency">currency of amount to add</param>
        /// <exception cref="WalletAddCreditException">Something went wrong, check the error code in the exception</exception>
        void addCredit(String userId, String walletId, double amount, LinkIDCurrency currency);

        /// <summary>
        /// Remove credit for specific user/wallet
        /// </summary>
        /// <param name="userId">the linkID user</param>
        /// <param name="walletId">the wallet ID of the linkID user</param>
        /// <param name="amount">amount to remove</param>
        /// <param name="currency">currency of amount to remove</param>
        /// <exception cref="WalletRemoveCreditException">Something went wrong, check the error code in the exception</exception>
        void removeCredit(String userId, String walletId, double amount, LinkIDCurrency currency);

        /// <summary>
        /// Remove specified wallet
        /// </summary>
        /// <param name="userId">the linkID user</param>
        /// <param name="walletId">the ID of the wallet to be removed</param>
        /// <exception cref="WalletRemoveException">Something went wrong, check the error code in the exception</exception>
        void remove(String userId, String walletId);

        /// <summary>
        /// Commit a wallet transaction. The amount payed by the specified wallet transaction ID will be free'd.
        /// If not committed, linkID will after a period of time release it.
        /// </summary>
        /// <param name="userId">the linkID user</param>
        /// <param name="walletId">the ID of the wallet</param>
        /// <param name="walletTransactionId">the ID of the wallet transaction to commit</param>
        /// <exception cref="WalletRemoveException">Something went wrong, check the error code in the exception</exception>
        void commit(String userId, String walletId, String walletTransactionId);
    }
}
