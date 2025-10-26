import 'package:shared_preferences/shared_preferences.dart';
import '../models/garmin_device.dart';

class PreferencesService {
  static const String _selectedDeviceIdKey = 'selected_device_id';
  static const String _selectedDeviceNameKey = 'selected_device_name';
  static const String _includePaidKey = 'include_paid';
  static const String _excludedPermissionsKey = 'excluded_permissions';
  static const String _selectedOrderByKey = 'selected_order_by';
  
  // Save selected device
  static Future<void> saveSelectedDevice(GarminDevice? device) async {
    final prefs = await SharedPreferences.getInstance();
    if (device != null) {
      await prefs.setInt(_selectedDeviceIdKey, device.id);
      await prefs.setString(_selectedDeviceNameKey, device.name);
    } else {
      await prefs.remove(_selectedDeviceIdKey);
      await prefs.remove(_selectedDeviceNameKey);
    }
  }

  // Load selected order by
  static Future<String?> loadSelectedOrderBy(List<String> orderByOptions) async {
    final prefs = await SharedPreferences.getInstance();
    final orderBy = prefs.getString(_selectedOrderByKey);
    if (orderBy != null) {
       try {
        return orderByOptions.firstWhere((option) => option == orderBy);
      } catch (e) {
        // orderBy not found in current list, return null
        return null;
      }
    }
    
    return null;
  }

  // Save selected order by
  static Future<void> saveSelectedOrderBy(String orderBy) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setString(_selectedOrderByKey, orderBy);
  }


  // Load selected device
  static Future<GarminDevice?> loadSelectedDevice(List<GarminDevice> devices) async {
    final prefs = await SharedPreferences.getInstance();
    final deviceId = prefs.getInt(_selectedDeviceIdKey);
    final deviceName = prefs.getString(_selectedDeviceNameKey);
    
    if (deviceId != null && deviceName != null) {
      // Try to find the device in the current list
      try {
        return devices.firstWhere((device) => device.id == deviceId && device.name == deviceName);
      } catch (e) {
        // Device not found in current list, return null
        return null;
      }
    }
    return null;
  }

  // Save include paid preference
  static Future<void> saveIncludePaid(bool includePaid) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setBool(_includePaidKey, includePaid);
  }

  // Load include paid preference
  static Future<bool> loadIncludePaid() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getBool(_includePaidKey) ?? false; // Default to false
  }

  // Save excluded permissions
  static Future<void> saveExcludedPermissions(List<String> excludedPermissions) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setStringList(_excludedPermissionsKey, excludedPermissions);
  }

  // Load excluded permissions
  static Future<List<String>> loadExcludedPermissions() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getStringList(_excludedPermissionsKey) ?? [];
  }

  // Clear all preferences
  static Future<void> clearAllPreferences() async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.remove(_selectedDeviceIdKey);
    await prefs.remove(_selectedDeviceNameKey);
    await prefs.remove(_includePaidKey);
    await prefs.remove(_excludedPermissionsKey);
    await prefs.remove(_selectedOrderByKey);
  }
}
