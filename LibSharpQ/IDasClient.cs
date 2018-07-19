using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LibSharpQ.Models;

namespace LibSharpQ
{
    public interface IDasClient
    {
        string BaseUrl { get; }

        /// <summary>
        /// Send a signal to the API
        /// </summary>
        /// <param name="signal">The signal to send</param>
        /// <returns>The signal response, which includes the ID</returns>
        Task<Signal> SendSignal(Signal signal);

        /// <summary>
        /// Retrieve all signals from the API
        /// </summary>
        /// <param name="retrieveAll">Whether to retrieve all pages of results, or just the first</param>
        /// <returns>A collection of signals that were previously sent</returns>
        Task<IReadOnlyList<Signal>> GetSignals(bool retrieveAll = true);

        /// <summary>
        /// Deletes the signal with the given ID
        /// </summary>
        /// <param name="signalId">The ID of a signal, provided by the API</param>
        Task DeleteSignal(int signalId);

        /// <summary>
        /// Deletes all signals currently active
        /// </summary>
        Task DeleteAllSignals();
        
        /// <summary>
        /// Deletes the signals with the given IDs
        /// </summary>
        /// <param name="signalIds">A collection of IDs for signals to delete</param>
        Task DeleteSignals(IEnumerable<int> signalIds);
    }
}
