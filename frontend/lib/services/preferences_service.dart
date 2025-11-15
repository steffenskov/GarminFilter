import 'package:shared_preferences/shared_preferences.dart';
import '../models/garmin_device.dart';

class PreferencesService {
  static const String _deviceIdKey = 'device_id';
  static const String _paidKey = 'paid';
  static const String _excludedPermissionsKey = 'excluded_permissions';
  static const String _selectedOrderByKey = 'selected_order_by';

  // Save selected device
  static Future<void> saveSelectedDevice(int? deviceId) async {
    final prefs = await SharedPreferences.getInstance();
    if (deviceId != null) {
      await prefs.setInt(_deviceIdKey, deviceId);
    } else {
      await prefs.remove(_deviceIdKey);
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
  static Future<void> saveSelectedOrderBy(String? orderBy) async {
    final prefs = await SharedPreferences.getInstance();
    if (orderBy == null)
      await prefs.remove(_selectedOrderByKey);
    else
      await prefs.setString(_selectedOrderByKey, orderBy);
  }

  // Load selected device
  static Future<GarminDevice?> loadSelectedDevice(List<GarminDevice> devices) async {
    final prefs = await SharedPreferences.getInstance();
    var deviceId = prefs.getInt(_deviceIdKey);
    if (deviceId != null) {
      try {
        return devices.firstWhere((device) => device.id == deviceId);
      } catch (e) {
        return null;
      }
    }
    return null;
  }

  // Save include paid preference
  static Future<void> savePaid(bool? paid) async {
    final prefs = await SharedPreferences.getInstance();
    if (paid == null)
      await prefs.remove(_paidKey);
    else
      await prefs.setBool(_paidKey, paid == true);
  }

  // Load include paid preference
  static Future<bool> loadPaid() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getBool(_paidKey) ?? false; // Default to false
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
    await prefs.remove(_deviceIdKey);
    await prefs.remove(_paidKey);
    await prefs.remove(_excludedPermissionsKey);
    await prefs.remove(_selectedOrderByKey);
  }
}
