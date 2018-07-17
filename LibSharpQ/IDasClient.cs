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

        Task<Signal> SendSignal(Signal signal);
        Task<IReadOnlyList<Signal>> GetSignals(bool retrieveAll);
        Task DeleteSignal(int signalId);
        Task DeleteAllSignals();
        Task DeleteSignals(IEnumerable<int> signalIds);
    }
}
