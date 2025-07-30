using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace REPOLoan;

internal static class SaveDataManager
{
    private static string _configFilePath = Path.Combine(Application.persistentDataPath, "REPOModData", "REPOLoan", "LoanData.json");
    private static Dictionary<string, List<Loan>> _persistedLoans;

    public static void Initialize()
    {
        string directory = Path.GetDirectoryName(_configFilePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(_configFilePath))
        {
            _persistedLoans = new Dictionary<string, List<Loan>>();
            string startingJSONString = JsonConvert.SerializeObject(_persistedLoans, Formatting.Indented);

            REPOLoan.Logger.LogInfo("Path: " + _configFilePath);
            File.WriteAllText(_configFilePath, startingJSONString);
        }
        else
        {
            _persistedLoans = loadJsonData(_configFilePath);
        }

        REPOLoan.Logger.LogInfo("Data: " + JsonConvert.SerializeObject(_persistedLoans));
    }

    public static List<Loan> GetLobbyLoans(string lobbyId)
    {
        return _persistedLoans.TryGetValue(lobbyId, out var loans) ? loans : new List<Loan>();
    }

    public static void AddLobbyLoan(string lobbyId, Loan loan)
    {
        addLoanToJsonData(_configFilePath, lobbyId, loan);

        if (!_persistedLoans.TryGetValue(lobbyId, out var existingLobbyLoans))
        {
            _persistedLoans[lobbyId] = new List<Loan> { loan };
        }
        else
        {
            existingLobbyLoans.Add(loan);
        }
    }

    public static void RemoveLobbyLoan(string lobbyId, string loanId)
    {
        removeLoanFromJsonData(_configFilePath, lobbyId, loanId);

        if (_persistedLoans.TryGetValue(lobbyId, out var loans))
        {
            loans.RemoveAll(loan => loan.LoanID == loanId);
        }
    }

    public static void RemoveLobby(string lobbyId)
    {
        removeLobbyFromJsonData(_configFilePath, lobbyId);
        _persistedLoans.Remove(lobbyId); // Safe to call even if key doesn't exist
    }

    public static void UpdateLoan(string lobbyId, Loan loan)
    {
        updateLoanInJsonData(_configFilePath, lobbyId, loan);

        if (!_persistedLoans.TryGetValue(lobbyId, out var existingLobbyLoans))
        {
            return;
        }

        int index = existingLobbyLoans.FindIndex(l => l.LoanID == loan.LoanID);
        if (index != -1)
        {
            existingLobbyLoans[index] = loan;
        }
    }

    private static Dictionary<string, List<Loan>> loadJsonData(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new Dictionary<string, List<Loan>>();
        }

        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<Dictionary<string, List<Loan>>>(json) ?? new Dictionary<string, List<Loan>>();
    }

    private static void addLoanToJsonData(string filePath, string lobbyId, Loan loan)
    {
        string json = File.ReadAllText(filePath);
        var jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<Loan>>>(json) ?? new();

        if (!jsonData.ContainsKey(lobbyId))
        {
            jsonData[lobbyId] = new List<Loan>();
        }

        jsonData[lobbyId].Add(loan);
        File.WriteAllText(filePath, JsonConvert.SerializeObject(jsonData, Formatting.Indented));
    }

    private static void updateLoanInJsonData(string filePath, string lobbyId, Loan loan)
    {
        string json = File.ReadAllText(filePath);
        var jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<Loan>>>(json) ?? new();

        if (jsonData.TryGetValue(lobbyId, out var loans))
        {
            int index = loans.FindIndex(l => l.LoanID == loan.LoanID);
            if (index != -1)
            {
                loans[index] = loan;
                jsonData[lobbyId] = loans;
                File.WriteAllText(filePath, JsonConvert.SerializeObject(jsonData, Formatting.Indented));
            }
        }
    }

    private static void removeLoanFromJsonData(string filePath, string lobbyId, string loanId)
    {
        string json = File.ReadAllText(filePath);
        var jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<Loan>>>(json) ?? new();

        if (jsonData.TryGetValue(lobbyId, out var loans))
        {
            loans.RemoveAll(loan => loan.LoanID == loanId);
            File.WriteAllText(filePath, JsonConvert.SerializeObject(jsonData, Formatting.Indented));
        }
    }

    private static void removeLobbyFromJsonData(string filePath, string lobbyId)
    {
        if (!File.Exists(filePath)) return;

        string json = File.ReadAllText(filePath);
        var jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<Loan>>>(json) ?? new();

        if (jsonData.Remove(lobbyId))
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(jsonData, Formatting.Indented));
        }
    }
}
