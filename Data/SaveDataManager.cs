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
        return _persistedLoans[lobbyId];
    }

    public static void AddLobbyLoan(string lobbyId, Loan loan)
    {
        addLoanToJsonData(_configFilePath, lobbyId, loan);

        List<Loan> existingLobbyLoans = _persistedLoans[lobbyId];

        if (existingLobbyLoans == null)
        {
            _persistedLoans.Add(lobbyId, new List<Loan> { loan });
        }
        else
        {
            existingLobbyLoans.Add(loan);
            _persistedLoans[lobbyId] = existingLobbyLoans;
        }
    }

    public static void RemoveLobbyLoan(string lobbyId, string loanId)
    {
        removeLoanFromJsonData(_configFilePath, lobbyId, loanId);

        if (_persistedLoans[lobbyId] != null)
        {
            _persistedLoans[lobbyId].RemoveAll(loan => loan.LoanID == loanId);
        }
    }

    public static void RemoveLobby(string lobbyId)
    {
        removeLobbyFromJsonData(_configFilePath, lobbyId);

        if (_persistedLoans[lobbyId] != null)
        {
            _persistedLoans.Remove(lobbyId);
        }
    }

    public static void UpdateLoan(string lobbyId, Loan loan)
    {
        updateLoanInJsonData(_configFilePath, lobbyId, loan);

        if (_persistedLoans[lobbyId] == null)
        {
            return;
        }

        List<Loan> existingLobbyLoans = _persistedLoans[lobbyId];
        int index = existingLobbyLoans.FindIndex(l => l.LoanID == loan.LoanID);

        if (index == -1)
        {
            return;
        }

        existingLobbyLoans[index] = loan;
        _persistedLoans[lobbyId] = existingLobbyLoans;
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

        Dictionary<string, List<Loan>> jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<Loan>>>(json) ?? new Dictionary<string, List<Loan>>();

        if (!jsonData.ContainsKey(lobbyId))
        {
            jsonData[lobbyId] = new List<Loan>();
        }

        jsonData[lobbyId].Add(loan);

        string updatedJson = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
        File.WriteAllText(filePath, updatedJson);
    }

    private static void updateLoanInJsonData(string filePath, string lobbyId, Loan loan)
    {
        string json = File.ReadAllText(filePath);

        Dictionary<string, List<Loan>> jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<Loan>>>(json) ?? new Dictionary<string, List<Loan>>();

        if (!jsonData.ContainsKey(lobbyId))
        {
            jsonData[lobbyId] = new List<Loan>();
        }

        List<Loan> loans = jsonData[lobbyId];
        int index = loans.FindIndex(l => l.LoanID == loan.LoanID);

        if (index == -1)
        {
            return;
        }

        loans[index] = loan;
        jsonData[lobbyId] = loans;

        string updatedJson = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
        File.WriteAllText(filePath, updatedJson);
    }

    private static void removeLoanFromJsonData(string filePath, string lobbyId, string loanId)
    {
        string json = File.ReadAllText(filePath);

        Dictionary<string, List<Loan>> jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<Loan>>>(json) ?? new Dictionary<string, List<Loan>>();

        if (!jsonData.ContainsKey(lobbyId))
        {
            return;
        }

        jsonData[lobbyId].RemoveAll(loan => loan.LoanID == loanId);

        string updatedJson = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
        File.WriteAllText(filePath, updatedJson);
    }

    private static void removeLobbyFromJsonData(string filePath, string lobbyId)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        string json = File.ReadAllText(filePath);
        Dictionary<string, List<Loan>> jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<Loan>>>(json) ?? new Dictionary<string, List<Loan>>();

        jsonData.Remove(lobbyId);

        string updatedJson = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
        File.WriteAllText(filePath, updatedJson);
    }
}
