using Microsoft.Win32;

namespace CustomToolbox.Services;

public static class RegistryService
{
    public static object? GetValue(string keyPath, string valueName, object? defaultValue = null)
    {
        try
        {
            var val = Registry.GetValue(keyPath, valueName, null);
            return val ?? defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    public static bool SetValue(string keyPath, string valueName, object value, RegistryValueKind kind = RegistryValueKind.DWord)
    {
        try
        {
            using var key = Registry.LocalMachine.CreateSubKey(keyPath.Replace(@"HKEY_LOCAL_MACHINE\", ""));
            if (key == null) return false;
            key.SetValue(valueName, value, kind);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool SetCurrentUserValue(string keyPath, string valueName, object value, RegistryValueKind kind = RegistryValueKind.DWord)
    {
        try
        {
            using var key = Registry.CurrentUser.CreateSubKey(keyPath.Replace(@"HKEY_CURRENT_USER\", ""));
            if (key == null) return false;
            key.SetValue(valueName, value, kind);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool DeleteValue(string keyPath, string valueName)
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(keyPath.Replace(@"HKEY_LOCAL_MACHINE\", ""), true);
            if (key == null) return false;
            key.DeleteValue(valueName, false);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool DeleteCurrentUserValue(string keyPath, string valueName)
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(keyPath.Replace(@"HKEY_CURRENT_USER\", ""), true);
            if (key == null) return false;
            key.DeleteValue(valueName, false);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool KeyExists(string keyPath)
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(keyPath.Replace(@"HKEY_LOCAL_MACHINE\", ""));
            return key != null;
        }
        catch
        {
            return false;
        }
    }

    public static bool CurrentUserKeyExists(string keyPath)
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(keyPath.Replace(@"HKEY_CURRENT_USER\", ""));
            return key != null;
        }
        catch
        {
            return false;
        }
    }

    public static int GetIntValue(string keyPath, string valueName, int defaultValue = 0)
    {
        var val = GetValue(keyPath, valueName, defaultValue);
        if (val is int intVal) return intVal;
        if (val is long longVal) return (int)longVal;
        return defaultValue;
    }

    public static int GetCurrentUserIntValue(string keyPath, string valueName, int defaultValue = 0)
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(keyPath.Replace(@"HKEY_CURRENT_USER\", ""));
            var val = key?.GetValue(valueName, defaultValue);
            if (val is int intVal) return intVal;
            if (val is long longVal) return (int)longVal;
            return defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    public static bool GetBoolValue(string keyPath, string valueName, bool defaultValue = false)
    {
        return GetIntValue(keyPath, valueName, defaultValue ? 1 : 0) == 1;
    }

    public static bool GetCurrentUserBoolValue(string keyPath, string valueName, bool defaultValue = false)
    {
        return GetCurrentUserIntValue(keyPath, valueName, defaultValue ? 1 : 0) == 1;
    }
}
