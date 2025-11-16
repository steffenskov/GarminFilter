import 'package:shared_preferences/shared_preferences.dart';

import '../models/garmin_device.dart';

final class _Device {
  static const String _deviceIdKey = 'device_id';
  static const String _deviceNameKey = 'device_name'; // Need both Id and Name, as Garmin have multiple devices with the same underlying Id (e.g. Epix == Quatix)

  Future<void> saveAsync(GarminDevice? device) async {
    final prefs = await SharedPreferences.getInstance();

    if (device != null) {
      await prefs.setInt(_deviceIdKey, device.id);
      await prefs.setString(_deviceNameKey, device.name);
    } else {
      await prefs.remove(_deviceIdKey);
      await prefs.remove(_deviceNameKey);
    }
  }

  Future<GarminDevice?> loadAsync(List<GarminDevice> devices) async {
    final prefs = await SharedPreferences.getInstance();

    var deviceId = prefs.getInt(_deviceIdKey);
    var deviceName = prefs.getString(_deviceNameKey);
    if (deviceId != null && deviceName != null) {
      try {
        return devices.firstWhere((device) => device.id == deviceId && device.name == deviceName);
      } catch (e) {
        return null;
      }
    }
    return null;
  }
}

final class _Paid {
  static const String _key = 'paid';

  Future<void> saveAsync(bool? paid) async {
    final prefs = await SharedPreferences.getInstance();
    if (paid != null) {
      await prefs.setBool(_key, paid);
    } else {
      await prefs.remove(_key);
    }
  }

  Future<bool?> loadAsync() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getBool(_key);
  }
}

final class _Permissions {
  static const String _key = 'excluded_permissions';

  Future<void> saveAsync(List<String> excludedPermissions) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setStringList(_key, excludedPermissions);
  }

  Future<List<String>> loadAsync() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getStringList(_key) ?? [];
  }
}

final class _OrderBy {
  static const String _key = 'selected_order_by';

  Future<String?> loadAsync(List<String> orderByOptions) async {
    final prefs = await SharedPreferences.getInstance();
    final orderBy = prefs.getString(_key);
    if (orderBy != null) {
      try {
        return orderByOptions.firstWhere((option) => option == orderBy);
      } catch (e) {
        return null;
      }
    }

    return null;
  }

  Future<void> saveAsync(String? orderBy) async {
    final prefs = await SharedPreferences.getInstance();
    if (orderBy == null) {
      await prefs.remove(_key);
    } else {
      await prefs.setString(_key, orderBy);
    }
  }
}

final class _Developer {
  static const String _key = 'developer';

  Future<String?> loadAsync(List<String> developers) async {
    final prefs = await SharedPreferences.getInstance();
    var selectedDeveloper = prefs.getString(_key);
    if (selectedDeveloper != null) {
      try {
        return developers.firstWhere((developer) => developer == selectedDeveloper);
      } catch (e) {
        return null;
      }
    }
    return null;
  }

  Future<void> saveAsync(String? developer) async {
    final prefs = await SharedPreferences.getInstance();
    if (developer == null) {
      await prefs.remove(_key);
    } else {
      await prefs.setString(_key, developer);
    }
  }
}

class PreferencesService {
  static final device = _Device();
  static final paid = _Paid();
  static final permissions = _Permissions();
  static final orderBy = _OrderBy();
  static final developer = _Developer();
}
